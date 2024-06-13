using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using LandsOfAzerith.scripts;
using System.Linq;
using System.Text.Json;
using LandsOfAzerith.scripts.character;
using LandsOfAzerith.scripts.main_menu;
using LandsOfAzerith.scripts.quests;
using LandsOfAzerith.scripts.quests.goals;
using LandsOfAzerith.scripts.quests.rewards;

namespace LandsOfAzerith.scripts;

public partial class MainMenu : Control
{
	[Export]
	private int _port = 8901;

	[Export]
	private string _address = "127.0.0.1";

	private ENetMultiplayerPeer _peer = new ENetMultiplayerPeer();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;

		GetNode<ServerBrowser>("ServerBrowser").JoinServer += JoinServer;

		// For testing purposes
		/*var file2 = File.Create("quests/test2.json");
		file2.Write(JsonSerializer.Serialize(JsonSerializer.Deserialize<Quest>(File.ReadAllText("quests/test.json")), new JsonSerializerOptions{WriteIndented = true, IncludeFields = true}).ToAsciiBuffer());
		file2.Close();
		var file = File.OpenRead("quests/test.json");
		file.Write(JsonSerializer.Serialize(new Quest
		{
			Description = "Test quest",
			Name = "Test",
			Goals = new List<Goal>
			{
				new HaveItem
				{
					TargetGoal = 3,
					UseStatistics = false,
					ItemId = "1",
					StartAmount = 0
				},
				new KillMob
				{
					TargetGoal = 5,
					UseStatistics = true,
					MobId = "3",
					StartAmount = 2
				},
				new GoToPlace
				{
					Coordinates = new Vector2(3,4),
					Radius = 5,
					TargetGoal = 1,
					UseStatistics = false
				}
			},
			Rewards = new List<Reward>()
		}, new JsonSerializerOptions{WriteIndented = true, IncludeFields = true}).ToAsciiBuffer());
		file.Close();*/
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
		if (GetTree().HasGroup("World"))
		{
			return;
		}
		var scene = ResourceLoader.Load<PackedScene>("res://scenes/emberwood.tscn").Instantiate<Node2D>();
		GetTree().Root.AddChild(scene);
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
}
