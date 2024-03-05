using System.Collections.Generic;

namespace LandsOfAzerith.scripts.character.mob.human;

public class Mercenary : Mob
{
    public override ulong HealthPoints => 100;
    protected override Character? Aggro { get; } = null;
    public override List<Mob> Preys => new List<Mob>();
    public override List<Mob> Predators => new List<Mob>();
    public override List<Zones> Zones => new List<Zones>()
    {
        scripts.Zones.RuinedRunes,
        scripts.Zones.RadioactiveSteppes,
        scripts.Zones.AncientCity,
        scripts.Zones.GravityCrater
    };
}