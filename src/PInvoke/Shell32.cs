using System.Runtime.InteropServices;

namespace Igneous.Launcher.PInvoke;

static class Shell32
{
    [DllImport("Shell32", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int ShellMessageBox(nint hAppInst, nint hWnd, string lpcText, string lpcTitle, uint fuStyle);
}