using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.goals;

public class KillMob : Goal
{
    public readonly string MobId;
    private readonly int  _startAmount;
    public int AmountKilled
    {
        get
        {
            if (Player.Statistics.MobKilled.ContainsKey(MobId))
                return 0;
            else if (UseStatistics)
                return Player.Statistics.MobKilled[MobId];
            else
                return Player.Statistics.MobKilled[MobId] - _startAmount;
        }
    }

    public KillMob(bool useStatistics, int targetGoal, Player player, string mobId) : base(useStatistics, targetGoal, player)
    {
        MobId = mobId;
        if (Player.Statistics.MobKilled.ContainsKey(MobId))
            _startAmount = Player.Statistics.MobKilled[MobId];
        else
            _startAmount = 0;
    }

    public override int CheckProgress()
    {
        return AmountKilled;
    }
}