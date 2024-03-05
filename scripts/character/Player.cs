using Godot;

namespace LandsOfAzerith.scripts.character;

public class Player : Character
{
    public override ulong HealthPoints { get; }
    protected override Character? Aggro { get; }
    public int Strength { get; private set; }
    public int Stamina { get; private set; }
    public int Mana { get; private set; }
    
    public Player()
    {
        Strength = 10;
        Stamina = 10;
        HealthPoints = 10;
        Mana = 10;
    }

}