using System.Collections.Generic;
using Godot;
using LandsOfAzerith.scripts.character.mob;
using LandsOfAzerith.scripts.inventory;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon;
using LandsOfAzerith.scripts.poi;

namespace LandsOfAzerith.scripts.character;

public partial class PlayerNode : PlayerBackend
{
	
	[Export]
	public uint WalkingSpeed { get; set; } = 10000; // How fast the player will move (pixels/sec).
	private double _inventoryCooldown = 0;
	private Directions Direction { get; set; }
	public static Vector2 ScreenSize; // Size of the game window.

	public MultiplayerSynchronizer MultiplayerSynchronizer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Direction = Directions.None;
		ScreenSize = GetViewportRect().Size; 
		MultiplayerSynchronizer = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
		MultiplayerSynchronizer.SetMultiplayerAuthority(Toolbox.ToInt(Name));
		
		ChangeHealth(MaxHealthPoints);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (MultiplayerSynchronizer.GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			ProcessInventory(delta);
			Move(delta);
			if (Input.IsActionJustPressed("player_attack"))
				InRangeMobs.ForEach(Attack);
        }
	}

	public override void _PhysicsProcess(double delta)
	{
		if (MultiplayerSynchronizer.GetMultiplayerAuthority() ==
		    Multiplayer.GetUniqueId())
		{
			Move(delta);
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

			Velocity = velocity * (float)delta;
			MoveAndSlide();
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
			velocity += Vector2.Left;
			Direction = Directions.Left;
		}
		if (Input.IsActionPressed("move_right") && !Input.IsActionPressed("move_left"))
		{
			if (velocity == Vector2.Zero)
			{
				animation.FlipH = false;
				animation.Play("walk_side");
			}
			velocity += Vector2.Right;
			Direction = Directions.Right;
		}
		if (Input.IsActionPressed("move_up")  && !Input.IsActionPressed("move_down"))
		{
			if (velocity == Vector2.Zero)
			{
				animation.FlipH = false;
				animation.Play("walk_up");
			}
			velocity += Vector2.Up;
			Direction = Directions.Up;
		}
		if (Input.IsActionPressed("move_down")  && !Input.IsActionPressed("move_up"))
		{
			if (velocity == Vector2.Zero)
			{
				animation.FlipH = false;
				animation.Play("walk_down");
			}
			velocity += Vector2.Down;
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
		Position = Statistics.Checkpoint;
		HealthPoints = MaxHealthPoints;
	}
	
	private void _on_range_entered(Node2D body)
	{
		if (body is Mob mob)
		{
			InRangeMobs.Add(mob);
		}
		else if (body is Checkpoint)
		{
			Statistics.Checkpoint = body.Position;
		}
	}
	
	private void _on_range_exited(Node2D body)
	{
		if (body is Mob mob)
		{
			InRangeMobs.Remove(mob);
		}
	}
	
	private void _on_area_entered(Area2D area)
	{
		if (area is Checkpoint)
		{
			Statistics.Checkpoint = area.Position;
		}
	}
}


