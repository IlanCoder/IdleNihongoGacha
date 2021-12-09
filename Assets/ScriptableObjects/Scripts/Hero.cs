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
	public new string name;
	[Min(1)]
	public int hp;
	[Min(1)]
	public int atk;
	[Min(0)]
	public int affinity;
	[Min(1)]
	public int level;
	[Range(0, 10)]
	public int reincarnation;
	public CLASS heroClass;
	public ELEMENT element;
	public RARITY rarity;
	#endregion
}
