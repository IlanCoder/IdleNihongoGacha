using System;
using System.Collections.Generic;
using Heroes.Scriptable;
using UnityEngine;

namespace Banners.Scriptable {
	[CreateAssetMenu(fileName = "New_Banner", menuName = "Gacha/Banners/Default Banner", order = 1)]
	public class GachaBanner : ScriptableObject {
		#region VARS
		[Header("Banner Pool")]
		[SerializeField] List<Hero> _pool = new List<Hero>();
		[SerializeField, Delayed] List<float> _chances = new List<float>();

		[Header("Pity System")]
		[SerializeField] uint _currentPityCount;
		[SerializeField] uint _pityCap;
		[SerializeField] Hero.Rarity _pityMinRarity;

		readonly Dictionary<Hero.Rarity, List<Hero>> _rarityPool = new Dictionary<Hero.Rarity, List<Hero>>();
		#endregion

		#region OBSERVERS
		public static event Action<Hero> OnBannerPull;
		#endregion

		#region PUBLIC_FUNCTIONS
		public void PullBanner() {
			Hero pulledHero = PullHero(UnityEngine.Random.Range(0, 100f));
			OnBannerPull?.Invoke(pulledHero);
		}
		#endregion

		#region PROTECTED_FUNCTIONS
		protected virtual Hero PullHero(float randomPullVal) {
			return GetPulledHero(GetPulledRarity(randomPullVal));
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		private Hero GetPulledHero(Hero.Rarity rarity) {
			List<Hero> tempList = _rarityPool[rarity];
			int randHeroIndex = UnityEngine.Random.Range(0, tempList.Count);
			return tempList[randHeroIndex];
		}

		private Hero.Rarity GetPulledRarity(float pullRandomVal) {
			for (var i = 0; i < _chances.Count; i++) {
				if (pullRandomVal <= _chances[i]) {
					var tempRarity = (Hero.Rarity)i;
					return CalculatePity(tempRarity);
				}
				pullRandomVal -= _chances[i];
			}
			return Hero.Rarity.Common;
		}

		private Hero.Rarity CalculatePity(Hero.Rarity rarityToPull) {
			if (rarityToPull < _pityMinRarity) {
				_currentPityCount++;
				if (_currentPityCount < _pityCap) return rarityToPull;
				_currentPityCount = 0;
				return _pityMinRarity;
			}
			_currentPityCount = 0;
			return rarityToPull;
		}
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("Reset Pity")]
		public void ResetPity() {
			_currentPityCount = 0;
		}

		private void OnValidate() {
			FillRarityPool();
			FillChancesList();
		}

		private void FillRarityPool() {
			_rarityPool.Clear();
			foreach (Hero hero in _pool) {
				if (_rarityPool.TryGetValue(hero.HeroRarity, out List<Hero> rarityHeroPool)) {
					rarityHeroPool.Add(hero);
					continue;
				}
				var tempList = new List<Hero> { hero };
				_rarityPool.Add(hero.HeroRarity, tempList);
			}
		} 

		private void FillChancesList() {
			if (_chances.Count == _rarityPool.Count) return;
			if (_chances.Count < _rarityPool.Count) {
				for (int i = _chances.Count; i < _rarityPool.Count; i++) {
					_chances.Add(new float());
				}
				return;
			}
			if (_chances.Count <= _rarityPool.Count) return;
			_chances.RemoveRange(_rarityPool.Count - 1, _chances.Count - _rarityPool.Count);
		}
#endif
		#endregion
	}
}