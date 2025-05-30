using System.Collections.Generic;

namespace Igneous.Launcher.Configuration;

sealed class Settings
{
   // static Settings _settings;

    static HashSet<string> _startup;

    static HashSet<string> _runtime;

    internal static HashSet<string> Startup => _startup ??= Serializer.Load("Startup.json");

    internal static HashSet<string> Runtime => _runtime ??= Serializer.Load("Runtime.json");

    internal static void Save()
    {
        Serializer.Save("Startup.json", _startup);
        Serializer.Save("Runtime.json", _runtime);
    }
}