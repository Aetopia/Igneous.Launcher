using System;
using System.Windows;
using Igneous.Launcher.Configuration;
using Igneous.Launcher.Minecraft;
using static Igneous.Launcher.PInvoke.Shell32;
using static Igneous.Launcher.PInvoke.Constants;
using System.Threading;
using System.Runtime.InteropServices;

[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.System32)]

static class Program
{
    [STAThread]
    static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            ShellMessageBox(0, 0, $"{args.ExceptionObject}", "Error", MB_ICONERROR);
            Environment.Exit(0);
        };

        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Loader.Launch([.. Settings.Startup]);

        using Mutex mutex = new(false, "F3C6CA66-40CC-4E9E-AE10-F11064726AFC", out var createdNew);
        if (!createdNew) return;

        Application application = new();
        application.Exit += (_, _) => Settings.Save();
        application.Run(new Igneous.Launcher.UI.Window());
    }
}