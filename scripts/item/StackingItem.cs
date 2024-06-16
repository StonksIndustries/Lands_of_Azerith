using System.Text.Json.Serialization;
using Godot;

namespace LandsOfAzerith.scripts.item;

public class StackingItem : InventoryItem
{ 
    [JsonIgnore] public static ulong MaxStack => 100;
    public ulong Quantity { get; set; }
}