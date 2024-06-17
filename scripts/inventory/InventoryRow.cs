using System.Collections.Generic;
using System.Linq;
using Godot;

namespace LandsOfAzerith.scripts.inventory;

public partial class InventoryRow : Control
{
	public bool IsEmpty => Slots.All(slot => slot.IsEmpty);

	public List<InventorySlot> Slots = new List<InventorySlot>();
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
				Slots.Add(slot);
			}
		}
	}
	
	public void Refresh()
	{
		Slots.ForEach(e => e.Refresh());
	}
}