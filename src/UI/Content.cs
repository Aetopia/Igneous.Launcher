using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Igneous.Launcher.Configuration;
using Igneous.Launcher.Minecraft;
using Igneous.Launcher.UI.Controls;

namespace Igneous.Launcher.UI;

sealed class Content : Grid
{
    internal Content(Window window)
    {
        ColumnDefinitions.Add(new());
        RowDefinitions.Add(new());
        RowDefinitions.Add(new());
        RowDefinitions.Add(new() { Height = GridLength.Auto });

        nint windowHandle = new WindowInteropHelper(window).EnsureHandle();

        Modifications startup = new(Settings.Startup) { Header = "🛠️", Margin = new(4, 4, 4, 2) };

        SetRow(startup, 0);
        SetColumn(startup, 0);
        Children.Add(startup);

        Modifications runtime = new(Settings.Runtime) { Header = "🎮", Margin = new(4, 2, 4, 2) };

        SetRow(runtime, 1);
        SetColumn(runtime, 0);
        Children.Add(runtime);

        Button play = new()
        {
            Content = "▶",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new(4, 2, 4, 4),
            Padding = new(8)
        };

        play.Click += async (_, _) =>
        {
            if (!Game.Release.Installed) return;

            startup.IsEnabled = false;
            runtime.IsEnabled = false;
            play.IsEnabled = false;

            await Task.Run(() =>
            {
                Settings.Save();
                Game.Release.Launch(Settings.Startup, Settings.Runtime);
            });

            startup.IsEnabled = true;
            runtime.IsEnabled = true;
            play.IsEnabled = true;
        };

        SetRow(play, 2);
        SetColumn(play, 0);
        Children.Add(play);
    }
}