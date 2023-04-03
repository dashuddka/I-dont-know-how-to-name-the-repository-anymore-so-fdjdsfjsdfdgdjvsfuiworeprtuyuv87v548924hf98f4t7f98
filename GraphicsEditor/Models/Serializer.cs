using System.IO;
using System.Xml.Serialization;
using System;
using GraphicsEditor.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class Serializer<T>
{
    public static void Save(string path, T data)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(T));

        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static T Load(string path)
    {
        Type type = typeof(T);
        T retVal;

        XmlSerializer formatter = new XmlSerializer(type);

        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            retVal = (T)formatter.Deserialize(stream);
        }

        return retVal;
    }
}

public static class JsonSerializer<T>
{
    public static void Save(string path, T data)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };
        string Serialized = JsonConvert.SerializeObject(data, settings);
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.WriteLine(Serialized);
        }
    }

    public static T Load(string path)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };
        string Serialized;
        using (StreamReader reader = new StreamReader(path))
        {
            Serialized = reader.ReadToEnd();
        }
        return JsonConvert.DeserializeObject<T>(Serialized, settings);
    }
}
