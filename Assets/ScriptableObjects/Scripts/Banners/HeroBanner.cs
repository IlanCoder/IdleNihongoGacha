using UnityEngine;

namespace Gacha.Scriptable {
	[CreateAssetMenu(fileName = "New_Hero_Banner", menuName = "Gacha/Banners/Hero Banner", order = 2)]
	public class HeroBanner : GachaBanner {
		#region VARS
		[Header("Hero Spawn Chance")]
		[Delayed, SerializeField] float heroChance;

		[Header("Hero Pity System")]
		[ReadOnly, SerializeField] uint currentHeroPityCount;
		[SerializeField] uint heroPityCap;
		[SerializeField] Hero pityHero;
		#endregion

		#region PROTECTED_FUNCTIONS
		protected override Hero PullHero(float randomPullVal) {
			if (GetBannerHero(randomPullVal)) {
				return pityHero;
			}
			randomPullVal -= heroChance;
			return base.PullHero(randomPullVal);
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		private bool GetBannerHero(float randomPullVal) {
			if (randomPullVal <= heroChance) {
				return true;
			}
			currentHeroPityCount++;
			if (currentHeroPityCount >= heroPityCap) {
				currentHeroPityCount = 0;
				return true;
			}
			return false;
		}
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("ResetHeroPity")]
		public void ResetHeroPity() {
			currentHeroPityCount = 0;
		}
#endif
		#endregion
	}
}
