using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Heroes.Scriptable {
	[CreateAssetMenu(fileName = "New_Hero", menuName = "Heroes/Hero", order = 1)]
	public class Hero : ScriptableObject {
		#region ENUMS
		public enum Class {
			Tank,
			Fighter,
			Mage
		}
		public enum Element {
			Earth,
			Wind,
			Nature,
			Friendship
		}
		public enum Rarity {
			Common = 0,
			Uncommon = 1,
			Rare = 2,
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
		[SerializeField] Class heroClass;
		public Class HeroClass { get { return heroClass; } }
		[SerializeField] Element heroElement;
		public Element HeroElement { get { return heroElement; } }
		[SerializeField] Rarity heroRarity;
		public Rarity HeroRarity { get { return heroRarity; } }

		[FormerlySerializedAs("name")]
		[Header("Base Stats")]
		[SerializeField, Delayed] string _name;
		public string Name { get { return _name; } }
		[FormerlySerializedAs("baseHp")]
		[Min(1), SerializeField, Delayed] uint _baseHp;
		[FormerlySerializedAs("baseAtk")]
		[Min(1), SerializeField, Delayed] uint _baseAtk;
		[FormerlySerializedAs("baseAffinity")]
		[Min(0), SerializeField, Delayed] uint _baseAffinity;
		[FormerlySerializedAs("incrementHp")]
		[Min(0.01f), SerializeField, Delayed] float _incrementHp;
		[FormerlySerializedAs("incrementAtk")]
		[Min(0.01f), SerializeField, Delayed] float _incrementAtk;
		[FormerlySerializedAs("incrementAffinity")]
		[Min(0.01f), SerializeField, Delayed] float _incrementAffinity;
		[FormerlySerializedAs("reincarnationStatPower")]
		[Range(0.01f, 1), SerializeField, Delayed] float _reincarnationStatPower;
		#endregion
		
		#region OBSERVERS
		public static event Action<Hero> OnLevelUp;
		public static event Action<Rarity, bool> OnTryReincarnate;
		#endregion

		#region PUBLIC_FUNCTIONS
		public uint GetFinalHp() {
			float returnHp = Hp;
			switch (heroClass) {
				case Class.Tank:
					returnHp += Affinity;
					break;
				case Class.Fighter:
					returnHp += Affinity * 0.5f;
					break;
				default: throw new ArgumentOutOfRangeException();
			}
			if (Reincarnation > 0) {
				returnHp *= Reincarnation * _reincarnationStatPower;
			}
			return Convert.ToUInt32(Mathf.FloorToInt(returnHp));
		}

		public uint GetFinalAtk() {
			float returnAtk = Atk;
			switch (heroClass) {
				case Class.Mage:
					returnAtk += Affinity;
					break;
				case Class.Fighter:
					returnAtk += Affinity * 0.5f;
					break;
				default: throw new ArgumentOutOfRangeException();
			}
			returnAtk *= Reincarnation * _reincarnationStatPower;
			return Convert.ToUInt32(Mathf.FloorToInt(returnAtk));
		}

		public void LevelUp() {
			if (!CanLevelUp()) return;
			Level++;
			Hp += _incrementHp;
			Atk += _incrementAtk;
			Affinity += _incrementAffinity;
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

		public void TryReincarnate() {
			if (!Unlocked) return;
			if (Reincarnation >= 10) {
				OnTryReincarnate?.Invoke(heroRarity, false);
				return;
			}
			;
			Reincarnation++;
			OnTryReincarnate?.Invoke(heroRarity, true);
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		bool CanLevelUp() {
			if (!Unlocked) return false;
			switch (heroRarity) {
				case Rarity.Common: return Level < 1000 ? true : false;
				case Rarity.Uncommon: return Level < 750 ? true : false;
				case Rarity.Rare: return Level < 500 ? true : false;
				default: return true;
			}
		}
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("Reset Current Stats")]
		public void ResetStats() {
			Level = 1;
			Hp = _baseHp;
			Atk = _baseAtk;
			Affinity = _baseAffinity;
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
}