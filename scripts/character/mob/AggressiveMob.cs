﻿using System;
using Godot;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon.melee;

namespace LandsOfAzerith.scripts.character.mob;

public partial class AggressiveMob : Mob
{
    public override uint HealthPoints { get; protected set; }
    public override uint MaxHealthPoints { get; }
    public override Weapon Weapon { get; set; } = new Hands();
    protected override Character? Aggro { get; set; }
    public override uint Speed { get; set; } = 80;
    public override uint Strength { get; set; }

    [Export]
    private Vector2 _poi = Vector2.Zero;
    
    private Random _random = new Random();

    public override void _Process(double delta)
    {
        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
            return;
        var velocity = Vector2.Zero;
        if (Aggro != null)
        {
            var animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

            if (Position.DistanceTo(Aggro.Position) > 15)
            {
                
                velocity = Position.DirectionTo(Aggro.Position);
                animation.Play("slime");
            }
            else
            {
                if (AttackCooldown <= 0)
                {
                    Attack(Aggro);
                    AttackCooldown = Weapon.CoolDown;
                }
                else
                {
                    AttackCooldown -= (float)delta;
                }
            }
        }
        else if (_poi != Vector2.Zero)
        {
            velocity = Position.DirectionTo(_poi);
            if (Position.DistanceTo(_poi) < 5)
            {
                _poi = Vector2.Zero;
            }
        }
        else if (_random.Next(0, 5000) < 1)
        {
            _poi = new Vector2( Position.X + _random.Next(-100, 100), Position.Y + _random.Next(-100, 100));
            _poi = new Vector2(
                x: Mathf.Clamp(_poi.X, 0, Player.ScreenSize.X),
                y: Mathf.Clamp(_poi.Y, 0, Player.ScreenSize.Y)
            );
        }
        Position += Speed * velocity * (float)delta;
    }


    private void _on_aggro_zone_entered(Area2D body)
    {
        if (Aggro is null && body is Player player)
        {
            Aggro = player;
        }
    }
    
    private void _on_de_aggro_zone_exited(Area2D body)
    {
        if (body == Aggro)
        {
            Aggro = null;
        }
    }
    
    
}