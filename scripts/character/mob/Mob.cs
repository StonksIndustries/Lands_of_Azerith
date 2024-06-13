using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon;
using FileAccess = Godot.FileAccess;

namespace LandsOfAzerith.scripts.character.mob;

public abstract partial class Mob : Character
{
    private uint _cooldown = 0;
    private Random _random = new Random();
    protected abstract Character? Aggro { get; set; }
    protected abstract string LootTable { get; }
    
    protected NavigationAgent2D _navAgent;
    private PackedScene _floorItemScene;
    
    public override void _Ready()
    {
        base._Ready();
        _navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _floorItemScene = GD.Load<PackedScene>("res://scenes/inventory/floor_item.tscn");
    }
    
    private void DropLoot()
    {
        var lootTable = JsonSerializer.Deserialize<List<string>>(File.ReadAllBytes("res://loot_tables/slime.json"));
        if (lootTable != null)
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
    
    

    private void _on_aggro_zone_entered(Node2D body)
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
}