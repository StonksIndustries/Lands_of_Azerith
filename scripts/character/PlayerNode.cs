using System.Collections.Generic;
using Godot;
using LandsOfAzerith.scripts.character.mob;
using LandsOfAzerith.scripts.inventory;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon;

namespace LandsOfAzerith.scripts.character;

public partial class PlayerNode : PlayerBackend
{
	[Signal]
	public delegate void HitEventHandler();
	
	[Export]
	public uint WalkingSpeed { get; set; } = 100; // How fast the player will move (pixels/sec).
	private double _inventoryCooldown = 0;
	private Directions Direction { get; set; }
	public static Vector2 ScreenSize; // Size of the game window.
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Direction = Directions.None;
		ScreenSize = GetViewportRect().Size;
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(Toolbox.ToInt(Name));
		ChangeHealth(MaxHealthPoints);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			ProcessInventory(delta);
			Move(delta);
			if (Input.IsActionJustPressed("player_attack"))
				InRangeMobs.ForEach(Attack);
        }
	}

	private void ProcessInventory(double delta)
	{
		if (_inventoryCooldown <= 0)
		{
			if (Input.IsActionPressed("open_inv"))
			{
				GetNodeOrNull<Inventory>("Inventory").Visible ^= true;
				_inventoryCooldown = 0.2;
			}
		}
		else
		{
			_inventoryCooldown -= delta;
		}
	}

	private void _on_body_entered(Node2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	/// <summary>
	/// Moves the player and animates it.
	/// </summary>
	/// <param name="delta">Time since last tick</param>
	private void Move(double delta)
	{
		var animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var velocity = MovementDirection(animation);
		if (velocity == Vector2.Zero)
		{
			AnimationIdle(animation);
		}
		else
		{
			velocity = velocity.Normalized() * WalkingSpeed;
			if (Input.IsActionPressed("sprint"))
			{
				animation.SpeedScale = 2;
				velocity *= 2;
			}
			else
			{
				animation.SpeedScale = 1;
			}

			Position += velocity * (float)delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
				y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
			);
		}
	}

	/// <summary>
	/// Returns the direction the player is moving after giving the appropriate animation.
	/// </summary>
	/// <param name="animation">Animation of the player</param>
	/// <returns></returns>
	private Vector2 MovementDirection(AnimatedSprite2D animation)
	{
		Vector2 velocity = Vector2.Zero;
		if (Input.IsActionPressed("move_left") && !Input.IsActionPressed("move_right"))
		{
			if (velocity == Vector2.Zero)
			{
				animation.FlipH = true;
				animation.Play("walk_side");
			}
			velocity.X -= 1;
			Direction = Directions.Left;
		}
		if (Input.IsActionPressed("move_right") && !Input.IsActionPressed("move_left"))
		{
			if (velocity == Vector2.Zero)
			{
				animation.FlipH = false;
				animation.Play("walk_side");
			}
			velocity.X += 1;
			Direction = Directions.Right;
		}
		if (Input.IsActionPressed("move_up")  && !Input.IsActionPressed("move_down"))
		{
			if (velocity == Vector2.Zero)
			{
				animation.FlipH = false;
				animation.Play("walk_up");
			}
			velocity.Y -= 1;
			Direction = Directions.Up;
		}
		if (Input.IsActionPressed("move_down")  && !Input.IsActionPressed("move_up"))
		{
			if (velocity == Vector2.Zero)
			{
				animation.FlipH = false;
				animation.Play("walk_down");
			}
			velocity.Y += 1;
			Direction = Directions.Down;
		}

		return velocity;
	}

	/// <summary>
	/// Animates the player when it is idle depending on the direction it is facing.
	/// </summary>
	/// <param name="animation">Animation of the player</param>
	private void AnimationIdle(AnimatedSprite2D animation)
	{
		animation.SpeedScale = 1;
		switch (Direction)
		{
			case Directions.Up:
				animation.Play("idle_up");
				break;
			case Directions.Down or Directions.None:
				animation.Play("idle_down");
				break;
			case Directions.Left or Directions.Right:
				animation.Play("idle_side");
				break;
		}
	}
	
	
	public override void Die()
	{
		Position = new Vector2(0, 0);
		HealthPoints = MaxHealthPoints;
	}
	
	public void _on_range_entered(Area2D body)
	{
		if (body is Mob mob)
		{
			InRangeMobs.Add(mob);
		}
	}
	
	public void _on_range_exited(Area2D body)
	{
		if (body is Mob mob)
		{
			InRangeMobs.Remove(mob);
		}
	}
}


