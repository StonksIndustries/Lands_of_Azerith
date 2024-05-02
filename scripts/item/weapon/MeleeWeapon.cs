using System.Collections.Generic;

namespace LandsOfAzerith.scripts.item.weapon;

public class MeleeWeapon : Weapon
{
    public MeleeWeapon()
    {
        Name = "Hands";
        Description = "Your hands.";
        Rarity = Rarity.Common;
        Damage = 1;
        Range = 1;
        CoolDown = 1;
    }
}