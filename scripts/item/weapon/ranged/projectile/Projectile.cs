namespace LandsOfAzerith.scripts.item.weapon.ranged.projectile;

public abstract class Projectile : Item, IStackable
{
    public abstract ulong MaxStack { get; }
    public abstract ulong Amount { get; set; }
    public void Add(ulong amount)
    {
        if (amount + Amount > MaxStack)
        {
            Amount = MaxStack;
        }
        else
        {
            Amount += amount;
        }
    }

    public void Remove(ulong amount)
    {
        if (Amount < amount)
        {
            Amount = 0;
        }
        else
        {
            Amount -= amount;
        }
    }

    public void Set(ulong amount)
    {
        if (amount > MaxStack)
        {
            Amount = MaxStack;
        }
        else
        {
            Amount = amount;
        }
    }
}