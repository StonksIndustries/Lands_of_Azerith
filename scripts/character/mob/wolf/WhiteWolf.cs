using System.Collections.Generic;

namespace LandsOfAzerith.scripts.character.mob.wolf;

public partial class WhiteWolf : Mob
{
    public override ulong HealthPoints { get; protected set; }

    public override ulong MaxHealthPoints { get; }
    protected override Character? Aggro { get; set; } = null;
    public override List<Mob> Preys => new List<Mob>();
    public override List<Mob> Predators => new List<Mob>();
    public override List<Zones> Zones => new List<Zones>()
    {
        scripts.Zones.WhiteVolcano,
        scripts.Zones.RadioactiveSteppes
    };
}