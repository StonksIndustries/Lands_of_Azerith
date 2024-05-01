namespace LandsOfAzerith.scripts.item;

public abstract class Weapon : InventoryItem
{
    public abstract uint Damage { get; set; }
    public abstract uint Range { get; set; }
    
    public abstract uint CoolDown { get; set; }
}