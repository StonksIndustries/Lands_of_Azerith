using Godot;
using Godot.Collections;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.rewards;

public class ItemReward : Reward
{
    public readonly string ItemId;
    public ItemReward(int amount, string itemId) : base(amount)
    {
        ItemId = itemId;
    }
    
    // Don't mind the warning, if there's a problem there should be a problem
    public ItemReward(Dictionary reward) : base(reward)
    {
        var itemId = Toolbox.LoadProperty(reward, nameof(ItemId));
        if (itemId == null)
        {
            GD.PrintErr("Error parsing quest: ItemReward properties not found.");
            IsValid = false;
        }
        else
        {
            ItemId = (string)itemId;
        }
    }

    public override void GiveReward(Player player)
    {
        throw new System.NotImplementedException();
    }
}