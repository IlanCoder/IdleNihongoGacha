using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Hero", menuName = "Hero", order = 1)]
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
	[Min(1), SerializeField, Delayed] float hp;
	[Min(1), SerializeField, Delayed] float atk;
	[Min(0), SerializeField, Delayed] float affinity;
	[Min(1), SerializeField, Delayed] int level;
	public int Level { get { return level; } }
	[Range(0, 10), Delayed] int reincarnation;
	public int Reincarnation { get { return reincarnation; } }

	[Header("Hero Specifics")]
	public CLASS heroClass;
	public ELEMENT element;
	public RARITY rarity;

	[Header("Level Up Increments")]
	[Min(0.01f), SerializeField, Delayed] float incrementHp;
	[Min(0.01f), SerializeField, Delayed] float incrementAtk;
	[Min(0.01f), SerializeField, Delayed] float incrementAffinity;
	[Min(0.01f), SerializeField, Delayed] float reincarnationStatPower;
	#endregion

	#region PUBLIC FUNCTIONS
	public int GetHP() {
		float returnHP = hp;
		switch (heroClass) {
			case CLASS.TANK: returnHP += affinity;
				break;
			case CLASS.FIGHTER: returnHP += affinity * 0.5f;
				break;
		}
		returnHP *= reincarnation * reincarnationStatPower;
		return Mathf.FloorToInt(returnHP);
	}

	public int GetATK() {
		float returnATK = atk;
		switch (heroClass) {
			case CLASS.MAGE:
				returnATK += affinity;
				break;
			case CLASS.FIGHTER:
				returnATK += affinity * 0.5f;
				break;
		}
		returnATK *= reincarnation * reincarnationStatPower;
		return Mathf.FloorToInt(returnATK);
	}

	public void LevelUp() {
		if (!CanLevelUp()) return;
		level++;
		hp += incrementHp;
		atk += incrementAtk;
		affinity += incrementAffinity;
	}

	public void Reincarnate() {
		reincarnation++;
	}
	#endregion

	#region PRIVATE FUNCTIONS
	bool CanLevelUp() {
		switch (rarity) {
			case RARITY.COMMON: return level < 1000 ? true : false;
			case RARITY.UNCOMMON: return level < 750 ? true : false;
			case RARITY.RARE: return level < 500 ? true : false;
			default: return true;
		}
	}
	#endregion
}
