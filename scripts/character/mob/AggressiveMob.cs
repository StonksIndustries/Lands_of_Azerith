using System;
using Godot;

namespace LandsOfAzerith.scripts.character.mob;

public partial class AggressiveMob : Mob
{
    public override ulong HealthPoints { get; protected set; }
    public override ulong MaxHealthPoints { get; }
    protected override Player? Aggro { get; set; }
    public override ulong Speed { get; set; } = 80;
    
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
        if (body is Player player)
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