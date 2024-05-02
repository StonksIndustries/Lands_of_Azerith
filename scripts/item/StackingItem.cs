using Godot;

namespace LandsOfAzerith.scripts.item;

public class StackingItem : InventoryItem
{
    public ulong MaxStack => 100;
    public ulong Quantity { get; protected set; }
    
    // Technically doesn't work, here to avoid warnings.
    public StackingItem(){}
    
    public StackingItem(string name, string description, Rarity rarity, ulong quantity)
    {
        Name = name;
        Description = description;
        Rarity = rarity;
        Quantity = quantity;
    }
    
    public new Godot.Collections.Dictionary<string, Variant> Save()
    {
        var data = base.Save();
        data.Add(nameof(Quantity), Quantity);
        return data;
    }
    
    public new void Load(Godot.Collections.Dictionary<string, Variant> data)
    {
        base.Load(data);
        Quantity = (ulong)data[nameof(Quantity)];
    }
}