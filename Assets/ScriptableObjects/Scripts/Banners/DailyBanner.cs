using System;
using UnityEngine;

namespace Gacha {
	[CreateAssetMenu(fileName = "New_Daily_Banner", menuName = "Gacha/Banners/Daily Banner", order = 3)]
	public class DailyBanner : GachaBanner {
		#region VARS
		[Header("Day of the Banner")]
		[SerializeField] DayOfWeek bannerDay;
		public DayOfWeek Day { get { return bannerDay; } }
		#endregion
	}
}
