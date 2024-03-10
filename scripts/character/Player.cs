using Godot;

namespace LandsOfAzerith.scripts.character;

public partial class Player : Character
{
	[Signal]
	public delegate void HitEventHandler();
	
	[Export]
	public int Speed { get; set; } = 100; // How fast the player will move (pixels/sec).
	public override ulong HealthPoints { get; protected set; }
	public override ulong MaxHealthPoints => 100;
	protected override Character? Aggro { get; set; }
	private Directions Direction { get; set; }
	public Vector2 ScreenSize; // Size of the game window.
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Direction = Directions.None;
		ScreenSize = GetViewportRect().Size;
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(Toolbox.ToInt(Name));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
			Move(delta);
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

	public void Move(double delta)
	{
		var velocity = Vector2.Zero;
		var animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (Input.IsActionPressed("move_up"))
		{
			animation.FlipH = false;
			animation.Play("walk_up");
			velocity.Y -= Speed;
			Direction = Directions.Up;
		}
		if (Input.IsActionPressed("move_down"))
		{
			animation.FlipH = false;
			animation.Play("walk_down");
			velocity.Y += Speed;
			Direction = Directions.Down;
		}
		if (Input.IsActionPressed("move_left"))
		{
			animation.FlipH = true;
			animation.Play("walk_side");
			velocity.X -= Speed;
			Direction = Directions.Left;
		}
		if (Input.IsActionPressed("move_right"))
		{
			animation.FlipH = false;
			animation.Play("walk_side");
			velocity.X += Speed;
			Direction = Directions.Right;
		}
		if (velocity == Vector2.Zero)
		{
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


