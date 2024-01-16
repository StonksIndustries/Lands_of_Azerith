using Godot;

public partial class Player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public void _PhysicsProcess(float delta)
	{
		Vector2 velocity = Velocity; 
		
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X = Speed;
			velocity.Y = 0;
		}
		else if (Input.IsActionPressed("move_left"))
		{
			velocity.X = -Speed;
			velocity.Y = 0;
		}
		else
		{
			velocity.X = 0;
		}

		MoveAndSlide();
	}

	
}
