using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item.weapon.ranged.projectile;

public class Arrow : Projectile
{
    public override string TexturePath => "res://assets/items/weapons/projectiles/arrow.png";
    public override string Name => "Arrow";
    public override string Description => "A simple arrow.";

    public override List<Zones> Zones => new List<Zones>()
    {
        scripts.Zones.Everywhere
    };

    public override Rarity Rarity => Rarity.Common;
    public override ulong MaxStack => 99;
    public override ulong Amount { get; set; }
}