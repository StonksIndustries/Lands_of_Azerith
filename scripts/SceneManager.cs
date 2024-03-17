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
		InstantiatePlayers();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void InstantiatePlayers()
	{
		foreach (PlayerInfo item in GameManager.Players)
		{
			Rpc(nameof(InstantiateIndividualPlayer), item.Id);
		}
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void InstantiateIndividualPlayer(int id)
	{
		if (GetNodeOrNull(id.ToString()) != null)
		{
			return;
		}
		Player currentPlayer = _playerScene.Instantiate<Player>();
		currentPlayer.Name = id.ToString();
		AddChild(currentPlayer);
		Node2D spawnPoint = GetNode<Node2D>("SpawnPoint");
		currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
		if (id == Multiplayer.GetUniqueId())
			currentPlayer.AddChild(new Camera2D(){ Zoom = new Vector2(2, 2) });
	}
}