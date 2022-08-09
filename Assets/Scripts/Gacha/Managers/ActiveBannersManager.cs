using System;
using System.Collections.Generic;
using UnityEngine;
using Gacha.Scriptable;

namespace Gacha.Managers {
	[DisallowMultipleComponent]
	public class ActiveBannersManager : MonoBehaviour {
		[Header("Banners")]
		[SerializeField] GachaBanner _defaultBanner;
		[SerializeField] HeroBanner _heroBanner;
		[SerializeField] DailyBanner _dailyBanner;

		[Header("Daily Banners")]
		[SerializeField] List<DailyBanner> _dailyBanners;

		public GachaBanner GetDefaultBanner() { return _defaultBanner; }

		public HeroBanner GetHeroBanner() { return _heroBanner; }

		public DailyBanner GetDailyBanner() { return _dailyBanner; }

		private void Awake() {
			SetDailyBanner();
		}

		private void SetDailyBanner() {
			DayOfWeek today = DateTime.Today.DayOfWeek;
			foreach (DailyBanner banner in _dailyBanners) {
				if (banner.Day == today) {
					_dailyBanner = banner;
					return;
				}
			}
			_dailyBanner = null;
		}
	}
}
