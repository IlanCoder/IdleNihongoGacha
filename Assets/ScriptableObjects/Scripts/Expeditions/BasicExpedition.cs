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
			FOREST = Hero.ELEMENT.NATURE
		}
		#endregion

		#region VARS
		const uint PARTY_SIZE = 4;
		const uint TICK_TIME = 10;

		[Header("Expedition Status")]
		[ReadOnly, SerializeField] private bool onExpedition;
		[ReadOnly, SerializeField] private Hero[] party = new Hero[PARTY_SIZE];
		private TimeSpan explorationTime;

		[Header("Expedition Details")]
		[SerializeField, Min(1)] private uint damagePerTick;
		[SerializeField] private FIELD field;
		public FIELD Field { get { return field; } }
		#endregion

		#region PUBLIC_FUNCTIONS
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
				partyHP += hero.GetFinalHP();
			}
			return partyHP;
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
#endif
		#endregion
	}
}

