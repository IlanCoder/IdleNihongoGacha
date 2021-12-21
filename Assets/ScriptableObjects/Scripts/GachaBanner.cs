using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Banner", menuName = "Gacha/Banner", order = 1)]
public class GachaBanner : ScriptableObject
{
	#region VARS
	[Header("Banner Pool")]
	[SerializeField] List<Hero> pool = new List<Hero>();
	[SerializeField, Delayed] List<float> chances = new List<float>();

	Dictionary<Hero.RARITY, List<Hero>> rarityPool = new Dictionary<Hero.RARITY, List<Hero>>();
	#endregion

	#region OBSERVERS
	public static event Action<Hero> OnBannerPull;
	#endregion

	#region PUBLIC_FUNCTIONS
	void PullBanner() {
		Hero pulledHero = GetPulledHero(GetPulledRarity());
		OnBannerPull?.Invoke(pulledHero);
	}
	#endregion

	#region PRIVATE_FUNCTIONS
	Hero.RARITY GetPulledRarity() {
		float randomVal = UnityEngine.Random.Range(0, 100f);
		for(int i =0; i < chances.Count; i++) {
			if (randomVal <= chances[i]) {
				return (Hero.RARITY)i;
			}
			randomVal -= chances[i];
		}
		return Hero.RARITY.COMMON;
	}

	Hero GetPulledHero(Hero.RARITY rarity) {
		List<Hero> tempList = rarityPool[rarity];
		int randHeroIndex = UnityEngine.Random.Range(0, tempList.Count);
		return tempList[randHeroIndex];
	}
	#endregion

	#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
	private void OnValidate() {
		FillRarityPool();
		FillChancesList();
	}

	private void FillRarityPool() {
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
		if(chances.Count < rarityPool.Count) {
			for(int i= chances.Count; i <rarityPool.Count; i++) {
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
