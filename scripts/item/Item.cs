using System.Collections.Generic;

namespace LandsofAzerith.scripts.item;

public abstract class Item
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract List<Zones> Zones { get; }
}