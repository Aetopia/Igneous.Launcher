using System.Runtime.InteropServices;
using System.Security;

namespace Igneous.Launcher.PInvoke;

[SuppressUnmanagedCodeSecurity]
static class User32
{
    [DllImport("User32", CharSet = CharSet.Unicode)]
    internal static extern nint FindWindowEx(nint hWndParent, nint hWndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("User32")]
    internal static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);
}