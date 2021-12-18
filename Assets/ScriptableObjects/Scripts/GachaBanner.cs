using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Banner", menuName = "Gacha/Banner", order = 1)]
public class GachaBanner : ScriptableObject
{
	#region VARS
	[Header("Banner General Pool")]
	[SerializeField] List<Hero> pool = new List<Hero>();
	[SerializeField, Delayed] List<float> chances = new List<float>();

	Dictionary<Hero.RARITY, List<Hero>> rarityPool = new Dictionary<Hero.RARITY, List<Hero>>();
	#endregion

	#region PUBLIC_FUNCTIONS
	#endregion

	#region PRIVATE_FUNCTIONS
	#endregion

	#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
	private void OnValidate() {
		FillRarityPool();
		FillChancesList();
		RecalculateChances();
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

	private void RecalculateChances() {
		if (chances.Count <= 0) {
			return;
		}
		if (chances.Count <= 1) {
			chances[0] = 100;
			return;
		}
		if (chances[0] >= 100) {
			chances[0] = 99;
		}
		float remainingChances = 100 - chances[0];
		for(int i=1; i < chances.Count - 1; i++) {
			float prevChance = chances[i - 1];
			if (chances[i] >= prevChance) {
				chances[i] = prevChance - 0.1f;
				while (chances[i] >= remainingChances) {
					chances[i] -= remainingChances/10;
				}
			}
			float nextChance = chances[i + 1];
			if(chances[i] <= nextChance) {
				chances[i] = nextChance + 0.1f;
			}
			if (chances[i] <= 0) {
				chances[i] = 0.1f;
				while (chances[i] >= remainingChances) {
					chances[i] -= remainingChances / 100;
				}
			}
			remainingChances -= chances[i];
		}
		chances[chances.Count - 1] = remainingChances;
		if(chances[chances.Count - 1] == chances[chances.Count - 2]) {
			chances[chances.Count - 1] -= 0.001f;
			chances[chances.Count - 2] += 0.001f;
		}
	} //CPU Intensive
#endif
	#endregion
}
