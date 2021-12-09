using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Hero", order = 1)]
public class Hero : ScriptableObject {
	#region ENUMS
	public enum CLASS {
		TANK,
		FIGHTER,
		MAGE
	}
	public enum ELEMENT {
		EARTH,
		WIND,
		NATURE
	}
	public enum RARITY {
		COMMON,
		UNCOMMON,
		RARE
	}
	#endregion

	#region VARS
	[Header("Stats")]
	[Delayed] public new string name;
	[Min(1), SerializeField, Delayed] int hp;
	[Min(1), SerializeField, Delayed] int atk;
	[Min(1), SerializeField, Delayed] int affinity;

	[Header("Level")]
	[Min(1), Delayed] public int level;
	[Range(0, 10), Delayed] public int reincarnation;

	[Header("Hero Specifics")]
	public CLASS heroClass;
	public ELEMENT element;
	public RARITY rarity;
#endregion

#region PUBLIC FUNCTIONS
	public int GetHP() {
		switch (heroClass) {
			case CLASS.TANK: return hp + affinity;
			case CLASS.FIGHTER: return hp + (int)(affinity * 0.5);
			default: return hp;
		}
	}

	public int GetATK() {
		switch (heroClass) {
			case CLASS.FIGHTER: return atk + (int)(affinity * 0.5);
			case CLASS.MAGE: return atk + affinity;
			default: return atk;
		}
	}
#endregion
}
