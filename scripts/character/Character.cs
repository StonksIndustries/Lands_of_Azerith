using Godot;
using LandsOfAzerith.scripts.item;

namespace LandsOfAzerith.scripts.character;

public abstract partial class Character : CharacterBody2D
{
    public abstract uint HealthPoints { get; set; }
    public abstract uint MaxHealthPoints { get; }
    public abstract uint Speed { get; set; }
    public abstract uint Strength { get; set; }
    public abstract Weapon Weapon { get; set; }
    public uint Damage => Strength + Weapon.Damage;
    public ProgressBar? HealthBar;
    public float AttackCooldown { get; set; }
    public uint WalkingSpeed { get; set; } = 4000; // How fast the player will move (pixels/sec).

    public abstract void Die();

    public override void _Ready()
    {
        HealthBar = GetNodeOrNull<ProgressBar>("HealthBar");
        ChangeHealth(MaxHealthPoints);
        SetPhysicsProcess(true);
    }

    public abstract bool TakeDamage(Character attacker, uint damage);
    
    public void Attack(Character target, uint damage)
    {
        target.TakeDamage(this, damage);
    }
    
    public void Attack(Character target)
    {
        target.TakeDamage(this, Damage);
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
        
        UpdateHealthBar();
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
        
        UpdateHealthBar();
    }

    protected void UpdateHealthBar()
    {
        if (HealthBar != null)
            HealthBar.Value = (double)(HealthPoints * 100) / MaxHealthPoints;
    }
}