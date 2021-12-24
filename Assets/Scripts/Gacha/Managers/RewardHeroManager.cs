using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gacha.Scriptable;

namespace Gacha.Managers {
	public class RewardHeroManager : MonoBehaviour {
		private void OnEnable() => GachaBanner.OnBannerPull += ManageRewardHero;

		private void OnDisable() => GachaBanner.OnBannerPull -= ManageRewardHero;

		#region LISTENERS
		private void ManageRewardHero(Hero rewardHero) {
			if (!rewardHero.Unlocked) {
				rewardHero.Unlock();
				return;
			}
			rewardHero.TryReincarnate();
		}
		#endregion
	}
}

