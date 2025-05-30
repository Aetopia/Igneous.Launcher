using System.Runtime.InteropServices;
using System.Security;

namespace Igneous.Launcher.PInvoke;

using static Constants;

[SuppressUnmanagedCodeSecurity]
unsafe static class Kernel32
{
    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern nint lstrcpy(in ApplicationUserModelId lpString1, string lpString2);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern nint lstrcpy(in PackageFamilyName lpString1, string lpString2);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern uint ExpandEnvironmentStrings(string lpSrc, out Path lpDst, uint nSize);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern int GetPackagesByPackageFamily(in PackageFamilyName packageFamilyName, out uint count, nint packageFullNames, in uint bufferLength, nint buffer);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern int GetPackagesByPackageFamily(in PackageFamilyName packageFamilyName, in uint count, in nint packageFullNames, in uint bufferLength, out PackageFullName buffer);

    [DllImport("Kernel32")]
    internal static extern int GetApplicationUserModelId(nint hProcess, in uint applicationUserModelIdLength, out ApplicationUserModelId applicationUserModelId);

    [DllImport("Kernel32")]
    internal static extern nint OpenProcess(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

    [DllImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool CloseHandle(nint hObject);

    [DllImport("Kernel32")]
    internal static extern int CompareStringOrdinal(in ApplicationUserModelId lpString1, int cchCount1, in ApplicationUserModelId lpString2, int cchCount2, [MarshalAs(UnmanagedType.Bool)] bool bIgnoreCase);

    [DllImport("Kernel32")]
    internal static extern nint CreateFile2(in Path lpFileName, uint dwDesiredAccess, uint dwShareMode, uint dwCreationDisposition, nint pCreateExParams);

    [DllImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetFileInformationByHandleEx(nint hFile, FILE_INFO_BY_HANDLE_CLASS FileInformationClass, out FILE_STANDARD_INFO lpFileInformation, uint dwBufferSize);

    [DllImport("Kernel32")]
    internal static extern uint WaitForSingleObject(nint hHandle, uint dwMilliseconds);

    [DllImport("Kernel32")]
    internal static extern nint VirtualAllocEx(nint hProcess, nint lpAddress, nuint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool VirtualFreeEx(nint hProcess, nint lpAddress, nuint dwSize, uint dwFreeType);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool WriteProcessMemory(nint hProcess, nint lpBaseAddress, string lpBuffer, nuint nSize, nint lpNumberOfBytesWritten);

    [DllImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FreeLibrary(nint hLibModule);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern nint LoadLibraryEx(string lpLibFileName, nint hFile, uint dwFlags);

    [DllImport("Kernel32")]
    internal static extern nint CreateRemoteThread(nint hProcess, nint lpThreadAttributes, nuint dwStackSize, nint lpStartAddress, nint lpParameter, uint dwCreationFlags, nint lpThreadId);

    [DllImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool TerminateThread(nint hThread, uint dwExitCode);

    [DllImport("Kernel32")]
    internal static extern uint ResumeThread(nint hThread);

    [DllImport("Kernel32")]
    internal static extern uint QueueUserAPC(nint pfnAPC, nint hThread, nuint dwData);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern nint GetModuleHandle(string lpModuleName);

    [DllImport("Kernel32", CharSet = CharSet.Ansi)]
    internal static extern nint GetProcAddress(nint hModule, string lpProcName);

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern nint CreateEvent(nint lpEventAttributes, [MarshalAs(UnmanagedType.Bool)] bool bManualReset, [MarshalAs(UnmanagedType.Bool)] bool bInitialState, string lpName);

    [DllImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DuplicateHandle(nint hSourceProcessHandle, nint hSourceHandle, nint hTargetProcessHandle, out nint lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

    [DllImport("Kernel32")]
    internal static extern nint GetCurrentProcess();

    [DllImport("Kernel32")]
    internal static extern nint OpenThread(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwThreadId);

    [DllImport("Kernel32")]
    internal static extern uint WaitForMultipleObjects(uint nCount, nint* lpHandles, [MarshalAs(UnmanagedType.Bool)] bool bWaitAll, uint dwMilliseconds);

    [DllImport("Kernel32")]
    internal static extern uint GetProcessIdOfThread(nint Thread);

    [DllImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool OpenProcessToken(nint ProcessHandle, uint DesiredAccess, out nint TokenHandle);

    [DllImport("Kernel32")]
    internal static extern int AppPolicyGetShowDeveloperDiagnostic(nint processToken, out AppPolicyShowDeveloperDiagnostic policy);
}
