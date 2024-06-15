using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts;

public partial class GameManager : Node
{
    public static List<PlayerInfo> Players = new List<PlayerInfo>();
    

    public static bool ContainId(long id)
    {
        return Players.Exists(i => i.Id == id);
    }
    
    public static PlayerInfo? GetPlayer(long id)
    {
        return Players.Find(i => i.Id == id);
    }
    
    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
            GetTree().Quit();
    }
}