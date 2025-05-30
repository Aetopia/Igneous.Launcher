using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Igneous.Launcher.Minecraft;

using static PInvoke.Constants;
using static PInvoke.Kernel32;

public sealed class Loader
{
    readonly Game _game;

    static readonly string _path = Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName;

    static readonly FileSystemAccessRule _rule = new(new SecurityIdentifier("S-1-15-2-1"), FileSystemRights.FullControl, AccessControlType.Allow);

    internal static readonly nint _closeHandle;

    internal static readonly nint _setEvent;

    internal static readonly nint _loadLibrary;

    static Loader()
    {
        var moduleHandle = GetModuleHandle("Kernel32");
        _closeHandle = GetProcAddress(moduleHandle, "CloseHandle");
        _setEvent = GetProcAddress(moduleHandle, "SetEvent");
        _loadLibrary = GetProcAddress(moduleHandle, "LoadLibraryW");
    }

    internal Loader(Game game) => _game = game;

    public unsafe static void Launch(IReadOnlyList<string> paths)
    {
        /*
            - This function is executed at startup whenever the calling application is attached as a debugger to the game.
            - Ensure there is a launch in progress before injecting startup modifications.
        */

        using Mutex mutex = new(false, "39D92C1A-53D7-4236-91C9-DE1415719559", out var createdNew);
        if (createdNew) return;

        var args = Environment.GetCommandLineArgs();
        for (var _ = 0; _ + 1 < args.Length; _++)
            if (args[_] == "-tid")
            {
                var addresses = new nint[paths.Count];
                nint threadHandle = 0, processHandle = 0, sourceHandle = 0, targetHandle = 0, processToken = 0;

                try
                {
                    threadHandle = OpenThread(THREAD_ALL_ACCESS, false, uint.Parse(args[_ + 1]));
                    processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, GetProcessIdOfThread(threadHandle));

                    /*
                        - Check if the target process is actually a UWP app to avoid unwanted injection.
                        - If not a UWP app just resume the process & return early.
                    */

                    OpenProcessToken(processHandle, TOKEN_QUERY, out processToken);
                    AppPolicyGetShowDeveloperDiagnostic(processToken, out var policy);

                    if (policy != AppPolicyShowDeveloperDiagnostic.AppPolicyShowDeveloperDiagnostic_None)
                    {
                        ResumeThread(threadHandle);
                        break;
                    }

                    /*  
                        - Abuse events to synchorize with the target thread for resource cleanup.
                        - This is done since we do not know the target thread's lifecycle when it is resumed.
                    */

                    sourceHandle = CreateEvent(0, true, false, null);
                    DuplicateHandle(GetCurrentProcess(), sourceHandle, processHandle, out targetHandle, 0, false, DUPLICATE_SAME_ACCESS);

                    /*
                        - Abuse APCs to queue dynamic link library injection requests.
                        - When the target thread is resumed, the APC queue is flushed injecting dynamic link libraries.
                    */

                    for (var index = 0; index < addresses.Length; index++)
                    {
                        var path = Path.GetFullPath(paths[index]);
                        var size = (nuint)(sizeof(char) * path.Length + 1);
                        var address = addresses[index] = VirtualAllocEx(processHandle, 0, size, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
                        WriteProcessMemory(processHandle, address, path, size, 0);
                        QueueUserAPC(_loadLibrary, threadHandle, (nuint)address);
                    }

                    /*
                        - Queue APCs for thread synchorization.
                        - To avoid a deadlock, we wait for either the target thread or event to be signaled.
                    */

                    QueueUserAPC(_setEvent, threadHandle, (nuint)targetHandle);
                    QueueUserAPC(_closeHandle, threadHandle, (nuint)targetHandle);
                    ResumeThread(threadHandle);

                    var handles = stackalloc nint[] { threadHandle, sourceHandle };
                    WaitForMultipleObjects(2, handles, false, INFINITE);
                }
                finally
                {
                    /*
                        - Attempt to resume the thread regardless of the calling application's state. 
                    */

                    ResumeThread(threadHandle);

                    foreach (var address in addresses)
                        VirtualFreeEx(processHandle, address, 0, MEM_RELEASE);

                    CloseHandle(processToken);

                    CloseHandle(sourceHandle);
                    CloseHandle(targetHandle);

                    CloseHandle(processHandle);
                    CloseHandle(threadHandle);
                }
                break;
            }

        Environment.Exit(0);
    }

    public void Launch(IReadOnlyList<string> startup, IReadOnlyList<string> runtime)
    {
        /*
            - Assign ACLs for startup modifications.
        */

        foreach (var path in startup)
            try
            {
                FileInfo info = new(path);
                var security = info.GetAccessControl();
                security.SetAccessRule(_rule);
                info.SetAccessControl(security);
            }
            catch { }

        uint? processId = null;

        /*
            - Only load startup modifications when the game is not running.
        */

        if (!_game.Running)
        {
            /*
                - Create a mutex to signal any new instances of the calling application to inject startup modifications.
                - Attach the calling application as the game's debugger.
            */

            using Mutex mutex = new(false, "39D92C1A-53D7-4236-91C9-DE1415719559", out var createdNew);
            try
            {
                if (createdNew)
                {
                    _game.Debugger = null;
                    _game.Debugger = _path;
                }
                processId = _game.Launch();
            }
            finally { if (createdNew) _game.Debugger = null; }
        }
        else processId = _game.Launch();
        _game.Debug = true;

        if (processId is null) return;

        var addresses = new nint[runtime.Count];
        nint processHandle = 0, threadHandle = 0;

        try
        {
            /*
                - At runtime, we queue a suspended thread that will be used to dynamic link library injection.
                - Since we have control over the target thread's lifecycle, we don't need to use events. 
            */

            processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, (uint)processId);
            threadHandle = CreateRemoteThread(processHandle, 0, 0, _loadLibrary, 0, CREATE_SUSPENDED, 0);

            for (var index = 0; index < addresses.Length; index++)
            {
                FileInfo info = new(runtime[index]);
                try
                {
                    var security = info.GetAccessControl();
                    security.SetAccessRule(_rule);
                    info.SetAccessControl(security);
                }
                catch { }

                /*
                    - Abuse APCs to queue dynamic link library injection requests.
                    - When the target thread is resumed, the APC queue is flushed injecting dynamic link libraries.
                */

                var size = (nuint)(sizeof(char) * info.FullName.Length + 1);
                var address = addresses[index] = VirtualAllocEx(processHandle, 0, size, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
                WriteProcessMemory(processHandle, address, info.FullName, size, 0);
                QueueUserAPC(_loadLibrary, threadHandle, (nuint)address);
            }

            ResumeThread(threadHandle);
            WaitForSingleObject(threadHandle, INFINITE);
        }
        finally
        {
            /*
                - Terminate the suspended thread to allow the target process to terminate correctly.
            */

            TerminateThread(threadHandle, 0);

            foreach (var address in addresses)
                VirtualFreeEx(processHandle, address, 0, MEM_RELEASE);

            CloseHandle(processHandle);
            CloseHandle(threadHandle);
        }
    }
}