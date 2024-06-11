using System.Collections.Generic;
using LandsOfAzerith.scripts.character.mob;
using LandsOfAzerith.scripts.inventory;
using LandsOfAzerith.scripts.item;
using LandsOfAzerith.scripts.item.weapon;

namespace LandsOfAzerith.scripts.character;

// This class exists only for better organization of the code.
public partial class PlayerBackend : Character
{
    public Statistics Statistics { get; set; }
    public Inventory Inventory => GetNode<Inventory>("Inventory");
    public override uint Strength { get; set; }
    // Technically doesn't work, here to avoid warnings.
    public override Weapon Weapon { get; set; } = new MeleeWeapon();
    public override uint HealthPoints { get; protected set; }
    public override uint MaxHealthPoints => 100;
    public override uint Speed { get; set; }
    protected override Character? Aggro { get; set; }
    public override void Die()
    {
        throw new System.NotImplementedException();
    }
    protected readonly List<Mob> InRangeMobs = new List<Mob>();
}