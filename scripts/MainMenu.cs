using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using LandsOfAzerith.scripts;
using System.Linq;
using System.Text.Json;
using LandsOfAzerith.scripts.character;
using LandsOfAzerith.scripts.inventory;
using LandsOfAzerith.scripts.main_menu;
using LandsOfAzerith.scripts.quests;
using LandsOfAzerith.scripts.quests.goals;
using LandsOfAzerith.scripts.quests.rewards;

namespace LandsOfAzerith.scripts;
public partial class MainMenu : Control
{
	[Export] private int _port = 8901;
	[Export] private string _address = "127.0.0.1";
	
	private PackedScene _playerScene = GD.Load<PackedScene>("res://scenes/player.tscn");
	private PackedScene _inventoryScene = GD.Load<PackedScene>("res://scenes/inventory/inventory.tscn");

	private ENetMultiplayerPeer _peer = new ENetMultiplayerPeer();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;

		GetNode<ServerBrowser>("ServerBrowser").JoinServer += JoinServer;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	/// <summary>
	/// Called when a player connects, on all peers.
	/// </summary>
	/// <param name="id">ID of the player that connected</param>
	public void PeerConnected(long id)
	{
		GD.Print("Player " + id + " connected");
	}

	/// <summary>
	/// Called when a player disconnects, on all peers.
	/// </summary>
	/// <param name="id">ID of the player that disconnected</param>
	private void PeerDisconnected(long id)
	{
		if (id == 1)
		{
			GD.Print("Server disconnected!");
			GetTree().Quit();
		}
		else
		{
			GD.Print("Player " + id + " disconnected!");
			GameManager.Players.Remove(GameManager.Players.Where(i => i.Id == id).First());
			var players = GetTree().GetNodesInGroup("Player");
			foreach (var item in players)
			{
				if (item.Name == id.ToString())
				{
					item.QueueFree();
				}
			}
		}
	}

	/// <summary>
	/// Called when connection is established, only on client side.
	/// </summary>
	private void ConnectedToServer()
	{
		GD.Print("Connected to server!");
		RpcId(1, nameof(SendPlayerData), GetNode<LineEdit>("Name").Text, Multiplayer.GetUniqueId());
	}

	/// <summary>
	/// Called when connection fails, only on client side.
	/// </summary>
	private void ConnectionFailed()
	{
		GD.Print("Connection failed!");
	}
	
	public void _on_quit_button_down()
	{
		GetTree().Quit();
	}

	private void _on_host_button_down()
	{
		var error = _peer.CreateServer(_port, 8);
		if (error != Error.Ok)
		{
			GD.Print("Error creating server: " + error);
			return;
		}
		_peer.Host.Compress(ENetConnection.CompressionMode.Fastlz);
		
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Waiting for players...");
		SendPlayerData(GetNode<LineEdit>("Name").Text, 1);
		DisableButtons();
		
		GetNode<ServerBrowser>("ServerBrowser").SetUpBroadcast(GetNode<LineEdit>("Name").Text + "'s Server");
	}

	private void _on_join_button_down()
	{
		JoinServer(GetNode<LineEdit>("Address").Text);
	}
	
	private void JoinServer(string serverIp)
	{
		_peer.CreateClient(serverIp, _port);
		_peer.Host.Compress(ENetConnection.CompressionMode.Fastlz);
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Joining Game!");
		DisableButtons();
	}

	private void _on_start_button_down()
	{
		Rpc(nameof(StartGame));
	}

	// Reliable by default
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame()
	{
		GetNode<ServerBrowser>("ServerBrowser").Clean();
		GetTree().Root.AddChild(new Node2D()
		{
			YSortEnabled = true,
			Name = "Base"
		});
		
		if (GetTree().HasGroup("World"))
		{
			return;
		}
		var scene = ResourceLoader.Load<PackedScene>("res://scenes/emberwood.tscn").Instantiate<Node2D>();
		GetTree().Root.GetNode("/root/Base").AddChild(scene);
		InstantiatePlayers();
		Hide();
		foreach (var item in GameManager.Players)
		{
			GD.Print(item.Id + " is playing!");
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SendPlayerData(string name, long id)
	{
		PlayerInfo playerInfo = new PlayerInfo
		{
			Name = name,
			Number = GameManager.Players.Count + 1,
			Id = id
		};
		if (!GameManager.ContainId(playerInfo.Id))
		{
			GameManager.Players.Add(playerInfo);
		}

		if (Multiplayer.IsServer())
		{
			foreach (var item in GameManager.Players)
			{
				Rpc(nameof(SendPlayerData), item.Name, item.Id);
			}
		}
	}
	
	private void DisableButtons()
	{
		GetNode<Button>("Host").Disabled = true;
		GetNode<Button>("Join").Disabled = true;
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
			return;

		PlayerNode currentPlayerNode = _playerScene.Instantiate<PlayerNode>();
		currentPlayerNode.Name = playerInfo.Id.ToString();
		GetTree().Root.GetNode("/root/Base").AddChild(currentPlayerNode);
		
		// Need to load existing statistics or default
		currentPlayerNode.Statistics = new Statistics();
		currentPlayerNode.Position = new Vector2(527, 406);
		currentPlayerNode.CurrentWorld = (Node2D) GetTree().Root.GetNode("/root/Base").GetChildren().First(e => e is not PlayerNode);

		var nameLabel = currentPlayerNode.GetNode<Label>("Name");
		if (playerInfo.Name == "")
			nameLabel.Text += playerInfo.Number.ToString();
		else
			nameLabel.Text = playerInfo.Name;

		// Only adds to the controlled player
		if (playerInfo.Id == Multiplayer.GetUniqueId())
		{
			// Adds the camera to the player
			int zoom = 3;
			var camera = new Camera2D
			{
				Name = "Camera",
				Zoom = new Vector2(zoom, zoom),
				LimitBottom = 6688,
				LimitRight = 4160,
				LimitLeft = 0,
				LimitTop = 0,
				LimitSmoothed = true
			};
			currentPlayerNode.AddChild(camera);

			// Adds the inventory to the player
			var inventory = _inventoryScene.Instantiate<Inventory>();
			inventory.Position = -GetViewportRect().Size / (2 * zoom);
			inventory.Visible = false;
			currentPlayerNode.AddChild(inventory);
			currentPlayerNode.Inventory = inventory;
		}
	}
}
