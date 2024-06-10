using Godot;
using Godot.Collections;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.rewards;

public abstract class Reward
{
    public int Amount { get; }
    public bool IsValid;
    
    protected Reward(int amount)
    {
        Amount = amount;
    }
    
    protected Reward(Dictionary reward)
    {
        var amount = Toolbox.LoadProperty(reward, nameof(Amount));
        if (amount == null)
        {
            GD.PrintErr("Error parsing quest: Reward properties not found.");
            IsValid = false;
        }
        else
        {
            Amount = (int)amount;
            IsValid = true;
        }
    }
    
    public abstract void GiveReward(Player player);
}