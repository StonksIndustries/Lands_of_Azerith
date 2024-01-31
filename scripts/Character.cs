using Godot;

namespace LandsofAzerith.scripts;

public class Character
{
    public long HealthPoints { get; private set; }
    private Character? Aggro = null;

    public Character()
    {
        HealthPoints = 100;
    }

    public Character(long healthPoints)
    {
        HealthPoints = healthPoints;
    }
}