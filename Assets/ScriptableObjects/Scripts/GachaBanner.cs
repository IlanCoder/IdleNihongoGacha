using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gacha {
	[CreateAssetMenu(fileName = "New_Banner", menuName = "Gacha/Default Banner", order = 1)]
	public class GachaBanner : ScriptableObject {
		#region VARS
		[Header("Banner Pool")]
		[SerializeField] List<Hero> pool = new List<Hero>();
		[SerializeField, Delayed] List<float> chances = new List<float>();

		[Header("Pity System")]
		[ReadOnly, SerializeField] int currentPityCount;
		[SerializeField] int pityCap;
		[SerializeField] Hero.RARITY pityMinRarity;

		Dictionary<Hero.RARITY, List<Hero>> rarityPool = new Dictionary<Hero.RARITY, List<Hero>>();
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
			List<Hero> tempList = rarityPool[rarity];
			int randHeroIndex = UnityEngine.Random.Range(0, tempList.Count);
			return tempList[randHeroIndex];
		}

		private Hero.RARITY GetPulledRarity(float pullRandomVal) {
			for (int i = 0; i < chances.Count; i++) {
				if (pullRandomVal <= chances[i]) {
					Hero.RARITY tempRarity = (Hero.RARITY)i;
					return CalculatePity(tempRarity);
				}
				pullRandomVal -= chances[i];
			}
			return Hero.RARITY.COMMON;
		}

		private Hero.RARITY CalculatePity(Hero.RARITY rarityToPull) {
			if (rarityToPull < pityMinRarity) {
				currentPityCount++;
				if (currentPityCount >= pityCap) {
					currentPityCount = 0;
					return pityMinRarity;
				}
				return rarityToPull;
			}
			currentPityCount = 0;
			return rarityToPull;
		}
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		[ContextMenu("Reset Pity")]
		public void ResetPity() {
			currentPityCount = 0;
		}

		private void OnValidate() {
			FillRarityPool();
			FillChancesList();
		}

		private void FillRarityPool() {
			rarityPool.Clear();
			foreach (Hero hero in pool) {
				if (rarityPool.TryGetValue(hero.Rarity, out List<Hero> rarityHeroPool)) {
					rarityHeroPool.Add(hero);
					continue;
				}
				List<Hero> tempList = new List<Hero>();
				tempList.Add(hero);
				rarityPool.Add(hero.Rarity, tempList);
			}
		}

		private void FillChancesList() {
			if (chances.Count == rarityPool.Count) {
				return;
			}
			if (chances.Count < rarityPool.Count) {
				for (int i = chances.Count; i < rarityPool.Count; i++) {
					chances.Add(new float());
				}
				return;
			}
			if (chances.Count > rarityPool.Count) {
				chances.RemoveRange(rarityPool.Count - 1, chances.Count - rarityPool.Count);
				return;
			}
		}
#endif
		#endregion
	}

}

