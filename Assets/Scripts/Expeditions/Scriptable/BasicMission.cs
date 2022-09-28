using System;
using UnityEngine;
using Heroes.Scriptable;

namespace Expeditions.Scriptable {
	public class BasicMission : ScriptableObject {
		#region ENUMS
		protected enum Field {
			Plains = Hero.Element.Wind,
			Mountain = Hero.Element.Earth,
			Forest = Hero.Element.Nature,
		}
		#endregion

		#region VARS
		const float FriendshipBoost = .25f;
		const float ElementBoost = .5f;
		protected const uint PartySize = 4;
		
		protected readonly Hero[] _party = new Hero[PartySize];
		protected TimeSpan _explorationTime;

		public bool OnMission { get; protected set; }
		
		[Header("Mission Details")]
		[SerializeField] protected uint _baseRewards;
		[SerializeField] protected uint _requiredPower;
		[SerializeField] protected Field _field;
		#endregion
		
		#region OBSERVERS
		public static event Action<TimeSpan> OnTimerChange;
        #endregion

		#region PUBLIC_FUNCTIONS
		public virtual bool ReduceExpeditionTime(TimeSpan timeElapsed) {
			if (!OnMission) return false;
			_explorationTime -= timeElapsed;
			return _explorationTime.TotalSeconds <= 0;
		}
		
		public void AddHero(Hero newHero) {
			if (OnMission) return;
			if (newHero == null) return;
			if (newHero.OnExpedition) return;
			for (var i = 0; i < PartySize; i++) {
				if (_party[i] != null) continue;
				_party[i] = newHero;
				newHero.AddToExpedition();
				return;
			}
		}
		
		public void RemoveHero(Hero hero) {
			if (hero == null) return;
			for (var i = 0; i < PartySize; i++) {
				if (_party[i] != hero) continue;
				_party[i].RemoveFromExpedition();
				_party[i] = null;
				return;
			}
		}
		#endregion

		#region PROTECTED_FUNCTIONS
		protected void CallOnTimerChange() {
			OnTimerChange?.Invoke(_explorationTime);
		}

		protected bool CanStartMission() {
			return !IsPartyEmpty() && !OnMission;
		}
		
		protected uint GetPartyHp() {
			uint partyHp = 0;
			foreach (Hero hero in _party) {
				if (hero == null) continue;
				partyHp += GetBoostedStat(hero, hero.GetFinalHp());
			}
			return partyHp;
		}
        
		protected uint GetPartyAtk() {
			uint partyAtk = 0;
			foreach (Hero hero in _party) {
				if(hero == null)continue;
				partyAtk = GetBoostedStat(hero, hero.GetFinalAtk());
			}
			return partyAtk;
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		bool IsPartyEmpty() {
			foreach (Hero hero in _party) {
				if (hero != null) return false;
			}
			return true;
		}
		
		uint GetBoostedStat(Hero hero, uint statToBoost) {
			if (hero.HeroElement == Hero.Element.Friendship) {
				return Convert.ToUInt32(statToBoost * (1 + FriendshipBoost));
			}
			if ((int)hero.HeroElement == (int)_field) {
				return Convert.ToUInt32(statToBoost * (1 + ElementBoost));
			}
			return Convert.ToUInt32(statToBoost * ElementBoost);
		}
		#endregion
		
		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("Empty Party")]
		public void EmptyParty() {
			for (int i = 0; i < PartySize; i++) {
				RemoveHero(_party[i]);
			}
		}

		[ContextMenu("Reset Expedition")]
		public void ResetExpedition() {
			OnMission = false;
			EmptyParty();
		}
#endif
        #endregion
	}
}
