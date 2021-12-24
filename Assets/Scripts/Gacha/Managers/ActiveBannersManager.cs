using System;
using System.Collections.Generic;
using UnityEngine;
using Gacha.Scriptable;

namespace Gacha.Managers {
	public class ActiveBannersManager : MonoBehaviour {
		[Header("Banners")]
		[SerializeField] GachaBanner defaultBanner;
		[SerializeField] HeroBanner heroBanner;
		[SerializeField, ReadOnly] DailyBanner dailyBanner;

		[Header("Daily Banners")]
		[SerializeField] List<DailyBanner> dailyBanners;

		public GachaBanner GetDefaultBanner() { return defaultBanner; }

		public HeroBanner GetHeroBanner() { return heroBanner; }

		public DailyBanner GetDailyBanner() { return dailyBanner; }

		private void Awake() {
			SetDailyBanner();
		}

		private void SetDailyBanner() {
			DayOfWeek today = DateTime.Today.DayOfWeek;
			foreach (DailyBanner banner in dailyBanners) {
				if (banner.Day == today) {
					dailyBanner = banner;
					return;
				}
			}
			dailyBanner = null;
		}
	}
}
