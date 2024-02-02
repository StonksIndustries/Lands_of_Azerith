using Godot;

namespace LandsofAzerith.scripts;

public abstract class Character : CharacterBody2D
{
    public abstract long Health { get; set; }
    private Character? _aggro;
    
}