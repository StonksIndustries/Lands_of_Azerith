using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.item;

[JsonDerivedType(typeof(Weapon),nameof(Weapon))]
[JsonDerivedType(typeof(StackingItem),nameof(StackingItem))]
public abstract class InventoryItem
{
    private static string Path => "res://items/";
    [JsonIgnore] public string Icon => "res://assets/items/" + WeaponId + ".png";
    
    [JsonIgnore] public string WeaponId;
    public string Name { get; set; }
    public string Description { get; set; }
    public Rarity Rarity { get; set; }
    public static InventoryItem? Load(string itemId)
    {
        var item = Toolbox.LoadFileInJson<InventoryItem>(Path + itemId + ".json");
        if (item != null)
            item.WeaponId = itemId;
        
        return item;
    }
}