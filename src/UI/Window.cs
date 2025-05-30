using System.Windows;

namespace Igneous.Launcher.UI;

sealed class Window : System.Windows.Window
{
    internal Window()
    {
        // WindowHelper.SetUseModernWindowStyle(this, true);
        // ThemeManager.SetRequestedTheme(this, ElementTheme.Dark);

        Title = $"Igneous Launcher";
        Width = 960; Height = 540;
        UseLayoutRounding = SnapsToDevicePixels = true;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        ResizeMode = ResizeMode.NoResize;
        Content = new Content(this);
    }
}