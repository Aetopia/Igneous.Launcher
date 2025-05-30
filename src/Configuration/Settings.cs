using System.Collections.Generic;

namespace Igneous.Launcher.Configuration;

sealed class Settings
{
   // static Settings _settings;

    static HashSet<string> _startup;

    static HashSet<string> _runtime;

    internal static HashSet<string> Startup => _startup ??= Serializer.Load("Igneous.Launcher.Startup.json");

    internal static HashSet<string> Runtime => _runtime ??= Serializer.Load("Igneous.Launcher.Runtime.json");

    internal static void Save()
    {
        Serializer.Save("Igneous.Launcher.Startup.json", _startup);
        Serializer.Save("Igneous.Launcher.Runtime.json", _runtime);
    }
}