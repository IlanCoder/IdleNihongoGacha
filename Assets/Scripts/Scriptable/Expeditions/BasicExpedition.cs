using System;
using UnityEngine;

namespace Expedition.Scriptable {
	[CreateAssetMenu(fileName = "New_Expedition", menuName = "Expedition/Basic Expedition", order = 1)]
	public class BasicExpedition : ScriptableObject {
		#region ENUMS
		public enum FIELD {
			PLAINS = Hero.ELEMENT.WIND,
			MOUNTAIN = Hero.ELEMENT.EARTH,
			FOREST = Hero.ELEMENT.NATURE,
		}
		#endregion

		#region VARS
		const uint PARTY_SIZE = 4;
		const uint TICK_TIME = 10;
		const float FRIENDSHIP_BOOST = .25f;
		const float ELEMENT_BOOST = .5f;

		[Header("Expedition Status")]
		[ReadOnly, SerializeField] bool _onExpedition;
		public bool OnExpedition { get { return _onExpedition; } }
		[ReadOnly, SerializeField] bool _canEarlyFinish;
		[ReadOnly, SerializeField] Hero[] _party = new Hero[PARTY_SIZE];
		TimeSpan _explorationTime;
		TimeSpan _initExplorationTime;
		TimeSpan _earlyFinishCheck = new TimeSpan(12,0,0);

		[Header("Expedition Details")]
		[SerializeField, Min(1)] uint _damagePerTick;
		[SerializeField] FIELD _field;
		public FIELD Field { get { return _field; } }
		#endregion

		#region OBSERVERS
		public static event Action<TimeSpan> OnTimerChange;
		public static event Action<BasicExpedition> OnExpeditionSateChange;
		public static event Action<bool> OnEarlyFinishChange;
		#endregion

		#region PUBLIC_FUNCTIONS
		public bool ReduceExpeditionTime(TimeSpan timeElapsed) {
            if (!_onExpedition) return false;
            _explorationTime -= timeElapsed;
            OnTimerChange?.Invoke(_explorationTime);
            CheckIfEarlyFinish();
            if (_explorationTime.TotalSeconds <= 0) {
                return true;
            }
            return false;
        }

        public void StartExpedition() {
			if (_onExpedition) return;
			if (IsPartyEmpty()) return;
			CalculateExpeditionTime();
			_onExpedition = true;
			OnExpeditionSateChange?.Invoke(this);
		}

		public void FinishEarly() {
			if (!_canEarlyFinish) return;
			FinishExpedition();
        }

		public void FinishExpedition() {
			if (!_onExpedition) return;
			_onExpedition = false;
			_canEarlyFinish = false;
			OnEarlyFinishChange?.Invoke(_canEarlyFinish);
			OnExpeditionSateChange?.Invoke(this);
		}

		public void AddHero(Hero newHero) {
			if (_onExpedition) return;
			if (newHero == null) return;
			if (newHero.OnExpedition) return;
			for (int i = 0; i < PARTY_SIZE; i++) {
				if (_party[i] != null) continue;
				_party[i] = newHero;
				newHero.AddToExpedition();
				return;
			}
		}

		public void RemoveHero(Hero hero) {
			if (hero == null) return;
			for (int i = 0; i < PARTY_SIZE; i++) {
				if (_party[i] == hero) {
					_party[i].RemoveFromExpedition();
					_party[i] = null;
					return;
				}
			}
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		void CalculateExpeditionTime() {
			uint partyHP = GetPartyHP();
			float ticksToSurvive = partyHP / (float)_damagePerTick;
			_explorationTime = TimeSpan.FromSeconds(ticksToSurvive * TICK_TIME);
			_initExplorationTime = _explorationTime;
			OnTimerChange?.Invoke(_explorationTime);
		}

		void CheckIfEarlyFinish() {
			TimeSpan _passedTime = _initExplorationTime - _explorationTime;
			if (_passedTime >= _earlyFinishCheck) {
				_canEarlyFinish = true;
				OnEarlyFinishChange?.Invoke(_canEarlyFinish);
			}
		}

		uint GetPartyHP() {
			uint partyHP = 0;
			foreach (Hero hero in _party) {
				if (hero == null) continue;
				partyHP += GetBoostedStat(hero, hero.GetFinalHP());
			}
			return partyHP;
		}

		uint GetBoostedStat(Hero hero, uint statToBoost) {
			if(hero.Element == Hero.ELEMENT.FRIENDSHIP) {
				return Convert.ToUInt32(statToBoost * (1 + FRIENDSHIP_BOOST));
			}
			if((int)hero.Element == (int)_field) {
				return Convert.ToUInt32(statToBoost * (1 + ELEMENT_BOOST));
			}
			return Convert.ToUInt32(statToBoost * (ELEMENT_BOOST));
		}

		bool IsPartyEmpty() {
			foreach(Hero hero in _party) {
				if (hero != null) return false;
			}
			return true;
		}
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("Empty Party")]
		public void EmptyParty() {
			for (int i = 0; i < PARTY_SIZE; i++) {
				RemoveHero(_party[i]);
			}
		}

		[ContextMenu("Reset Expedition")]
		public void ResetExpedition() {
			_onExpedition = false;
			EmptyParty();
		}
#endif
		#endregion
	}
}