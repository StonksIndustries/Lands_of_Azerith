using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
using Godot.Collections;
using LandsOfAzerith.scripts.quests;

namespace LandsOfAzerith.scripts;

public class Statistics
{
    public Godot.Collections.Dictionary<string,int> MobKilled { get; set; }
    public Godot.Collections.Dictionary<string,int> ItemsCollected { get; set; }
    public int DistanceWalked { get; set; }
    public int Level { get; set; }
    public int ResourcesCollected { get; set; }
    public List<Quest> QuestsCompleted { get; set; }
    [JsonIgnore] public int CompletedQuestAmount => QuestsCompleted.Count;
    public int DamageDealt { get; set; }
    public int DamageTaken { get; set; }
    
}