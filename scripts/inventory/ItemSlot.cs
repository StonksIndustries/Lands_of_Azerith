using Godot;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon.melee;
using LandsOfAzerith.scripts.item.weapon.ranged;
using LandsOfAzerith.scripts.item.weapon.ranged.projectile;

namespace LandsOfAzerith.scripts.inventory;

public partial class ItemSlot : Control
{
	public Item? Item;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Name == "Slot1")
			Item = new Sword();
		else if (Name == "Slot2")
			Item = new Bow();
		else if (Name == "Slot3")
			Item = new Arrow();
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
			texture.Texture = ResourceLoader.Load<Texture2D>(Item.TexturePath);
			label.Text = Item.Name;
		}
	}
}