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
	public bool OnExpedition { get; private set; } = false;
	public bool Unlocked { get; private set; } = false;
	public float Hp { get; private set; }
	public float Atk { get; private set; }
	public float Affinity { get; private set; }
	public uint Level { get; private set; }
	public uint Reincarnation { get; private set; }

	[Header("Hero Specifics")]
	[SerializeField]CLASS heroClass;
	public CLASS HeroClass { get { return heroClass; } }
	[SerializeField] ELEMENT element;
	public ELEMENT Element { get { return element; } }
	[SerializeField] RARITY rarity;
	public RARITY Rarity { get { return rarity; } }

	[Header("Base Stats")]
	[SerializeField, Delayed] new string name;
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
		float returnHP = Hp;
		switch (heroClass) {
			case CLASS.TANK: returnHP += Affinity;
				break;
			case CLASS.FIGHTER: returnHP += Affinity * 0.5f;
				break;
		}
		if (Reincarnation > 0) {
			returnHP *= Reincarnation * reincarnationStatPower;
		}
		return Convert.ToUInt32(Mathf.FloorToInt(returnHP));
	}

	public uint GetFinalATK() {
		float returnATK = Atk;
		switch (heroClass) {
			case CLASS.MAGE:
				returnATK += Affinity;
				break;
			case CLASS.FIGHTER:
				returnATK += Affinity * 0.5f;
				break;
		}
		returnATK *= Reincarnation * reincarnationStatPower;
		return Convert.ToUInt32(Mathf.FloorToInt(returnATK));
	}

	public void LevelUp() {
		if (!CanLevelUp()) return;
		Level++;
		Hp += incrementHp;
		Atk += incrementAtk;
		Affinity += incrementAffinity;
		OnLevelUp?.Invoke(this);
	}

	public void Unlock() {
		if (!Unlocked) Unlocked = true;
	}

	public void AddToExpedition() {
		OnExpedition = true;
	}

	public void RemoveFromExpedition() {
		OnExpedition = false;
	}

	public bool TryReincarnate() {
		if (!Unlocked) return false;
		if (Reincarnation >= 10) {
			OnTryReincarnate?.Invoke(rarity, false);
			return false;
		};
		Reincarnation++;
		OnTryReincarnate?.Invoke(rarity, true);
		return true;
	}
	#endregion

	#region PRIVATE_FUNCTIONS
	bool CanLevelUp() {
		if (!Unlocked) return false;
		switch (rarity) {
			case RARITY.COMMON: return Level < 1000 ? true : false;
			case RARITY.UNCOMMON: return Level < 750 ? true : false;
			case RARITY.RARE: return Level < 500 ? true : false;
			default: return true;
		}
	}
	#endregion

	#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
	[ContextMenu("Reset Current Stats")]
	public void ResetStats() {
		Level = 1;
		Hp = baseHP;
		Atk = baseATK;
		Affinity = baseAffinity;
		Reincarnation = 0;
		Unlocked = false;
	}

	[ContextMenu("Unlock")]
	public void EasyUnlock() {
		Unlock();
	}

	void Reset() {
		Level = 1;
	}
#endif
	#endregion
}