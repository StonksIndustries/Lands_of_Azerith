using System.Collections.Generic;
using Godot;

namespace LandsOfAzerith.scripts.inventory;

public partial class Inventory : Control
{
	private List<InventoryRow> _rows = new List<InventoryRow>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeRows();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void InitializeRows()
	{
		foreach (Node child in GetChildren())
		{
			if (child is InventoryRow row)
			{
				_rows.Add(row);
			}
		}
	}

	public Godot.Collections.Dictionary<string, Variant> Save()
	{
		var result = new Godot.Collections.Dictionary<string, Variant>()
		{
			{ nameof(Name), Name }
		};
		foreach (var row in _rows)
		{
			result.Add(row.Name, row.Save());
		}
		return result;
	}
}