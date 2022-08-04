using System;
using UnityEngine;

namespace Gacha.Scriptable {
	[CreateAssetMenu(fileName = "New_Daily_Banner", menuName = "Gacha/Banners/Daily Banner", order = 3)]
	public class DailyBanner : GachaBanner {
		#region VARS
		[Header("Day of the Banner")]
		[SerializeField] DayOfWeek _bannerDay;
		public DayOfWeek Day { get { return _bannerDay; } }
		#endregion
	}
}
