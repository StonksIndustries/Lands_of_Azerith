using System.Text.Json;
using Godot;

namespace LandsOfAzerith.scripts;

public static class Toolbox
{
    /// <summary>
    /// The options for the JsonSerializer used in the code.
    /// </summary>
    public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions
    {
        WriteIndented = true,
        IncludeFields = true
    };
    
    /// <summary>
    /// Takes a positive integer in string format and returns it as an integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>-1 if value is not a positive integer, else the string as an integer</returns>
    public static int ToInt(string value)
    {
        int result = 0;
        foreach (char c in value)
        {
            if (c is < '0' or > '9')
            {
                return -1;
            }
            result = result * 10 + (c - '0');
        }
        return result;
    }
    
    /// <summary>
    /// Return the content of a Json file as an object of type T.
    /// </summary>
    /// <param name="path">The path to the Json file</param>
    /// <typeparam name="T">Type of object</typeparam>
    /// <returns></returns>
    public static T? LoadFileInJson<T>(string path)
    {
        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        return JsonSerializer.Deserialize<T>(file.GetAsText());
    }

    /// <summary>
    /// Loads a property from a dictionary and returns it.
    /// </summary>
    /// <param name="dict">The dictionary</param>
    /// <param name="key">The key to the item requested</param>
    /// <returns>null if the key is not in the dictionary, else the Variant associated</returns>
    public static Variant? LoadProperty(Godot.Collections.Dictionary dict, string key) 
    {
        if (!dict.ContainsKey(key))
        {
            GD.PrintErr($"Error parsing Json: {key} does not exist.");
            return null;
        }
        return dict[key];
    }
}