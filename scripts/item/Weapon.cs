namespace LandsOfAzerith.scripts.item;

public abstract class Weapon : Item
{
    public abstract uint Damage { get; }
    public abstract uint Range { get; }
    
    public abstract uint CoolDown { get; }
}