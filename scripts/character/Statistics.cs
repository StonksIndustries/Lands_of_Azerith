using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
using LandsOfAzerith.scripts.quests;

namespace LandsOfAzerith.scripts.character;

public class Statistics
{
    public Statistics(PlayerBackend player)
    {
        Player = player;
    }
    
    public Statistics(){}
    
    [JsonIgnore] public PlayerBackend Player;
    public Godot.Collections.Dictionary<string,int> MobKilled { get; set; }
    public Godot.Collections.Dictionary<string,int> ItemsCollected { get; set; }
    public int DistanceWalked { get; set; }
    public int Level { get; set; }
    public int ResourcesCollected { get; set; }
    public List<Quest> QuestsCompleted { get; set; }
    [JsonIgnore] public int CompletedQuestAmount => QuestsCompleted.Count;
    public int DamageDealt { get; set; }
    public int DamageTaken { get; set; }
    public uint Strength { get; set; }
    public uint HealthPoints { get; set; }
    public uint Speed { get; set; }
    public Vector2 Position { get => Player.Position; set => Player.Position = value; }
    public Vector2 Checkpoint { get; set; }
}