using Godot.Collections;
using LandsOfAzerith.scripts.character;
using Godot;

namespace LandsOfAzerith.scripts.quests.goals;

public class HaveItem : Goal
{

    public string ItemId { get; set; }
    public int  StartAmount { get; set; }

    public override int Progression(Player player) => AmountCollected(player);

    private int AmountCollected(Player player)
    {
        if (player.Statistics.ItemsCollected.ContainsKey(ItemId))
            return 0;
        else if (UseStatistics)
            return player.Statistics.ItemsCollected[ItemId];
        else
            return player.Statistics.ItemsCollected[ItemId] - StartAmount;
    }

    /*public HaveItem(bool useStatistics, int targetGoal, Player player, string itemId) : base(useStatistics, targetGoal, player)
    {
        _itemId = itemId;
        if (Player.Statistics.ItemsCollected.ContainsKey(_itemId))
            _startAmount = Player.Statistics.ItemsCollected[_itemId];
        else
            _startAmount = 0;
    }
    
    // Don't mind the warning, if there is a problem, there should be a problem
    public HaveItem(Dictionary goal, Player player) : base(goal, player)
    {
        var itemId = Toolbox.LoadProperty(goal, nameof(_itemId));
        if (itemId is null)
        {
            GD.PrintErr("Error parsing quest: HaveItem properties not found.");
            IsValid = false;
        }
        else
        {
            _itemId = (string)itemId;
            if (Player.Statistics.ItemsCollected.ContainsKey(_itemId))
                _startAmount = Player.Statistics.ItemsCollected[_itemId];
            else
                _startAmount = 0;
        }
    }*/
}