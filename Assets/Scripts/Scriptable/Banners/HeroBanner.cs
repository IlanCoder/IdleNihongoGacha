using UnityEngine;

namespace Gacha.Scriptable {
	[CreateAssetMenu(fileName = "New_Hero_Banner", menuName = "Gacha/Banners/Hero Banner", order = 2)]
	public class HeroBanner : GachaBanner {
		#region VARS
		[Header("Hero Spawn Chance")]
		[Delayed, SerializeField] float _heroChance;

		[Header("Hero Pity System")]
		[SerializeField] uint _currentHeroPityCount;
		[SerializeField] uint _heroPityCap;
		[SerializeField] Hero _pityHero;
		#endregion

		#region PROTECTED_FUNCTIONS
		protected override Hero PullHero(float randomPullVal) {
			if (GetBannerHero(randomPullVal)) {
				return _pityHero;
			}
			randomPullVal -= _heroChance;
			return base.PullHero(randomPullVal);
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		private bool GetBannerHero(float randomPullVal) {
			if (randomPullVal <= _heroChance) {
				return true;
			}
			_currentHeroPityCount++;
			if (_currentHeroPityCount >= _heroPityCap) {
				_currentHeroPityCount = 0;
				return true;
			}
			return false;
		}
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("ResetHeroPity")]
		public void ResetHeroPity() {
			_currentHeroPityCount = 0;
		}
#endif
		#endregion
	}
}
