using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Gacha.Scriptable;

namespace Gacha.View {
	public class PullRewardText : MonoBehaviour {
		TextMeshProUGUI text;

		private void Awake() => text = gameObject.GetComponent<TextMeshProUGUI>();

		private void OnEnable() => GachaBanner.OnBannerPull += UpdatePullRewardText;

		private void OnDisable() {
			GachaBanner.OnBannerPull -= UpdatePullRewardText;
			ResetText();
		}

		private void ResetText() {
			text.text = string.Empty;
		}

		private void UpdatePullRewardText(Hero rewardHero) {
			text.text = rewardHero.Name;
			switch (rewardHero.Rarity) {
				case Hero.RARITY.COMMON:
					text.color = Color.white;
					break;
				case Hero.RARITY.UNCOMMON:
					text.color = Color.green;
					break;
				case Hero.RARITY.RARE:
					text.color = Color.blue;
					break;
			}
		}
	}
}
