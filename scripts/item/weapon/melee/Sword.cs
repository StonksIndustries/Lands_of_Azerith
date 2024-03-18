using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item.weapon.melee;

public class Sword : DurableMeleeWeapon
{
    public override string Name => "Sword";
    public override string Description => "A simple sword.";

    public override List<Zones> Zones => new List<Zones>()
    {
        scripts.Zones.Everywhere
    };

    public override Rarity Rarity => Rarity.Common;
    public override ulong Damage => 10;
    public override ulong Range { get; }
    public override ulong Durability { get; set; }
    public override ulong MaxDurability { get; }
}