using System.Collections.Generic;

using Igneous.Launcher.PInvoke;

namespace Igneous.Launcher.Minecraft;

using static PInvoke.Constants;
using static PInvoke.Kernel32;
using static PInvoke.User32;

internal static class Minecraft
{
    internal static class Release
    {
        internal const string PackageFamilyName = "Microsoft.MinecraftUWP_8wekyb3d8bbwe";

        internal const string Path = $@"%LOCALAPPDATA%\Packages\{PackageFamilyName}\LocalState\games\com.mojang\minecraftpe\resource_init_lock";

        internal const string ApplicationUserModelId = $"{PackageFamilyName}!App";
    }

    internal static class Preview
    {
        internal const string PackageFamilyName = "Microsoft.MinecraftWindowsBeta_8wekyb3d8bbwe";

        internal const string Path = $@"%LOCALAPPDATA%\Packages\{PackageFamilyName}\LocalState\games\com.mojang\minecraftpe\resource_init_lock";

        internal const string ApplicationUserModelId = $"{PackageFamilyName}!App";
    }
}

public sealed class Game
{
    public static readonly Game Release = new(Minecraft.Release.Path, Minecraft.Release.PackageFamilyName, Minecraft.Release.ApplicationUserModelId);

    public static readonly Game Preview = new(Minecraft.Preview.Path, Minecraft.Preview.PackageFamilyName, Minecraft.Preview.ApplicationUserModelId);

    Game(string path, string packageFamilyName, string applicationUserModelId)
    {
        _path = new(path);
        _packageFamilyName = new(packageFamilyName);
        _applicationUserModelId = new(applicationUserModelId);
        _loader = new(this);
    }

    /*
        - We abuse structures to serve as drop in replacements for unmanaged contiguous allocations.
        - This allows us to use unmanaged contiguous allocations in a save context without Span<T> or Memory<T>.
    */

    readonly Path _path;

    readonly PackageFamilyName _packageFamilyName;

    readonly ApplicationUserModelId _applicationUserModelId;

    readonly Loader _loader;

    static readonly IApplicationActivationManager _applicationActivationManager = (IApplicationActivationManager)new ApplicationActivationManager();

    internal static readonly IPackageDebugSettings _packageDebugSettings = (IPackageDebugSettings)new PackageDebugSettings();

    public bool Installed
    {
        get
        {
            GetPackagesByPackageFamily(_packageFamilyName, out var count, 0, 0, 0);
            return count != 0;
        }
    }

    public bool Running
    {
        get
        {
            /*
                - Enumerating windows is faster since we are lazily iterating.
                - We use FindWindowEx() to bypass immersive window filtering.
            */

            nint windowHandle = 0;

            while ((windowHandle = FindWindowEx(0, windowHandle, "MSCTFIME UI", null)) != 0)
            {
                GetWindowThreadProcessId(windowHandle, out var processId);

                var processHandle = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, false, processId);
                try
                {
                    if (GetApplicationUserModelId(processHandle, ApplicationUserModelId.Length, out var applicationUserModelId) != 0) continue;
                    else if (CompareStringOrdinal(_applicationUserModelId, -1, applicationUserModelId, -1, true) == CSTR_EQUAL) return true;
                }
                finally { CloseHandle(processHandle); }
            }

            return false;
        }
    }

    public bool Debug
    {
        set
        {
            GetPackagesByPackageFamily(_packageFamilyName, 1, 0, PackageFullName.Length, out var packageFullName);
            if (value) _packageDebugSettings.EnableDebugging(packageFullName, null, null);
            else _packageDebugSettings.DisableDebugging(packageFullName);
        }
    }

    internal unsafe string Debugger
    {
        set
        {
            GetPackagesByPackageFamily(_packageFamilyName, 1, 0, PackageFullName.Length, out var packageFullName);
            if (string.IsNullOrEmpty(value)) _packageDebugSettings.DisableDebugging(packageFullName);
            else _packageDebugSettings.EnableDebugging(packageFullName, value, null);
        }
    }

    /*
        - Allows the caller to wait for the game to fully initialize.
        - Waiting for "resource_init_lock" to be deleted ensures the caller waits until the game is on the title screen.
    */

    internal unsafe uint? Launch()
    {
        nint fileHandle = CreateFile2(_path, 0, FILE_SHARE_DELETE, OPEN_EXISTING, 0), processHandle = 0;
        try
        {
            if (!Running || fileHandle != INVALID_HANDLE_VALUE)
            {
                _applicationActivationManager.ActivateApplication(_applicationUserModelId, 0, ACTIVATEOPTIONS.AO_NOERRORUI, out var processId);
                processHandle = OpenProcess(SYNCHRONIZE, false, processId);

                while (fileHandle == INVALID_HANDLE_VALUE)
                {
                    if (WaitForSingleObject(processHandle, 1) != WAIT_TIMEOUT) return null;
                    fileHandle = CreateFile2(_path, 0, FILE_SHARE_DELETE, OPEN_EXISTING, 0);
                }

                while (WaitForSingleObject(processHandle, 1) == WAIT_TIMEOUT)
                {
                    GetFileInformationByHandleEx(fileHandle, FILE_INFO_BY_HANDLE_CLASS.FileStandardInfo, out var fileInformation, (uint)sizeof(FILE_STANDARD_INFO));
                    if (fileInformation.DeletePending) return processId;
                }

                return null;
            }
            else
            {
                _applicationActivationManager.ActivateApplication(_applicationUserModelId, default, ACTIVATEOPTIONS.AO_NOERRORUI, out var processId);
                return processId;
            }
        }
        finally
        {
            CloseHandle(processHandle);
            CloseHandle(fileHandle);
        }
    }

    public void Launch(IReadOnlyCollection<string> startup, IReadOnlyCollection<string> runtime) => _loader.Launch([.. startup], [.. runtime]);
}