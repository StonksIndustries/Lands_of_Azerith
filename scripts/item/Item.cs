using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item;

public abstract class Item
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract List<Zones> Zones { get; }
}