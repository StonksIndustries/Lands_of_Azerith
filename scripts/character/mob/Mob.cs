using System.Collections.Generic;

namespace LandsOfAzerith.scripts.character.mob;

public abstract class Mob : Character
{
    public abstract List<Mob> Preys { get; }
    public abstract List<Mob> Predators { get;}
    public abstract List<Zones> Zones { get;}
}