using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts.item;

public abstract class Item
{
    public abstract string Icon { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract List<Zones> Zones { get; }
    public abstract Rarity Rarity { get; }

    public Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { nameof(Icon), Icon },
            { nameof(Name), Name },
            { nameof(Description), Description },
            { nameof(Rarity), Rarity.ToString() }
        };
    }
}