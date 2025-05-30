
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

namespace Igneous.Launcher.Configuration;

static class Serializer
{
    static readonly DataContractJsonSerializer _load = new(typeof(List<string>), new DataContractJsonSerializerSettings()
    {
        UseSimpleDictionaryFormat = true,
        MaxItemsInObjectGraph = int.MaxValue
    });

    static readonly DataContractJsonSerializer _save = new(typeof(HashSet<string>), new DataContractJsonSerializerSettings()
    {
        UseSimpleDictionaryFormat = true,
        MaxItemsInObjectGraph = int.MaxValue
    });

    internal static HashSet<string> Load(string path)
    {
        HashSet<string> collection = [];
        try
        {
            using GZipStream stream = new(File.OpenRead(path),CompressionMode.Decompress);
            foreach (var item in (List<string>)_load.ReadObject(stream)) collection.Add(item);
        }
        catch { }
        return collection;
    }

    internal static void Save(string path, HashSet<string> collection)
    {
        try
        {
            using GZipStream stream = new(File.Create(path), CompressionLevel.Optimal);
            _save.WriteObject(stream, collection);
        }
        catch { }
    }
}