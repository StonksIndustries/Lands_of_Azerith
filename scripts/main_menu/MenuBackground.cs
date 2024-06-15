using Godot;

namespace LandsOfAzerith.scripts.main_menu;

public partial class MenuBackground : ParallaxBackground
{

	[Export] private int ScrollSpeed = 40;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ScrollOffset -= new Vector2((float)(ScrollSpeed * delta), 0);
	}
}