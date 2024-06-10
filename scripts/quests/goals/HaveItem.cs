using Godot.Collections;
using LandsOfAzerith.scripts.character;
using Godot;

namespace LandsOfAzerith.scripts.quests.goals;

public class HaveItem : Goal
{
    private readonly string _itemId;
    private readonly int  _startAmount;

    public override int Progression => AmountCollected;

    private int AmountCollected
    {
        get
        {
            if (Player.Statistics.ItemsCollected.ContainsKey(_itemId))
                return 0;
            else if (UseStatistics)
                return Player.Statistics.ItemsCollected[_itemId];
            else
                return Player.Statistics.ItemsCollected[_itemId] - _startAmount;
        }
    }

    public HaveItem(bool useStatistics, int targetGoal, Player player, string itemId) : base(useStatistics, targetGoal, player)
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
    }
}