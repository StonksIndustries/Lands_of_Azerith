using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item.weapon.melee;

public class Hands : MeleeWeapon
{
    public override string Icon => "res://assets/items/weapons/hands.png";
    public override string Name => "Hands";
    public override string Description => "Your hands.";
    public override List<Zones> Zones => new List<Zones>();
    public override Rarity Rarity => Rarity.Common;
    public override uint Damage => 10;
    public override uint Range => 10;
    public override uint CoolDown => 1;
}