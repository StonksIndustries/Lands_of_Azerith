using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.goals;

public class HaveItem : Goal
{
    public readonly string ItemId;
    private readonly int  _startAmount;
    public int AmountCollected
    {
        get
        {
            if (Player.Statistics.ItemsCollected.ContainsKey(ItemId))
                return 0;
            else if (UseStatistics)
                return Player.Statistics.ItemsCollected[ItemId];
            else
                return Player.Statistics.ItemsCollected[ItemId] - _startAmount;
        }
    }

    public HaveItem(bool useStatistics, int targetGoal, Player player, string itemId) : base(useStatistics, targetGoal, player)
    {
        ItemId = itemId;
        if (Player.Statistics.ItemsCollected.ContainsKey(ItemId))
            _startAmount = Player.Statistics.ItemsCollected[ItemId];
        else
            _startAmount = 0;
    }

    public override int CheckProgress()
    {
        return AmountCollected;
    }
}