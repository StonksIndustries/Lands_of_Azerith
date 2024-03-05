using System.Collections.Generic;

namespace LandsofAzerith.scripts.character.mob.wolf;

public class Wolf : Mob
{
    public override ulong HealthPoints => 200;
    public override List<Mob> Preys => new List<Mob>();
    public override List<Mob> Predators => new List<Mob>();
    public override List<Zones> Zones => new List<Zones>()
    {
        scripts.Zones.BlackForest,
        scripts.Zones.PrairieParadise,
        scripts.Zones.ChampignomeBosquet
    };
}