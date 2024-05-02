using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts.item;

public abstract class InventoryItem
{
    private string _id;
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public string Icon => "res://assets/items/" + _id + ".png";
    public Rarity Rarity { get; protected set; }

    public Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { "id", _id },
            { nameof(Name), Name },
            { nameof(Description), Description },
            { nameof(Rarity), Rarity.ToString() }
        };
    }
    
    public void Load(Godot.Collections.Dictionary<string, Variant> data)
    {
        Name = (string)data[nameof(Name)];
        Description = (string)data[nameof(Description)];
        Rarity = (Rarity)(int)data[nameof(Rarity)];
        _id = (string)data["id"];
    }
}