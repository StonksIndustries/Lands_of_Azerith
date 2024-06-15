using System.Text.Json;
using Godot;

namespace LandsOfAzerith.scripts.main_menu;


// Used to work, doesn't work anymore, don't have time to fix
public partial class ServerBrowser : Control
{
	[Export] private PacketPeerUdp? _broadcaster;
	[Export] private PacketPeerUdp _listener = new PacketPeerUdp();
	[Export] private int _listenPort = 8900;
	[Export] private int _hostPort = 8901;
	[Export] private string _broadcastAdrress = "192.168.1.255";
	[Export] private PackedScene _serverInfoScene = GD.Load<PackedScene>("res://scenes/server_info.tscn");
	[Signal] public delegate void JoinServerEventHandler(string serverIp);

	private Timer _broadcastTimer;
	private Timer _refreshTimer;
	private ServerInfo _serverInfo;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_broadcastTimer = GetNode<Timer>("Timer");
		_refreshTimer = GetNode<Timer>("RefreshTimer");
		SetUpListener();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SetUpBroadcast(string name)
	{
		_serverInfo = new ServerInfo
		{
			Name = name,
			PlayerCount = GameManager.Players.Count
		};
		
		_broadcaster = new PacketPeerUdp();
		_broadcaster.SetBroadcastEnabled(true);
		_broadcaster.SetDestAddress(_broadcastAdrress, _listenPort);

		var error = _broadcaster.Bind(_hostPort);
		if (error == Error.Ok)
			GD.Print("Broadcaster bound to port: " + _hostPort);
			
		
		_broadcastTimer.Start();
	}

	private void SetUpListener()
	{
		var error = _listener.Bind(_listenPort);
		if (error == Error.Ok)
			GD.Print("Listener bound to port: " + _listenPort);
		
		_refreshTimer.Start();
	}

	private void _on_timer_timeout()
	{
		GD.Print("Broadcasting server info");
		_serverInfo.PlayerCount = GameManager.Players.Count;

		string json = JsonSerializer.Serialize(_serverInfo);
		var packet = json.ToAsciiBuffer();

		// Timer is on only when the broadcaster is activated
		_broadcaster!.PutPacket(packet);
	}
	
	private void _on_join_server(string serverIp)
	{
		EmitSignal(SignalName.JoinServer, serverIp);
	}
	
	private void _on_refresh()
	{
		foreach (Node oldServers in GetNode("ServerList").GetChildren())
		{
			oldServers.Free();
		}
			
		while (_listener.GetAvailablePacketCount() > 0)
		{
			string serverIp = _listener.GetPacketIP();
			int port = _listener.GetPacketPort();
			byte[] packet = _listener.GetPacket();
			ServerInfo info = JsonSerializer.Deserialize<ServerInfo>(packet.GetStringFromAscii())!;
			GD.Print(
				$"Server IP : {serverIp}, Port : {port}, Server Name : {info.Name}, Player Count : {info.PlayerCount}");
				
			if (GetNodeOrNull($"ServerList/{info.Name}") is not null)
				return;
				
			ServerInfoLine serverInfo = _serverInfoScene.Instantiate<ServerInfoLine>();
			serverInfo.Name = info.Name;
			serverInfo.GetNode<Label>("Name").Text = info.Name;
			serverInfo.GetNode<Label>("PlayerCount").Text = info.PlayerCount.ToString();
			serverInfo.GetNode<Label>("ServerIP").Text = serverIp;
				
			GetNode("ServerList").AddChild(serverInfo);
				
			serverInfo.JoinServer += _on_join_server;
		}
	}

	public void Clean()
	{
		_listener.Close();
		_broadcastTimer.Stop();
		if(_broadcaster is not null)
			_broadcaster.Close();
	}
}