using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item.weapon;

public class RangedWeapon : Weapon
{
    public RangedWeapon()
    {
        Name = "Hands";
        Description = "Your hands.";
        Rarity = Rarity.Common;
        Damage = 1;
        Range = 1;
        CoolDown = 1;
    }
}