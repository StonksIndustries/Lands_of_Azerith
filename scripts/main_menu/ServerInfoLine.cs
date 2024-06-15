using Godot;

namespace LandsOfAzerith.scripts.main_menu;

public partial class ServerInfoLine : HBoxContainer
{
    public ServerInfo Info;

    [Signal]
    public delegate void JoinServerEventHandler(string serverIp);
    
    private void _on_join_button_pressed()
    {
        EmitSignal(SignalName.JoinServer, GetNode<Label>("ServerIP").Text);
    }
}