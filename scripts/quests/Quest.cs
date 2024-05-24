using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LandsOfAzerith.scripts.character;
using LandsOfAzerith.scripts.quests.goals;
using LandsOfAzerith.scripts.quests.rewards;

namespace LandsOfAzerith.scripts.quests;

public class Quest
{
    public static string Path => "res://quests/";
    public List<Goal> Goals { get; }
    public List<Reward> Rewards { get; }
    public string Name { get; }
    public string Description { get; }
    
    public Quest(string questId)
    {
        throw new NotImplementedException();
    }

    public bool Finish(Player player)
    {
        if (Goals.All(e => e.IsCompleted))
        {
            foreach (var reward in Rewards)
            {
                reward.GiveReward(player);
            }
            return true;
        }

        return false;
    }
    
    public void LoadQuest(string questId)
    {
        string path = Path + questId + ".json";
        if (!FileAccess.FileExists(path))
        {
            GD.PrintErr($"Error parsing quest: {questId} does not exist.");
            return;
        }
        
        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        Json json = new Json();
        var error = json.Parse(file.GetAsText());
        file.Close();
        
        if (error != Error.Ok)
        {
            GD.PrintErr($"Error parsing loot table: {json.GetErrorMessage()} at line {json.GetErrorLine()}.");
            return;
        }
        var lootTable = (Godot.Collections.Dictionary) json.Data;
        
    }
}