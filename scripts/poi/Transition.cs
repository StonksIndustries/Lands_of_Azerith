using Godot;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.poi;

public partial class Transition : Checkpoint
{
    [Export] public PackedScene NextScene;
    [Export] public Vector2 NextScenePosition;
    [Export] public CollisionShape2D CollisionShape2D;
    
    private void _on_transition_body_entered(Node2D body)
    {
        if (body is PlayerNode player)
        {
            player.Position = NextScenePosition;
            var newScene = NextScene.Instantiate<Node2D>();
            player.CurrentWorld.QueueFree();
            player.CurrentWorld = newScene;
            GetTree().Root.GetNode("/root/Base").AddChild(newScene);
        }
    }
}