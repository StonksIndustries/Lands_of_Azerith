namespace LandsOfAzerith.scripts.item.weapon;

public abstract class DurableMeleeWeapon : Weapon, IDurable
{
    public abstract ulong Durability { get; set; }
    public abstract ulong MaxDurability { get; set; }

    public void Repair(ulong amount)
    {
        if (amount + Durability > MaxDurability)
        {
            Durability = MaxDurability;
        }
        else
        {
            Durability += amount;
        }
    }

    void IDurable.Damage(ulong amount)
    {
        if (amount > Durability)
        {
            Durability = 0;
        }
        else
        {
            Durability -= amount;
        }
    }

    public void SetDurability(ulong amount)
    {
        if (amount> MaxDurability)
        {
            Durability = MaxDurability;
        }
        else
        {
            Durability = amount;
        }
    }
}