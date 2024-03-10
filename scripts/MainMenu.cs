using Godot;
using System;
using LandsOfAzerith.scripts;

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
		GD.Print("Player " + id + " connected!");	
	}

	/// <summary>
	/// Called when a player disconnects, on all peers.
	/// </summary>
	/// <param name="id">ID of the player that disconnected</param>
	private void PeerDisconnected(long id)
	{
		GD.Print("Player " + id + " disconnected!");
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
	
	public void _on_host_button_down()
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
	}

	public void _on_join_button_down()
	{
		_peer.CreateClient(_address, _port);
		_peer.Host.Compress(ENetConnection.CompressionMode.Fastlz);
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Joining Game!");
	}

	public void _on_start_button_down()
	{
		Rpc("StartGame");
	}

	// Reliable by default
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame()
	{
		foreach (var item in GameManager.Players)
		{
			GD.Print(item.Name + " is playing!");
		}
		var scene = ResourceLoader.Load<PackedScene>("res://scenes/world.tscn").Instantiate<Node2D>();
        GetTree().Root.AddChild(scene);
        this.Hide();
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SendPlayerData(string name, long id)
	{
		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
			Id = id
		};
		if (!GameManager.Players.Contains(playerInfo))
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
}
