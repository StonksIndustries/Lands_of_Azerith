using System;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.rewards;

public class XpReward : Reward
{
    public XpReward(int amount) : base(amount)
    {
    }
    
    public override void GiveReward(Player player)
    {
        throw new NotImplementedException();
    }
}