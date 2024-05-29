using LandsOfAzerith.scripts.character;
using Godot;
using Godot.Collections;

namespace LandsOfAzerith.scripts.quests.goals;

public abstract class Goal
{
    public readonly bool UseStatistics;
    public readonly int TargetGoal;
    public readonly Player Player;
    public int Progression => CheckProgress();
    public bool IsCompleted => Progression >= TargetGoal;
    public bool IsValid;

    public Goal(bool useStatistics, int targetGoal, Player player)
    {
        Player = player;
        TargetGoal = targetGoal;
        UseStatistics = useStatistics;
    }
    
    public Goal(Dictionary goal, Player player)
    {
        Player = player;
        var useStatistics = Toolbox.LoadProperty(goal, nameof(UseStatistics));
        var targetGoal = Toolbox.LoadProperty(goal, nameof(TargetGoal));
        if (useStatistics == null || targetGoal == null)
        {
            GD.PrintErr("Error parsing quest: Goal properties not found.");
            IsValid = false;
        }
        else
        {
            UseStatistics = (bool)useStatistics;
            TargetGoal = (int)targetGoal;
            IsValid = true;
        }
    }
    
    public abstract int CheckProgress();
}