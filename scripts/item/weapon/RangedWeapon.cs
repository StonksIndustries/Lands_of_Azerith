namespace LandsOfAzerith.scripts.item.weapon;

public abstract class RangedWeapon : Item, IWeapon
{
    public abstract ulong Damage { get; }
}