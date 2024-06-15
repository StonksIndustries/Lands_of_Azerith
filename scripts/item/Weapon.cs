using System.Text.Json.Serialization;
using Godot;

namespace LandsOfAzerith.scripts.item;

public class Weapon : InventoryItem
{
    public uint Damage { get; set; }
    public uint Range { get; set; }
    public uint CoolDown { get; set; }
}