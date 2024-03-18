using Godot;

namespace LandsOfAzerith.scripts.character;

public abstract partial class Character : Area2D
{
    public abstract ulong HealthPoints { get; protected set; }
    public abstract ulong MaxHealthPoints { get; }
    public abstract ulong Speed { get; set; }
    protected abstract Player? Aggro { get; set; }
    
    public void TakeDamage(ulong damage)
    {
        if (HealthPoints < damage)
        {
            HealthPoints = 0;
        }
        else
        {
            HealthPoints -= damage;
        }
    }
    
    public void Heal(ulong heal)
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
}