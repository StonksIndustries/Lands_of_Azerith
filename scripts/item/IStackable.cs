namespace LandsofAzerith.scripts.item;

public interface IStackable
{
    public ulong MaxStack { get; }
    public ulong Amount { get; protected set; }
    public void Add(ulong amount);
    public void Remove(ulong amount);
    public void Set(ulong amount);
}