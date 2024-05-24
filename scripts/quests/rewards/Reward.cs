using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.rewards;

public abstract class Reward
{
    public int Amount { get; }
    
    protected Reward(int amount)
    {
        Amount = amount;
    }
    
    public abstract void GiveReward(Player player);
}