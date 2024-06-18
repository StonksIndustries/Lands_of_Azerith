using Godot;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.poi;

public partial class Transition : Checkpoint
{
    [Export] public string NextSceneId;
    [Export] public Vector2 NextScenePosition;
    [Export] public CollisionShape2D CollisionShape2D;
    private bool _isTransitioning = false;

    public override void _Process(double delta)
    {
        if (_isTransitioning)
            TeleportToZone();
    }

    private void _on_transition_body_entered(Node2D body)
    {
        if (body is PlayerNode)
        {
            _isTransitioning = true;
            var loadingScreen = GD.Load<PackedScene>("res://scenes/loading_screen.tscn").Instantiate<Control>();
            loadingScreen.Position = GetTree().Root.GetCamera2D().GetScreenCenterPosition();
            GetTree().Root.AddChild(loadingScreen);
            // Loading screen doesn't work, I don't know why
        }
    }

    private void TeleportToZone()
    {
        var player = GetTree().Root.GetNode<PlayerNode>($"/root/Base/{Multiplayer.GetUniqueId()}");
        GD.Print(player.Name);
        player.Position = NextScenePosition;
        player.Statistics.Checkpoint = NextScenePosition;
        
        var newScene = GD.Load<PackedScene>("res://scenes/" + NextSceneId + ".tscn").Instantiate<Map>();
        player.CurrentWorld.QueueFree();
        player.CurrentWorld = newScene;
        
        var camera = player.GetNodeOrNull<Camera2D>("Camera");
        if (camera != null)
            newScene.SetCameraLimits(camera);
        
        GetTree().Root.GetNode("/root/Base").CallDeferred(Node.MethodName.AddChild,newScene);
        GetTree().Root.GetNode("/root/LoadingScreen").CallDeferred(Node.MethodName.QueueFree);
    }
}