using Heroes.Scriptable;
using UnityEngine;

namespace Expeditions.Scriptable {
	[CreateAssetMenu(fileName = "New_Quest", menuName = "Expedition/Basic Quest", order = 1)]
	public class BasicQuest : BasicMission {
		#region ENUMS
		enum RequiredStat {
			Attack,
			Health
		}
		#endregion
		
		#region VARS
		[Header("Quest Details")]
		[SerializeField] RequiredStat _requiredStat;
		[SerializeField] uint _requiredStatValue;
		[SerializeField] Hero.Class _requieredClass;
		#endregion

		#region PUBLIC_FUNCTIONS
		public void StartQuest() {
			if (!CanStartMission()) return;
			if (!HasRequiredPower()) return;
			if (!HasRequiredClass()) return;
			if (!HasMinimumStat()) return;
			OnMission = true;
		}

		public void FinishQuest() {
			if (!OnMission) return;
			OnMission = false;
			CallOnShaigensRewards(_baseRewards);
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		bool HasRequiredClass() {
			for (var i = 0; i < PartySize; i++) {
				Hero hero = _party[i];
				if (hero == null) continue;
				if (hero.HeroClass == _requieredClass) return true;
			}
			return false;
		}
		
		bool HasRequiredPower() {
			uint power = GetPartyAtk() + GetPartyHp();
			return power >= _requiredPower;
		}

		bool HasMinimumStat() {
			for (var i = 0; i < PartySize; i++) {
				Hero hero = _party[i];
				if (hero == null) continue;
				switch (_requiredStat) {
					case RequiredStat.Attack:
						if (hero.GetFinalAtk() >= _requiredStatValue) return true;
						continue;
					case RequiredStat.Health:
						if (hero.GetFinalHp() >= _requiredStatValue) return true;
						continue;
					default:
						continue;
				}
			}
			return false;
		}
		#endregion
	}
}
