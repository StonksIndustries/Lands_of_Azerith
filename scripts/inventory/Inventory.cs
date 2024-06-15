using System.Collections.Generic;
using System.Linq;
using Godot;
using LandsOfAzerith.scripts.item;

namespace LandsOfAzerith.scripts.inventory;

public partial class Inventory : Control
{
	public bool IsEmpty => Rows.All(row => row.IsEmpty);
	
	public List<InventoryRow> Rows = new List<InventoryRow>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeRows();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Note: This method is not final, and may be changed in the future.
	private void InitializeRows()
	{
		foreach (Node child in GetChildren())
		{
			if (child is InventoryRow row)
			{
				Rows.Add(row);
			}
		}
	}
	
	public void AddItem(InventoryItem item)
	{
		if (IsEmpty)
			Rows.First(e => e.IsEmpty).Slots.First(e => e.IsEmpty).Item = item;
	}
}