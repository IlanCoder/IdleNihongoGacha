using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gacha.Managers;
using Gacha.Scriptable;

namespace Gacha.View.Managers {
	public class BannersViewManager : MonoBehaviour {
		[Header("Required Managers")]
		public ActiveBannersManager activeBanners;

		[Header("Banner Screens")]
		[SerializeField] GameObject defaultBannerScreen;
		[SerializeField] GameObject heroBannerScreen;
		[SerializeField] GameObject dailyBannerScreen;

		private void OnEnable() {
			CheckBanners();
		}

		private void CheckBanners() {
			if (activeBanners == null) {
				Debug.LogError($"Please set an ActiveBanners Object in {this}");
				return;
			}
			ActivateScreenWithBanner(activeBanners.GetDefaultBanner(), defaultBannerScreen);
			ActivateScreenWithBanner(activeBanners.GetHeroBanner(), heroBannerScreen);
			ActivateScreenWithBanner(activeBanners.GetDailyBanner(), dailyBannerScreen);
		}

		private void ActivateScreenWithBanner(GachaBanner banner, GameObject screen) {
			if (banner != null) {
				screen.SetActive(true);
				CheckAndAssignBanner(banner, screen);
				return;
			}
			screen.SetActive(false);
		}

		private void CheckAndAssignBanner(GachaBanner banner, GameObject screen) {
			PullButton pullButton = screen.GetComponentInChildren<PullButton>();
			if(pullButton.banner == banner) {
				return;
			}
			pullButton.banner = banner;
		}
	}
}

