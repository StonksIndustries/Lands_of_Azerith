using Godot;

namespace LandsOfAzerith.scripts;

public static class Toolbox
{
    
    /// <summary>
    /// Takes a positive integer in string format and returns it as an integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>-1 if value is not a postive integer, else the string as an integer</returns>
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
    /// Takes a file path to a Json file and returns the Json object with the file parsed.
    /// </summary>
    /// <param name="path">The path to the Json file</param>
    /// <returns>null if path or file content is invalid, else a Json with the file parsed</returns>
    public static Json? LoadFileInJson(string path)
    {
        if (!FileAccess.FileExists(path))
        {
            GD.PrintErr($"Error parsing quest: {path} does not exist.");
            return null;
        }
        
        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        Json jsonParser = new Json();
        var error = jsonParser.Parse(file.GetAsText());
        file.Close();
        
        if (error != Error.Ok)
        {
            GD.PrintErr($"Error parsing loot table: {jsonParser.GetErrorMessage()} at line {jsonParser.GetErrorLine()}.");
            return null;
        }

        return jsonParser;
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