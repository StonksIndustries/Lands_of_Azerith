using Godot;

namespace LandsofAzerith.scripts.character;

public class Player : Character
{
    public int Strength { get; }
    public int Stamina { get; }
    public override long Health { get; set; }
    public int Mana { get; }
    
    

    public Player()
    {
        Strength = 10;
        Stamina = 10;
        Health = 10;
        Mana = 10;
    }
    
    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.W))
        {
            Position += new Vector2(0, -10);
        }
    }
}