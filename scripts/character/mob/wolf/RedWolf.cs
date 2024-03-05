using System.Collections.Generic;

namespace LandsOfAzerith.scripts.character.mob.wolf;

public class RedWolf : Mob
{
    public override ulong HealthPoints => 200;
    protected override Character? Aggro { get; } = null;
    public override List<Mob> Preys => new List<Mob>();
    public override List<Mob> Predators => new List<Mob>();
    public override List<Zones> Zones => new List<Zones>()
    {
        scripts.Zones.CarminCaves,
        scripts.Zones.BrambleBog
    };
}