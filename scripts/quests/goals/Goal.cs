using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.goals;

public abstract class Goal
{
    public readonly bool UseStatistics;
    public readonly int TargetGoal;
    public readonly Player Player;
    public int Progression => CheckProgress();
    public bool IsCompleted => Progression >= TargetGoal;

    public Goal(bool useStatistics, int targetGoal, Player player)
    {
        Player = player;
        TargetGoal = targetGoal;
        UseStatistics = useStatistics;
    }
    
    public abstract int CheckProgress();
}