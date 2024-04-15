using Godot;
using LandsOfAzerith.scripts.item;

namespace LandsOfAzerith.scripts.character;

public abstract partial class Character : Area2D
{
    public abstract uint HealthPoints { get; protected set; }
    public abstract uint MaxHealthPoints { get; }
    public abstract uint Speed { get; set; }
    public abstract uint Strength { get; set; }
    public abstract Weapon Weapon { get; set; }
    public uint Damage => Strength + Weapon.Damage;
    protected abstract Character? Aggro { get; set; }
    public ProgressBar? HealthBar => GetNodeOrNull<ProgressBar>("HealthBar");
    public float AttackCooldown { get; set; }

    public abstract void Die();
    
    public void TakeDamage(uint damage)
    {
        if (HealthPoints <= damage)
        {
            HealthPoints = 0;
            Die();
        }
        else
        {
            HealthPoints -= damage;
        }
        
        if (HealthBar == null)
        {
            return;
        }
        HealthBar.Value = (double)(HealthPoints * 100) / MaxHealthPoints;
    }
    
    public void Attack(Character target, uint damage)
    {
        target.TakeDamage(damage);
    }
    
    public void Attack(Character target)
    {
        target.TakeDamage(Damage);
    }
    
    public void Heal(uint heal)
    {
        if (HealthPoints + heal > MaxHealthPoints)
        {
            HealthPoints = MaxHealthPoints;
        }
        else
        {
            HealthPoints += heal;
        }
    }
    
    public void ChangeHealth(uint amount)
    {
        if (amount > MaxHealthPoints)
        {
            HealthPoints = MaxHealthPoints;
        }
        else
        {
            HealthPoints = amount;
        }
        
        if (HealthBar == null)
        {
            return;
        }
        HealthBar.Value = (double)(HealthPoints * 100) / MaxHealthPoints;
    }
}