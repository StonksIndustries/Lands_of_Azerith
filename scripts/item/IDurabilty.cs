namespace LandsofAzerith.scripts.item;

public interface IDurabilty
{
    public ulong Durability { get; protected set; }
    public ulong MaxDurability { get; }
    public void Repair(ulong amount);
    public void Damage(ulong amount);
    public void SetDurability(ulong amount);
}