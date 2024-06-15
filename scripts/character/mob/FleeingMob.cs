using Godot;

namespace LandsOfAzerith.scripts.character.mob;

public partial class FleeingMob : Mob
{
    public override uint MaxHealthPoints => 100;
    public override uint Strength { get; set; }
    protected override string LootTable => "res://loot_tables/slime.json";

    public override void _PhysicsProcess(double delta)
    {
        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
            return;
        var velocity = Vector2.Zero;
        if (Aggro != null)
        {
            var animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            _navAgent.TargetPosition = Aggro.Position;
            // C'est juste du foutage de gueule, mais ça marche xD.
            velocity = -Position.DirectionTo(_navAgent.GetNextPathPosition());
            animation.Play("slime");
        }
        else if (Position.DistanceTo(_navAgent.TargetPosition) > 5)
        {
            velocity = Position.DirectionTo(_navAgent.GetNextPathPosition());
        }

        Velocity = WalkingSpeed * velocity * (float)delta;
        MoveAndSlide();
    }
}