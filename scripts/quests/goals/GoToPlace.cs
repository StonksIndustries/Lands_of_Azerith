using Godot;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.goals;

public class GoToPlace : Goal
{
    public readonly Vector2 Coordinates;
    public readonly double Radius;
    public GoToPlace(bool useStatistics, int targetGoal, Player player, Vector2 coordinates, int radius) : base(useStatistics, 1, player)
    {
        Coordinates = coordinates;
        Radius = radius;
    }

    public override int CheckProgress()
    {
        if (Player.Position.DistanceTo(Coordinates) < Radius)
            return 1;
        else
            return 0;
    }
}