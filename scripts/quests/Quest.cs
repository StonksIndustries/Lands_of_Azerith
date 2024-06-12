using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Godot;
using LandsOfAzerith.scripts.character;
using LandsOfAzerith.scripts.quests.goals;
using LandsOfAzerith.scripts.quests.rewards;

namespace LandsOfAzerith.scripts.quests;

public class Quest
{
    private static string Path => "res://quests/";
    public List<Goal> Goals { get; set; }
    public List<Reward> Rewards { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [JsonIgnore] public bool IsCompleted => Goals.All(e => e.IsCompleted(PlayerNode));
    [JsonIgnore] public PlayerNode PlayerNode;
    
    public static Quest? Load(string questId, PlayerNode playerNode)
    {
        var quest = Toolbox.LoadFileInJson<Quest>(Path + questId + ".json");
        if (quest is not null)
            quest.PlayerNode = playerNode;
        return quest;
    }

    public bool GetReward()
    {
        if (IsCompleted)
            foreach (var reward in Rewards)
                reward.GiveReward(PlayerNode);
        return IsCompleted;
    }
}