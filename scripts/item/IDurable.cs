namespace LandsOfAzerith.scripts.item;

public interface IDurable
{
    public ulong Durability { get; protected set; }
    public ulong MaxDurability { get; set; }
    public void Repair(ulong amount);
    public void Damage(ulong amount);
    public void SetDurability(ulong amount);
}