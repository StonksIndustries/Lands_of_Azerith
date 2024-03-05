using Godot;

namespace LandsOfAzerith.scripts;

public abstract class Character : CharacterBody2D
{
    public abstract long Health { get; set; }
    private Character? _aggro;
    
}