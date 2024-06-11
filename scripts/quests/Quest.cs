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
    
    // public Quest(string name, string description, List<Goal> goals, List<Reward> rewards, Player player)
    // {
    //     Name = name;
    //     Description = description;
    //     Goals = goals;
    //     Rewards = rewards;
    //     Player = player;
    // }
    
    public static Quest? Load(string questId, PlayerNode playerNode)
    {
        /*string path = Path + questId + ".json";
        var json = Toolbox.LoadFileInJson(path);
        if (json == null)
            return null;
        var quest = (Godot.Collections.Dictionary)json.Data;
        var name = Toolbox.LoadProperty(quest, nameof(Name));
        var description = Toolbox.LoadProperty(quest, nameof(Description));
        var goals = Toolbox.LoadProperty(quest, nameof(Goals));
        var rewards = Toolbox.LoadProperty(quest, nameof(Rewards));
        if (name == null || description == null || goals == null || rewards == null)
        {
            GD.PrintErr("Error parsing quest: Properties not found.");
            return null;
        }
        return new Quest((string)name, (string)description, LoadGoals((Array)goals, player), LoadRewards((Array)rewards, player), player);*/
        
        // I'm so sad that I could do only this instead of the above code
        var quest = Toolbox.LoadFileInJson<Quest>(Path + questId + ".json");
        if (quest is not null)
            quest.PlayerNode = playerNode;
        return quest;
    }
    
    /*public static List<Reward> LoadRewards(Array rewards, Player player)
    {
        var rewardList = new List<Reward>();
        foreach (Dictionary reward in rewards)
        {
            var rewardType = Toolbox.LoadProperty(reward, "Type");
            if (rewardType == null)
                GD.PrintErr("Error parsing quest: Reward type not found.");
            else
            {
                switch ((string)rewardType)
                {
                    case nameof(ItemReward):
                        Reward instance = new ItemReward(reward);
                        if (instance.IsValid)
                            rewardList.Add(instance);
                        break;
                    case nameof(XpReward):
                        instance = new XpReward(reward);
                        if (instance.IsValid)
                            rewardList.Add(instance);
                        break;
                    default:
                        GD.PrintErr("Error parsing quest: Reward type invalid.");
                        break;
                }
            }
        }
        return rewardList;
    }*/
    
    /*public static List<Goal> LoadGoals(Array goals, Player player)
    {
        var goalList = new List<Goal>();
        foreach (Dictionary goal in goals)
        {
            var goalType = Toolbox.LoadProperty(goal, "Type");
            if (goalType == null)
                GD.PrintErr("Error parsing quest: Goal type not found.");
            else
            {
                switch ((string)goalType)
                {
                    case nameof(KillMob):
                        Goal instance = new KillMob(goal, player);
                        if (instance.IsValid)
                            goalList.Add(instance);
                        break;
                    case nameof(HaveItem):
                        instance = new HaveItem(goal, player);
                        if (instance.IsValid)
                            goalList.Add(instance);
                        break;
                    case nameof(GoToPlace):
                        instance = new GoToPlace(goal, player);
                        if (instance.IsValid)
                            goalList.Add(instance);
                        break;
                    default:
                        GD.PrintErr("Error parsing quest: Goal type invalid.");
                        break;
                }
            }
        }
        return goalList;
    }*/

    public bool GetReward()
    {
        if (IsCompleted)
            foreach (var reward in Rewards)
                reward.GiveReward(PlayerNode);
        return IsCompleted;
    }
}