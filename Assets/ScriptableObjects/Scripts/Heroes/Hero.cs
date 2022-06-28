using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Hero", menuName = "Heroes/Hero", order = 1)]
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
	[Header("Ingame Status")]
	[ReadOnly, SerializeField] bool onExpedition = false;
	public bool OnExpedition { get { return onExpedition; } }

	[Header("Current Stats")]
	[ReadOnly, SerializeField] bool unlocked = false;
	public bool Unlocked { get { return unlocked; } }
	[ReadOnly, SerializeField] float hp;
	public uint HP { get { return Convert.ToUInt32(Mathf.FloorToInt(hp)); } }
	[ReadOnly, SerializeField] float atk;
	public uint ATK { get { return Convert.ToUInt32(Mathf.FloorToInt(atk)); } }
	[ReadOnly, SerializeField] float affinity;
	public uint Affinity { get { return Convert.ToUInt32(Mathf.FloorToInt(affinity)); } }
	[ReadOnly, SerializeField, Min(1)] uint level;
	public uint Level { get { return Convert.ToUInt32(level); } }
	[ReadOnly, SerializeField] uint reincarnation;
	public uint Reincarnation { get { return Convert.ToUInt32(reincarnation); } }

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
	[Min(1), SerializeField, Delayed] uint baseHP;
	[Min(1), SerializeField, Delayed] uint baseATK;
	[Min(0), SerializeField, Delayed] uint baseAffinity;
	[Min(0.01f), SerializeField, Delayed] float incrementHp;
	[Min(0.01f), SerializeField, Delayed] float incrementAtk;
	[Min(0.01f), SerializeField, Delayed] float incrementAffinity;
	[Range(0.01f,1), SerializeField, Delayed] float reincarnationStatPower;
	#endregion

	#region OBSERVERS
	public static event Action<Hero> OnLevelUp;

	public static event Action<RARITY, bool> OnTryReincarnate;
	#endregion

	#region PUBLIC_FUNCTIONS
	public uint GetFinalHP() {
		float returnHP = hp;
		switch (heroClass) {
			case CLASS.TANK: returnHP += affinity;
				break;
			case CLASS.FIGHTER: returnHP += affinity * 0.5f;
				break;
		}
		if (reincarnation > 0) {
			returnHP *= reincarnation * reincarnationStatPower;
		}
		return Convert.ToUInt32(Mathf.FloorToInt(returnHP));
	}

	public uint GetFinalATK() {
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
		return Convert.ToUInt32(Mathf.FloorToInt(returnATK));
	}

	public void LevelUp() {
		if (!CanLevelUp()) return;
		level++;
		hp += incrementHp;
		atk += incrementAtk;
		affinity += incrementAffinity;
		OnLevelUp?.Invoke(this);
	}

	public void Unlock() {
		if (!unlocked) unlocked = true;
	}

	public void AddToExpedition() {
		onExpedition = true;
	}

	public bool TryReincarnate() {
		if (!unlocked) return false;
		if (reincarnation >= 10) {
			OnTryReincarnate?.Invoke(rarity, false);
			return false;
		};
		reincarnation++;
		OnTryReincarnate?.Invoke(rarity, true);
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
	[ContextMenu("Reset Current Stats")]
	public void ResetStats() {
		level = 1;
		hp = baseHP;
		atk = baseATK;
		affinity = baseAffinity;
		reincarnation = 0;
		unlocked = false;
	}

	void Reset() {
		level = 1;
	}
#endif
	#endregion
}