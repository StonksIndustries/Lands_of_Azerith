using Godot;

namespace LandsOfAzerith.scripts.item;

public abstract class Weapon : InventoryItem
{
    public uint Damage { get; protected set; }
    public uint Range { get; protected set; }
    public uint CoolDown { get; protected set; }
    
    public new Godot.Collections.Dictionary<string, Variant> Save()
    {
        var data = base.Save();
        data.Add(nameof(Damage), Damage);
        data.Add(nameof(Range), Range);
        data.Add(nameof(CoolDown), CoolDown);
        return data;
    }
    
    public new void Load(Godot.Collections.Dictionary<string, Variant> data)
    {
        base.Load(data);
        Damage = (uint)data[nameof(Damage)];
        Range = (uint)data[nameof(Range)];
        CoolDown = (uint)data[nameof(CoolDown)];
    }
}