using System.Text.Json.Serialization;
using System.Text.Json;
using LandsOfAzerith.scripts.character;
using Godot;
using Godot.Collections;

namespace LandsOfAzerith.scripts.quests.goals;

[JsonDerivedType(typeof(GoToPlace), nameof(GoToPlace))]
[JsonDerivedType(typeof(HaveItem), nameof(HaveItem))]
[JsonDerivedType(typeof(KillMob), nameof(KillMob))]
public abstract class Goal
{
    public bool UseStatistics { get; set; }
    public int TargetGoal { get; set; }
    public readonly Player Player;
    public abstract int Progression { get; }
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
}