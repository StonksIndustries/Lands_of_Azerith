using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts.inventory;

public partial class InventoryRow : Control
{
	private List<InventorySlot> _slots = new List<InventorySlot>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeSlots();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void InitializeSlots()
	{
		foreach (Node child in GetChildren())
		{
			if (child is InventorySlot slot)
			{
				_slots.Add(slot);
			}
		}
	}
	
	public Godot.Collections.Dictionary<string, Variant> Save()
	{
		var result = new Godot.Collections.Dictionary<string, Variant>()
		{
			{ nameof(Name), Name }
		};
		foreach (var slot in _slots)
		{
			result.Add(slot.Name, slot.Save());
		}
		return result;
	}
}