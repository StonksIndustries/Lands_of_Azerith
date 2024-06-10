using Godot;
using Godot.Collections;
using LandsOfAzerith.scripts.character;

namespace LandsOfAzerith.scripts.quests.goals;

public class GoToPlace : Goal
{
    private readonly Vector2 _coordinates;
    private readonly double _radius;

    public override int Progression
    {
        get
        {
            if (Player.Position.DistanceTo(_coordinates) < _radius)
                return 1;
            else
                return 0;
        }
    }

    public GoToPlace(int targetGoal, Player player, Vector2 coordinates, int radius) : base(false, 1, player)
    {
        _coordinates = coordinates;
        _radius = radius;
    }
    
    public GoToPlace(Dictionary goal, Player player) : base(goal, player)
    {
        var coordinates = Toolbox.LoadProperty(goal, nameof(_coordinates));
        var radius = Toolbox.LoadProperty(goal, nameof(_radius));
        if (coordinates == null || radius == null)
        {
            GD.PrintErr("Error parsing quest: GoToPlace properties not found.");
            return;
        }
        _coordinates = (Vector2)coordinates;
        _radius = (int)radius;
    }
}