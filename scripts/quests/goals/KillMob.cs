using LandsOfAzerith.scripts.character;
using Godot;
using Godot.Collections;

namespace LandsOfAzerith.scripts.quests.goals;

public class KillMob : Goal
{
    private readonly string _mobId;
    private readonly int  _startAmount;

    private int AmountKilled
    {
        get
        {
            if (Player.Statistics.MobKilled.ContainsKey(_mobId))
                return 0;
            else if (UseStatistics)
                return Player.Statistics.MobKilled[_mobId];
            else
                return Player.Statistics.MobKilled[_mobId] - _startAmount;
        }
    }

    public KillMob(bool useStatistics, int targetGoal, Player player, string mobId) : base(useStatistics, targetGoal, player)
    {
        _mobId = mobId;
        if (Player.Statistics.MobKilled.ContainsKey(_mobId))
            _startAmount = Player.Statistics.MobKilled[_mobId];
        else
            _startAmount = 0;
    }
    
    // Don't mind the warning, if there is a problem, there should be a problem
    public KillMob(Dictionary goal, Player player) : base(goal, player)
    {
        var mobId = Toolbox.LoadProperty(goal, nameof(_mobId));
        if (mobId == null)
        {
            GD.PrintErr("Error parsing quest: KillMob properties not found.");
            IsValid = false;
        }
        else
        {
            _mobId = (string)mobId;
            if (Player.Statistics.MobKilled.ContainsKey(_mobId))
                _startAmount = Player.Statistics.MobKilled[_mobId];
            else
                _startAmount = 0;
        }
    }

    public override int CheckProgress()
    {
        return AmountKilled;
    }
}