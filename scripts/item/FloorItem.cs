using Godot;

namespace LandsOfAzerith.scripts.item;

public partial class FloorItem : Area2D
{
	public InventoryItem? Item { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Item != null)
			GetNode<Sprite2D>("Texture").Texture = GD.Load<Texture2D>(Item.Icon);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}