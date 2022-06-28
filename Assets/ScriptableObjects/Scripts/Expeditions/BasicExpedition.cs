using System;
using System.Collections;
using System.Collections.Generic;
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
		[ReadOnly, SerializeField] private bool onExpedition;
		public bool OnExpedition { get { return onExpedition; } }
		[ReadOnly, SerializeField] private Hero[] party = new Hero[PARTY_SIZE];
		private TimeSpan explorationTime;

		[Header("Expedition Details")]
		[SerializeField, Min(1)] private uint damagePerTick;
		[SerializeField] private FIELD field;
		public FIELD Field { get { return field; } }
		#endregion

		#region OBSERVERS
		public static event Action<TimeSpan> OnTimerChange;
		public static event Action<BasicExpedition> OnExpeditionSateChange;
		#endregion

		#region PUBLIC_FUNCTIONS
		public void ReduceExpeditionTime(TimeSpan timeElapsed) {
			if (!onExpedition) return;
			explorationTime-=timeElapsed;
			OnTimerChange?.Invoke(explorationTime);
			if (explorationTime.TotalSeconds <= 0) {
				FinishExpedition();
			}
		}

		public void StartExpedition() {
			if (onExpedition) return;
			if (IsPartyEmpty()) return;
			CalculateExpeditionTime();
			onExpedition = true;
			OnExpeditionSateChange?.Invoke(this);
		}

		public void AddHero(Hero newHero) {
			if (onExpedition) return;
			if (newHero == null) return;
			if (newHero.OnExpedition) return;
			for (int i = 0; i < PARTY_SIZE; i++) {
				if (party[i] != null) continue;
				party[i] = newHero;
				newHero.AddToExpedition();
				return;
			}
		}

		public void RemoveHero(Hero hero) {
			if (hero == null) return;
			for (int i = 0; i < PARTY_SIZE; i++) {
				if (party[i] == hero) {
					party[i] = null;
					return;
				}
			}
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		private void CalculateExpeditionTime() {
			uint partyHP = GetPartyHP();
			Debug.Log(partyHP);
			float ticksToSurvive = partyHP / (float)damagePerTick;
			explorationTime = TimeSpan.FromSeconds(ticksToSurvive * TICK_TIME);
			OnTimerChange?.Invoke(explorationTime);
		}

		private uint GetPartyHP() {
			uint partyHP = 0;
			foreach (Hero hero in party) {
				if (hero == null) continue;
				partyHP += GetBoostedStat(hero, hero.GetFinalHP());
			}
			return partyHP;
		}

		private uint GetBoostedStat(Hero hero, uint statToBoost) {
			if(hero.Element == Hero.ELEMENT.FRIENDSHIP) {
				return Convert.ToUInt32(statToBoost * (1 + FRIENDSHIP_BOOST));
			}
			if((int)hero.Element == (int)field) {
				return Convert.ToUInt32(statToBoost * (1 + ELEMENT_BOOST));
			}
			return Convert.ToUInt32(statToBoost * (ELEMENT_BOOST));
		}

		private bool IsPartyEmpty() {
			foreach(Hero hero in party) {
				if (hero != null) return false;
			}
			return true;
		}

		private void FinishExpedition() {
			if (!onExpedition) return;
			onExpedition = false;
			OnExpeditionSateChange?.Invoke(this);
		}
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("Empty Party")]
		public void EmptyParty() {
			for (int i = 0; i < PARTY_SIZE; i++) {
				RemoveHero(party[i]);
			}
		}

		[ContextMenu("Reset Expedition")]
		public void ResetExpedition() {
			onExpedition = false;
			EmptyParty();
		}
#endif
		#endregion
	}
}

