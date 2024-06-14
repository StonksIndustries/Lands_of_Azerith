using System.Collections.Generic;
using LandsOfAzerith.scripts.item;

namespace LandsOfAzerith.scripts.character.mob;

public class MobStats
{
    public uint HealthPoints { get; set; }
    public uint Speed { get; set; }
    public Weapon Weapon { get; set; }
    public List<string> LootTable { get; set; }
    public string MobId { get; set; }
}