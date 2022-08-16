using Banners.Scriptable;
using Heroes.Scriptable;
using UnityEngine;

namespace Banners.Managers {
	[DisallowMultipleComponent]
	public class RewardHeroManager : MonoBehaviour {
		void OnEnable() => GachaBanner.OnBannerPull += ManageRewardHero;

		void OnDisable() => GachaBanner.OnBannerPull -= ManageRewardHero;

		#region LISTENERS
		void ManageRewardHero(Hero rewardHero) {
			if (!rewardHero.Unlocked) {
				rewardHero.Unlock();
				return;
			}
			rewardHero.TryReincarnate();
		}
		#endregion
	}
}

