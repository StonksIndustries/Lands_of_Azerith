using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using LandsOfAzerith.scripts.item;
using FileAccess = Godot.FileAccess;

namespace LandsOfAzerith.scripts.character.mob;

public abstract partial class Mob : Character
{
    public static string MobPath = "res://mobs/";
    public MobStats Stats { get; set; }
    public string MobId { get; set;}
    public override uint HealthPoints { get => Stats.HealthPoints; set => Stats.HealthPoints = value; }
    public override Weapon Weapon { get => Stats.Weapon; set => Stats.Weapon = value; }
    public override uint Speed { get => Stats.Speed; set => Stats.Speed = value; }
    private uint _cooldown = 0;
    protected readonly Random Random = new Random();
    protected Character? Aggro { get; set; }
    protected abstract string LootTable { get; }
    
    protected NavigationAgent2D _navAgent;
    private PackedScene _floorItemScene;
    
    public override void _Ready()
    {
        base._Ready();
        _navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _navAgent.TargetPosition = Position;
        _floorItemScene = GD.Load<PackedScene>("res://scenes/inventory/floor_item.tscn");
    }
    
    private void DropLoot()
    {
        var lootTable = Stats.LootTable;
        foreach (var item in lootTable)
        {
            var floorItem = _floorItemScene.Instantiate<FloorItem>();
            if (floorItem != null) 
            {
                floorItem.Item = InventoryItem.Load(item);
                floorItem.Position = Position;
                GetParent().AddChild(floorItem);
            }
        }
    }
    
    public override void Die()
    {
        DropLoot();
        QueueFree();
    }

    protected virtual void _on_aggro_zone_entered(Node2D body)
    {
        if (Aggro is null && body is PlayerNode player)
        {
            Aggro = player;
        }
    }
    
    private void _on_de_aggro_zone_exited(Node2D body)
    {
        if (body == Aggro)
        {
            Aggro = null;
            _navAgent.TargetPosition = Position;
        }
    }
    
    public override bool TakeDamage(Character attacker, uint damage)
    {
        Aggro = attacker;
        
        if (HealthPoints <= damage)
        {
            HealthPoints = 0;
            Die();
            UpdateHealthBar();

            return true;
        }
        else
        {
            HealthPoints -= damage;
            UpdateHealthBar();

            return false;
        }
    }
    
    private void _on_wandering_timer_timeout()
    {
        if (Aggro is null && Random.Next(0, 3) < 1)
        {
            _navAgent.TargetPosition = new Vector2(Position.X + Random.Next(-300, 300), Position.Y + Random.Next(-300, 300));
        }
    }

    public void LoadStats()
    {
        Stats = Toolbox.LoadFileInJson<MobStats>(MobPath + MobId + ".json") ?? new MobStats
        {
            HealthPoints = MaxHealthPoints,
            LootTable = new List<string>(),
            MobId = "",
            Speed = 0,
            Weapon = (Weapon) InventoryItem.Load("default_weapon")!
        };
    }
}