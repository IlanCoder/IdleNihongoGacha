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
	[ReadOnly, SerializeField] bool _onExpedition = false;
	public bool OnExpedition { get { return _onExpedition; } }

	[Header("Current Stats")]
	[ReadOnly, SerializeField] bool _unlocked = false;
	public bool Unlocked { get { return _unlocked; } }
	[ReadOnly, SerializeField] float _hp;
	public uint HP { get { return Convert.ToUInt32(Mathf.FloorToInt(_hp)); } }
	[ReadOnly, SerializeField] float _atk;
	public uint ATK { get { return Convert.ToUInt32(Mathf.FloorToInt(_atk)); } }
	[ReadOnly, SerializeField] float _affinity;
	public uint Affinity { get { return Convert.ToUInt32(Mathf.FloorToInt(_affinity)); } }
	[ReadOnly, SerializeField, Min(1)] uint _level;
	public uint Level { get { return Convert.ToUInt32(_level); } }
	[ReadOnly, SerializeField] uint _reincarnation;
	public uint Reincarnation { get { return Convert.ToUInt32(_reincarnation); } }

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
		float returnHP = _hp;
		switch (heroClass) {
			case CLASS.TANK: returnHP += _affinity;
				break;
			case CLASS.FIGHTER: returnHP += _affinity * 0.5f;
				break;
		}
		if (_reincarnation > 0) {
			returnHP *= _reincarnation * reincarnationStatPower;
		}
		return Convert.ToUInt32(Mathf.FloorToInt(returnHP));
	}

	public uint GetFinalATK() {
		float returnATK = _atk;
		switch (heroClass) {
			case CLASS.MAGE:
				returnATK += _affinity;
				break;
			case CLASS.FIGHTER:
				returnATK += _affinity * 0.5f;
				break;
		}
		returnATK *= _reincarnation * reincarnationStatPower;
		return Convert.ToUInt32(Mathf.FloorToInt(returnATK));
	}

	public void LevelUp() {
		if (!CanLevelUp()) return;
		_level++;
		_hp += incrementHp;
		_atk += incrementAtk;
		_affinity += incrementAffinity;
		OnLevelUp?.Invoke(this);
	}

	public void Unlock() {
		if (!_unlocked) _unlocked = true;
	}

	public void AddToExpedition() {
		_onExpedition = true;
	}

	public bool TryReincarnate() {
		if (!_unlocked) return false;
		if (_reincarnation >= 10) {
			OnTryReincarnate?.Invoke(rarity, false);
			return false;
		};
		_reincarnation++;
		OnTryReincarnate?.Invoke(rarity, true);
		return true;
	}
	#endregion

	#region PRIVATE_FUNCTIONS
	bool CanLevelUp() {
		if (!_unlocked) return false;
		switch (rarity) {
			case RARITY.COMMON: return _level < 1000 ? true : false;
			case RARITY.UNCOMMON: return _level < 750 ? true : false;
			case RARITY.RARE: return _level < 500 ? true : false;
			default: return true;
		}
	}
	#endregion

	#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
	[ContextMenu("Reset Current Stats")]
	public void ResetStats() {
		_level = 1;
		_hp = baseHP;
		_atk = baseATK;
		_affinity = baseAffinity;
		_reincarnation = 0;
		_unlocked = false;
	}

	[ContextMenu("Unlock")]
	public void EasyUnlock() {
		Unlock();
	}

	void Reset() {
		_level = 1;
	}
#endif
	#endregion
}