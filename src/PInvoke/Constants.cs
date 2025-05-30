namespace Igneous.Launcher.PInvoke;

static class Constants
{
    internal const uint SYNCHRONIZE = 0x00100000;

    internal const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

    internal const uint INFINITE = unchecked((uint)-1);

    internal const int CSTR_EQUAL = 2;

    internal const uint OPEN_EXISTING = 3;

    internal const uint FILE_SHARE_DELETE = 0x00000004;

    internal const nint INVALID_HANDLE_VALUE = -1;

    internal const uint WAIT_TIMEOUT = 0x00000102;

    internal enum FILE_INFO_BY_HANDLE_CLASS { FileStandardInfo = 1 }

    internal const uint DONT_RESOLVE_DLL_REFERENCES = 0x00000001;

    internal const uint PROCESS_ALL_ACCESS = 0X1FFFFF;

    internal const uint THREAD_ALL_ACCESS = 0X1FFFFF;

    internal const uint MEM_RELEASE = 0x00008000;

    internal const uint MEM_COMMIT = 0x00001000;

    internal const uint MEM_RESERVE = 0x00002000;

    internal const uint PAGE_READWRITE = 0x04;

    internal const uint CREATE_SUSPENDED = 0x00000004;

    internal const uint DUPLICATE_SAME_ACCESS = 0x00000002;

    internal const uint TOKEN_QUERY = 0x0008;

    internal enum AppPolicyShowDeveloperDiagnostic
    {
        AppPolicyShowDeveloperDiagnostic_None,
        AppPolicyShowDeveloperDiagnostic_ShowUI
    }

    internal const uint MB_ICONERROR = 0x00000010;

    internal const uint MB_ICONWARNING = 0x00000030;
}