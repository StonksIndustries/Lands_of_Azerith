using Godot;

namespace LandsOfAzerith.scripts.poi;

public partial class Map : Node2D
{
	[Export] public int LimitLeft;
	[Export] public int LimitTop;
	[Export] public int LimitRight;
	[Export] public int LimitBottom;
	
	public void SetCameraLimits(Camera2D camera)
	{
		camera.LimitBottom = LimitBottom;
		camera.LimitLeft = LimitLeft;
		camera.LimitRight = LimitRight;
		camera.LimitTop = LimitTop;
	}
}
