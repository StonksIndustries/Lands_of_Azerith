using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts;

public partial class GameManager : Node
{
    public static List<PlayerInfo> Players = new List<PlayerInfo>();
    public override void _Ready()
    {
    }
    
    public override void _Process(double delta)
    {
    }
}