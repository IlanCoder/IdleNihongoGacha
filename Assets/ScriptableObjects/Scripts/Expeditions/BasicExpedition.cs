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

		#region PUBLIC_FUNCTIONS
		public TimeSpan ReduceExpeditionTime(TimeSpan timeElapsed) {
			if (!onExpedition) return TimeSpan.Zero;
			explorationTime.Subtract(timeElapsed);
			if (explorationTime.TotalSeconds <= 0) {
				return TimeSpan.Zero;
			}
			return explorationTime;  
		}

		public void StartExpedition() {
			if (onExpedition) return;
			if (IsPartyEmpty()) return;
			CalculateExpeditionTime();
			onExpedition = true;
		}

		public void AddHero(Hero newHero) {
			if (onExpedition) return;
			if (newHero == null) return;
			for (int i = 0; i < PARTY_SIZE; i++) {
				if (party[i] != null) continue;
				party[i] = newHero;
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
			float ticksToSurvive = partyHP / (float)damagePerTick;
			explorationTime = TimeSpan.FromSeconds(ticksToSurvive * TICK_TIME);
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
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("Empty Party")]
		public void EmptyParty() {
			for (int i = 0; i < PARTY_SIZE; i++) {
				if(party[i] != null) {
					party[i] = null;
				}
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

