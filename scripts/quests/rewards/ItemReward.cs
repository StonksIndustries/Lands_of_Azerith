using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.rewards;

public class ItemReward : Reward
{
    public readonly string ItemId;
    public ItemReward(int amount, string itemId) : base(amount)
    {
        ItemId = itemId;
    }

    public override void GiveReward(Player player)
    {
        throw new System.NotImplementedException();
    }
}