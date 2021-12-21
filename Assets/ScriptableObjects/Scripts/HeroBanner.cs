using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Hero_Banner", menuName = "Gacha/Hero Banner", order = 1)]
public class HeroBanner : GachaBanner
{
	#region VARS
	[Header("Hero Pity System")]
	[ReadOnly, SerializeField] int currentHeroPityCount;
	[SerializeField] int heroPityCap;
	[SerializeField] Hero pityHero;
	#endregion

	#region PROTECTED_FUNCTIONS
	protected override Hero GetPulledHero(Hero.RARITY rarity) {
		Hero toPullHero = base.GetPulledHero(rarity);
		return CalculateHeroPity(toPullHero);
	}
	#endregion

	#region PRIVATE_FUNCTIONS
	private Hero CalculateHeroPity(Hero heroToPull) {
		if (heroToPull != pityHero) {
			currentHeroPityCount++;
			if (currentHeroPityCount >= heroPityCap) {
				currentHeroPityCount = 0;
				return pityHero;
			}
		}
		return heroToPull;
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
