using Godot;

namespace LandsOfAzerith.scripts.item;

public abstract class Weapon : InventoryItem
{
    public uint Damage { get; set; }
    public uint Range { get; set; }
    public uint CoolDown { get; set; }
}