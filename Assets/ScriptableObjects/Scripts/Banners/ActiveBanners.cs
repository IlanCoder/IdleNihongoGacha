using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gacha {
	[CreateAssetMenu(fileName = "New_Banner_Manager", menuName = "Gacha/Managers/Banner Manager", order = 4)]
	public class ActiveBanners : ScriptableObject {
		#region VARS
		[Header("Banners")]
		[SerializeField] GachaBanner defaultBanner;
		[SerializeField] HeroBanner heroBanner;
		[SerializeField, ReadOnly] DailyBanner dailyBanner;

		[Header("Daily Banners")]
		[SerializeField] List<DailyBanner> dailyBanners;
		#endregion

		#region PUBLIC_FUNCTIONS
		public GachaBanner GetDefaultBanner() { return defaultBanner; }

		public HeroBanner GetHeroBanner() { return heroBanner; }

		public DailyBanner GetDailyBanner() { return dailyBanner; }
		#endregion

		#region PRIVATE_FUNCTIONS
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
		#endregion

		#region UNITY_EDITOR_FUNCTIONS
#if UNITY_EDITOR
		private void OnValidate() {
			SetDailyBanner();
		}
#endif
		#endregion
	}
}
