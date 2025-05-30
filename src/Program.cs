using System;
using System.Windows;
using Igneous.Launcher.Configuration;
using Igneous.Launcher.Minecraft;

using static Igneous.Launcher.PInvoke.Shell32;
using static Igneous.Launcher.PInvoke.Constants;

static class Program
{
    [STAThread]
    static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            ShellMessageBox(0, 0, $"{args.ExceptionObject}", "Error", MB_ICONERROR);
            Environment.Exit(default);
        };

        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Loader.Launch([.. Settings.Startup]);

        Application application = new();
        application.Exit += (_, _) => Settings.Save();
        application.Run(new Igneous.Launcher.UI.Window());
    }
}