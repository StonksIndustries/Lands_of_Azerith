using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Godot;
using LandsOfAzerith.scripts.character.mob;
using LandsOfAzerith.scripts.inventory;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon;

namespace LandsOfAzerith.scripts.character;

// This class exists only for better organization of the code.
public abstract partial class PlayerBackend : Character
{
    [JsonConstructor]
    public PlayerBackend()
    {
        Statistics = new Statistics(this);
    }
    
    public Statistics Statistics { get; set; }
    public Inventory Inventory { get; set; }
    public override uint Strength { get => Statistics.Strength; set => Statistics.Strength = value; }
    public override Weapon Weapon { get; set; }
    public override uint HealthPoints { get => Statistics.HealthPoints; set => Statistics.HealthPoints = value; }
    public override uint Speed { get => Statistics.Speed; set => Statistics.Speed = value; }
    public override uint MaxHealthPoints => 100;
    protected readonly List<Mob> InRangeMobs = new List<Mob>();
}