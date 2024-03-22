using Godot;
using System;
using LandsOfAzerith.scripts;
using System.Linq;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts;

public partial class MainMenu : Control
{
	[Export]
	private int _port = 1234;

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
		RpcId(1, "SendPlayerData", GetNode<LineEdit>("Name").Text, Multiplayer.GetUniqueId());
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
	
	public void _on_host_button_down()
	{
		_port = GetNode<LineEdit>("Port").Text.ToInt();
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
	}

	public void _on_join_button_down()
	{
		_address = GetNode<LineEdit>("Address").Text;
		_port = GetNode<LineEdit>("Port").Text.ToInt();
		_peer.CreateClient(GetNode<LineEdit>("Address").Text, GetNode<LineEdit>("Port").Text.ToInt());
		_peer.Host.Compress(ENetConnection.CompressionMode.Fastlz);
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Joining Game!");
		DisableButtons();
	}

	public void _on_start_button_down()
	{
		Rpc("StartGame");
	}

	// Reliable by default
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame()
	{
		if (GetTree().HasGroup("World"))
		{
			return;
		}
		var scene = ResourceLoader.Load<PackedScene>("res://scenes/world.tscn").Instantiate<Node2D>();
		GetTree().Root.AddChild(scene);
		this.Hide();
		foreach (var item in GameManager.Players)
		{
			GD.Print(item.Id + " is playing!");
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SendPlayerData(string name, long id)
	{
		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
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
				Rpc("SendPlayerData", item.Name, item.Id);
			}
		}
	}
	
	private void DisableButtons()
	{
		GetNode<Button>("Host").Disabled = true;
		GetNode<Button>("Join").Disabled = true;
	}
}
