using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gacha {
	public class RewardManager : MonoBehaviour {
		private void OnEnable() => GachaBanner.OnBannerPull += ManageRewardHero;

		private void OnDisable() => GachaBanner.OnBannerPull -= ManageRewardHero;

		private void ManageRewardHero(Hero rewardHero) {
			if (!rewardHero.Unlocked) {
				rewardHero.Unlock();
				return;
			}
			if (rewardHero.Reincarnate()) return;
			//Tears system
		}
	}
}

