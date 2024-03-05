namespace LandsofAzerith.scripts.character;

public abstract class Character
{
    public abstract ulong HealthPoints { get;}
    private Character? _aggro = null;
}