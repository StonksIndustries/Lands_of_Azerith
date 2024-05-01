using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item.weapon;

public class RangedWeapon : Weapon
{
    public override string Name { get; }
    public override string Description { get; }
    public override string Icon { get; set; }
    public override List<Zones> Zones { get; }
    public override Rarity Rarity { get; }
    public override uint Damage { get; set; }
    public override uint Range { get; set; }
    public override uint CoolDown { get; set; }
}