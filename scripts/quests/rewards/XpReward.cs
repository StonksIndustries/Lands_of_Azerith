using System;
using Godot.Collections;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.rewards;

public class XpReward : Reward
{
    public XpReward(int amount) : base(amount)
    {
    }
    
    public XpReward(Dictionary reward) : base(reward)
    {
    }
    
    public override void GiveReward(Player player)
    {
        throw new NotImplementedException();
    }
}