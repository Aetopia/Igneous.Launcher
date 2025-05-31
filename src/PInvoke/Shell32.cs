using System.Runtime.InteropServices;
using System.Security;

namespace Igneous.Launcher.PInvoke;

[SuppressUnmanagedCodeSecurity]
static class Shell32
{
    [DllImport("Shell32", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int ShellMessageBox(nint hAppInst, nint hWnd, string lpcText, string lpcTitle, uint fuStyle);
}