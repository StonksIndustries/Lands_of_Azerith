using System;
using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts.character.mob;

public partial class SpawnZone : Area2D
{
	private Random _random = new Random();
	public List<Mob> Mobs = new List<Mob>();
	[Export] public PackedScene MobScene;
	[Export] public int MaxMobs = 5;
	[Export] public CollisionShape2D CollisionShape2D;
	
	private void _on_respawn_timeout()
	{
		if (Mobs.Count < MaxMobs)
		{
			var mob = MobScene.Instantiate<Mob>();
			Rect2 rect = CollisionShape2D.Shape.GetRect();
			Vector2 spawnPosition = new Vector2(_random.Next((int)rect.Size.X), _random.Next((int)rect.Size.Y));
			mob.Position = GlobalPosition + spawnPosition;
			GetParent().AddChild(mob);
			Mobs.Add(mob);
		}
	}
}