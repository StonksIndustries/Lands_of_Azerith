using System;
using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts.character.mob;

public partial class SpawnZone : Area2D
{
	private Random _random = new Random();
	public List<Mob> Mobs = new List<Mob>();
	[Export] public PackedScene MobScene;
	[Export] public string MobId;
	[Export] public int MaxMobs = 5;
	[Export] public CollisionShape2D CollisionShape2D;
	[Export] public Vector2 ZoneSize;

	public override void _Ready()
	{
		CollisionShape2D.Shape = new RectangleShape2D { Size = ZoneSize };
		if (GetMultiplayerAuthority() != 1)
			GetNode<Timer>("Timer").Stop();
	}
	
	private void _on_respawn_timeout()
	{
		if (Mobs.Count < MaxMobs)
		{
			Rect2 rect = CollisionShape2D.Shape.GetRect();
			Vector2 spawnPosition = GlobalPosition + rect.Position + new Vector2(_random.Next((int)rect.Size.X), _random.Next((int)rect.Size.Y));
			Rpc("SpawnMob", spawnPosition);
		}
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void SpawnMob(Vector2 position)
	{
		var mob = MobScene.Instantiate<Mob>();
		mob.MobId = MobId;
		mob.Position = position;
		mob.LoadStats();
		GetParent().AddChild(mob);
		Mobs.Add(mob);
	}
}
