using System;
using Godot;

namespace LandsOfAzerith.scripts.character.mob;

public abstract partial class Mob : Character
{
    private uint _cooldown = 0;
    private Random _random = new Random();
    private Vector2 _poi = Vector2.Zero;
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
                x: Mathf.Clamp(_poi.X, 1, Player.ScreenSize.X),
                y: Mathf.Clamp(_poi.Y, 1, Player.ScreenSize.Y)
            );
        }
    }

    private void WalkTo(Vector2 poi)
    {
        throw new NotImplementedException();
    }
    
    
    public override void Die()
    {
        GetTree().Quit();
    }
}