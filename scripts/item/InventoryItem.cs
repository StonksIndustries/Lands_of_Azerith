using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.item;

public abstract class InventoryItem
{
    private static string Path => "res://items/";
    
    private string _id;
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public string Icon => "res://assets/items/" + _id + ".png";
    public Rarity Rarity { get; protected set; }
    public static InventoryItem? Load(string itemId)
    {
        return JsonSerializer.Deserialize<InventoryItem>(File.ReadAllText(Path + itemId + ".json"));
    }
}