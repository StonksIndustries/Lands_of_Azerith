namespace LandsOfAzerith.scripts.character;

public abstract class Character
{
    public abstract ulong HealthPoints { get;}
    protected abstract Character? Aggro { get; }
}