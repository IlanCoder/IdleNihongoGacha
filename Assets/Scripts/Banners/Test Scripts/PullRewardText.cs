using Banners.Scriptable;
using Heroes.Scriptable;
using TMPro;
using UnityEngine;

namespace Banners.Test_Scripts {
	public class PullRewardText : MonoBehaviour {
		TextMeshProUGUI _text;

		private void Awake() => _text = gameObject.GetComponent<TextMeshProUGUI>();

		private void OnEnable() => GachaBanner.OnBannerPull += UpdatePullRewardText;

		private void OnDisable() {
			GachaBanner.OnBannerPull -= UpdatePullRewardText;
			ResetText();
		}

		private void ResetText() {
			_text.text = string.Empty;
		}

		private void UpdatePullRewardText(Hero rewardHero) {
			_text.text = rewardHero.Name;
			_text.color = rewardHero.HeroRarity switch {
				Hero.Rarity.Common => Color.white,
				Hero.Rarity.Uncommon => Color.green,
				Hero.Rarity.Rare => Color.blue,
				_ => _text.color
			};
		}
	}
}
