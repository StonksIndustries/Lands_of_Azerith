namespace LandsOfAzerith.scripts.item;

public abstract class Weapon : Item
{
    public abstract ulong Damage { get; }
    public abstract ulong Range { get; }
}