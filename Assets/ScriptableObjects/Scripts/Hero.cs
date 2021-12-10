using System;
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

	#region OBSERVERS
	public static event Action<Hero> OnLevepUp;
	#endregion

	#region PUBLIC_FUNCTIONS
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
		OnLevepUp?.Invoke(this);
	}

	public void Reincarnate() {
		reincarnation++;
	}
	#endregion

	#region PRIVATE_FUNCTIONS
	bool CanLevelUp() {
		switch (rarity) {
			case RARITY.COMMON: return level < 1000 ? true : false;
			case RARITY.UNCOMMON: return level < 750 ? true : false;
			case RARITY.RARE: return level < 500 ? true : false;
			default: return true;
		}
	}
	#endregion

	#region UNITY_EDITOR_VARS
#if UNITY_EDITOR
	static string rName;
	static float rHP;
	static float rATK;
	static float rAffinity;

	static CLASS rClass;
	static ELEMENT rElement;
	static RARITY rRarity;

	static float rIncrementHp;
	static float rIncrementAtk;
	static float rIncrementAffinity;
	static float rReincarnationStatPower;
#endif
	#endregion

	#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
	private void SaveResetData() {
		rName = name;
		rHP = hp;
		rATK = atk;
		rAffinity = affinity;

		rClass = heroClass;
		rElement = element;
		rRarity = rarity;

		rIncrementHp = incrementHp;
		rIncrementAtk = incrementAtk;
		rIncrementAffinity = incrementAffinity;
		rReincarnationStatPower = reincarnationStatPower;
	}

	private void LoadResetData() {
		name = rName;
		hp = rHP;
		atk = rATK;
		affinity = rAffinity;
		level = 1;

		heroClass = rClass;
		element = rElement;
		rarity = rRarity;

		incrementHp = rIncrementHp;
		incrementAtk = rIncrementAtk;
		incrementAffinity = rIncrementAffinity;
		reincarnationStatPower = rReincarnationStatPower;
	}

	private void OnValidate() {
		SaveResetData();
	}

	private void Reset() {
		LoadResetData();
	}
#endif
	#endregion
}
