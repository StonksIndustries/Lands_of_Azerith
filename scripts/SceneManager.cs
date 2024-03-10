using Godot;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts;

public partial class SceneManager : Node2D
{
	[Export]
	private PackedScene _playerScene = GD.Load<PackedScene>("res://scenes/player.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var item in GameManager.Players)
		{
			Player currentPlayer = _playerScene.Instantiate<Player>();
			currentPlayer.Name = item.Id.ToString();
			AddChild(currentPlayer);
			Node2D spawnPoint = GetNode<Node2D>("SpawnPoint");
			currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}