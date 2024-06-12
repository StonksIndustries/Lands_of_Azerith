using Godot;
using LandsOfAzerith.scripts.character;
using LandsOfAzerith.scripts.inventory;

namespace LandsOfAzerith.scripts;

public partial class SceneManager : Node2D
{
	[Export]
	private PackedScene _playerScene = GD.Load<PackedScene>("res://scenes/player.tscn");
	
	[Export]
	private PackedScene _inventoryScene = GD.Load<PackedScene>("res://scenes/inventory/inventory.tscn");
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
		PlayerInfo playerInfo = GameManager.GetPlayer(id)!;
		if (GetNodeOrNull(playerInfo.Id.ToString()) != null)
		{
			return;
		}
		PlayerNode currentPlayerNode = _playerScene.Instantiate<PlayerNode>();
		currentPlayerNode.Name = playerInfo.Id.ToString();
		AddChild(currentPlayerNode);
		Node2D spawnPoint = GetNode<Node2D>("SpawnPoint");
		currentPlayerNode.GlobalPosition = spawnPoint.GlobalPosition;
		var nameLabel = currentPlayerNode.GetNodeOrNull<Label>("Name");
		if (nameLabel != null)
		{
			if (playerInfo.Name == "")
				nameLabel.Text += playerInfo.Number.ToString();
			else
				nameLabel.Text = playerInfo.Name;
		}

		// Only adds to the controlled player
		if (playerInfo.Id == Multiplayer.GetUniqueId())
		{
			// Adds the camera to the player
			int zoom = 2;
			var camera = new Camera2D()
			{
				Name = "Camera",
				Zoom = new Vector2(zoom, zoom)
			};
			currentPlayerNode.AddChild(camera);
			
			// Adds the inventory to the player
			var inventory = _inventoryScene.Instantiate<Inventory>();
			inventory.Position = - GetViewportRect().Size / (2 * zoom);
			inventory.Visible = false;
			currentPlayerNode.AddChild(inventory);
			currentPlayerNode.Inventory = inventory;
		}
	}
}
