using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item.weapon.ranged;

public class Bow : DurableRangedWeapon
{
    public override string Icon => "res://assets/items/weapons/bow.png";
    public override string Name => "Bow";
    public override string Description => "A simple bow.";
    public override List<Zones> Zones => new List<Zones>()
    {
        scripts.Zones.Everywhere
    };
    public override Rarity Rarity => Rarity.Common;
    public override uint Damage => 20;
    public override uint Range => 10;
    public override ulong Durability { get; set; }
    public override ulong MaxDurability => 100;
}