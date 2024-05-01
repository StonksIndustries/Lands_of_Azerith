using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item;

public class StackingItem : InventoryItem
{
    public override string Name { get; }
    public override string Description { get; }
    public override string Icon { get; set; }
    public override List<Zones> Zones { get; }
    public override Rarity Rarity { get; }
    public ulong MaxStack => 100;
    public ulong Amount { get; protected set; }
}