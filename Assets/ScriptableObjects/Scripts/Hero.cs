using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Hero", menuName = "Gacha/Hero", order = 1)]
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
		NATURE,
		FRIENDSHIP
	}
	public enum RARITY {
		COMMON = 0,
		UNCOMMON = 1,
		RARE = 2,
	}
	#endregion

	#region VARS
	[Header("Current Stats")]
	[SerializeField] bool unlocked = false;
	public bool Unlocked { get { return unlocked; } }
	[ReadOnly, SerializeField] float hp;
	public int HP { get { return Mathf.FloorToInt(hp); } }
	[ReadOnly, SerializeField] float atk;
	public int ATK { get { return Mathf.FloorToInt(atk); } }
	[ReadOnly, SerializeField] float affinity;
	public int Affinity { get { return Mathf.FloorToInt(affinity); } }
	[ReadOnly, SerializeField] int level;
	public int Level { get { return level; } }
	[ReadOnly, SerializeField] int reincarnation;
	public int Reincarnation { get { return reincarnation; } }

	[Header("Hero Specifics")]
	[SerializeField]CLASS heroClass;
	public CLASS HeroClass { get { return heroClass; } }
	[SerializeField] ELEMENT element;
	public ELEMENT Element { get { return element; } }
	[SerializeField] RARITY rarity;
	public RARITY Rarity { get { return rarity; } }

	[Header("Base Stats")]
	[SerializeField,Delayed] new string name;
	public string Name { get { return name; } }
	[Min(1), SerializeField, Delayed] int baseHP;
	[Min(1), SerializeField, Delayed] int baseATK;
	[Min(0), SerializeField, Delayed] int baseAffinity;
	[Min(0.01f), SerializeField, Delayed] float incrementHp;
	[Min(0.01f), SerializeField, Delayed] float incrementAtk;
	[Min(0.01f), SerializeField, Delayed] float incrementAffinity;
	[Range(0.01f,1), SerializeField, Delayed] float reincarnationStatPower;
	#endregion

	#region OBSERVERS
	public static event Action<Hero> OnLevelUp;
	#endregion

	#region PUBLIC_FUNCTIONS
	public int GetFinalHP() {
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

	public int GetFinalATK() {
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
		OnLevelUp?.Invoke(this);
	}

	public bool Unlock() {
		if (!unlocked) unlocked = true;
		return unlocked;
	}

	public bool Reincarnate() {
		if (!unlocked) return false;
		if (reincarnation >= 10) return false;
		reincarnation++;
		return true;
	}
	#endregion

	#region PRIVATE_FUNCTIONS
	bool CanLevelUp() {
		if (!unlocked) return false;
		switch (rarity) {
			case RARITY.COMMON: return level < 1000 ? true : false;
			case RARITY.UNCOMMON: return level < 750 ? true : false;
			case RARITY.RARE: return level < 500 ? true : false;
			default: return true;
		}
	}
	#endregion

	#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
	private void OnValidate() {
		ResetStats();
	}

	[ContextMenu("Reset Current Stats")]
	public void ResetStats() {
		level = 1;
		hp = baseHP;
		atk = baseATK;
		affinity = baseAffinity;
	}

	void Reset() {
		level = 1;
	}
#endif
	#endregion
}