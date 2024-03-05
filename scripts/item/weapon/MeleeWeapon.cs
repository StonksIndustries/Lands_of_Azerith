namespace LandsOfAzerith.scripts.item.weapon;

public abstract class MeleeWeapon : Item, IWeapon
{
    public abstract ulong Damage { get; }
}