using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gacha.Scriptable {
	[CreateAssetMenu(fileName = "New_Banner", menuName = "Gacha/Banners/Default Banner", order = 1)]
	public class GachaBanner : ScriptableObject {
		#region VARS
		[Header("Banner Pool")]
		[SerializeField] List<Hero> _pool = new List<Hero>();
		[SerializeField, Delayed] List<float> _chances = new List<float>();

		[Header("Pity System")]
		[SerializeField] uint _currentPityCount;
		[SerializeField] uint _pityCap;
		[SerializeField] Hero.RARITY _pityMinRarity;

		Dictionary<Hero.RARITY, List<Hero>> _rarityPool = new Dictionary<Hero.RARITY, List<Hero>>();
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
		private Hero GetPulledHero(Hero.RARITY rarity) {
			List<Hero> tempList = _rarityPool[rarity];
			int randHeroIndex = UnityEngine.Random.Range(0, tempList.Count);
			return tempList[randHeroIndex];
		}

		private Hero.RARITY GetPulledRarity(float pullRandomVal) {
			for (int i = 0; i < _chances.Count; i++) {
				if (pullRandomVal <= _chances[i]) {
					Hero.RARITY tempRarity = (Hero.RARITY)i;
					return CalculatePity(tempRarity);
				}
				pullRandomVal -= _chances[i];
			}
			return Hero.RARITY.COMMON;
		}

		private Hero.RARITY CalculatePity(Hero.RARITY rarityToPull) {
			if (rarityToPull < _pityMinRarity) {
				_currentPityCount++;
				if (_currentPityCount >= _pityCap) {
					_currentPityCount = 0;
					return _pityMinRarity;
				}
				return rarityToPull;
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
				if (_rarityPool.TryGetValue(hero.Rarity, out List<Hero> rarityHeroPool)) {
					rarityHeroPool.Add(hero);
					continue;
				}
				List<Hero> tempList = new List<Hero>();
				tempList.Add(hero);
				_rarityPool.Add(hero.Rarity, tempList);
			}
		}

		private void FillChancesList() {
			if (_chances.Count == _rarityPool.Count) {
				return;
			}
			if (_chances.Count < _rarityPool.Count) {
				for (int i = _chances.Count; i < _rarityPool.Count; i++) {
					_chances.Add(new float());
				}
				return;
			}
			if (_chances.Count > _rarityPool.Count) {
				_chances.RemoveRange(_rarityPool.Count - 1, _chances.Count - _rarityPool.Count);
				return;
			}
		}
#endif
		#endregion
	}
}