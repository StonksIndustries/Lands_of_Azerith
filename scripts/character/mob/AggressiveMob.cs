using System;
using Godot;
using LandsOfAzerith.scripts.item;

namespace LandsOfAzerith.scripts.character.mob;

public partial class AggressiveMob : Mob
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

			if (Position.DistanceTo(Aggro.Position) > 15)
			{
				velocity = Position.DirectionTo(_navAgent.GetNextPathPosition());
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
		else if (Position.DistanceTo(_navAgent.TargetPosition) > 5)
		{
			velocity = Position.DirectionTo(_navAgent.GetNextPathPosition());
		}

		Velocity = WalkingSpeed * velocity * (float)delta;
		MoveAndSlide();
	}


}
