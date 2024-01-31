namespace LandsofAzerith.scripts.Character;

public class Character
{
    public long HealthPoints { get; private set; }
    private Character? _aggro = null;

    public Character()
    {
        HealthPoints = 100;
    }

    public Character(long healthPoints)
    {
        HealthPoints = healthPoints;
    }
}