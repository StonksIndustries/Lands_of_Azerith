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
			Rpc(nameof(InstantiateIndividualPlayer), item.Id, item.Name);
		}
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void InstantiateIndividualPlayer(int id, string name)
	{
		(int Id, string Name) playerInfo = (id, name);
		if (GetNodeOrNull(playerInfo.Id.ToString()) != null)
		{
			return;
		}
		Player currentPlayer = _playerScene.Instantiate<Player>();
		currentPlayer.Name = playerInfo.Id.ToString();
		AddChild(currentPlayer);
		Node2D spawnPoint = GetNode<Node2D>("SpawnPoint");
		currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
		var nameLabel = currentPlayer.GetNodeOrNull<Label>("Name");
		if (nameLabel != null)
		{
			if (playerInfo.Name == "")
				nameLabel.Text += playerInfo.Id.ToString();
			else
				nameLabel.Text = playerInfo.Name;
		}
		if (playerInfo.Id == Multiplayer.GetUniqueId())
			currentPlayer.AddChild(new Camera2D(){ Zoom = new Vector2(2, 2) });
	}
}