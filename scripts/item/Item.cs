using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item;

public abstract class Item
{
    public abstract string TexturePath { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract List<Zones> Zones { get; }
    public abstract Rarity Rarity { get; }
}