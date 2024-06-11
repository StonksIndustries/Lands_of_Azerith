using System;
using Godot;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon;
using FileAccess = Godot.FileAccess;

namespace LandsOfAzerith.scripts.character.mob;

public abstract partial class Mob : Character
{
    private uint _cooldown = 0;
    private Random _random = new Random();
    private Vector2 _poi = Vector2.Zero;
    protected abstract string LootTable { get; }
    private void Wander()
    {
        if (_cooldown > 0)
        {
            _cooldown--;
        }
        else if (_poi == Position)
        {
            _cooldown = 100;
            _poi = Vector2.Zero;
        }
        else if (_poi != Vector2.Zero)
        {
            // Pathfinding to the point of interest.
            WalkTo(_poi);
        }
        else if (_random.Next(0, 5000) < 1)
        {
            _poi = new Vector2(Position.X + _random.Next(-100, 100), Position.Y + _random.Next(-100, 100));
            _poi = new Vector2(
                x: Mathf.Clamp(_poi.X, 1, PlayerNode.ScreenSize.X),
                y: Mathf.Clamp(_poi.Y, 1, PlayerNode.ScreenSize.Y)
            );
        }
    }

    private void WalkTo(Vector2 poi)
    {
        throw new NotImplementedException();
    }
    
    private void DropLoot()
    {
        if (!FileAccess.FileExists(LootTable))
        {
            GD.PrintErr($"Error parsing loot table: {LootTable} does not exist.");
            return;
        }
        
        var file = FileAccess.Open(LootTable, FileAccess.ModeFlags.Read);
        Json json = new Json();
        var error = json.Parse(file.GetAsText());
        file.Close();
        
        if (error != Error.Ok)
        {
            GD.PrintErr($"Error parsing loot table: {json.GetErrorMessage()} at line {json.GetErrorLine()}.");
            return;
        }
        
        var lootTable = (Godot.Collections.Dictionary) json.Data;
        InventoryItem? item = FilterType((string) lootTable["type"]);
        
        if (item == null)
        {
            GD.PrintErr($"Error parsing loot table: {lootTable["type"]} is not a valid item type.");
            return;
        }
        
        //item.Icon = (string) lootTable["icon"];
        
        Node2D loot = GD.Load<PackedScene>("res://scenes/inventory/floor_item.tscn").Instantiate<Area2D>();
        loot.Position = Position;
        GetParent().AddChild(loot);
    }
    
    public override void Die()
    {
        DropLoot();
        QueueFree();
    }
    
    private InventoryItem? FilterType(string type)
    {
        switch (type)
        {
            case "meleeWeapon":
                return new MeleeWeapon();
            case "rangedWeapon":
                return new RangedWeapon();
            case "stackingItem":
                return new StackingItem();
            default:
                return null;
        }
    }
}