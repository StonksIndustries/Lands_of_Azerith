using Godot;
using Godot.Collections;
using LandsOfAzerith.scripts.item;

namespace LandsOfAzerith.scripts.inventory;

public partial class InventorySlot : Control
{
	public InventoryItem? Item;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Refresh();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void Refresh()
	{
		var texture = GetNodeOrNull<TextureRect>("Texture");
		var label = GetNodeOrNull<Label>("Label");
		
		if (Item == null)
		{
			texture.Texture = null;
			label.Text = "";
		}
		else
		{
			texture.Texture = ResourceLoader.Load<Texture2D>(Item.Icon);
			label.Text = Item.Name;
		}
	}
}