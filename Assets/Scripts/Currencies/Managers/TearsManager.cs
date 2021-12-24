using System.Collections.Generic;
using UnityEngine;

namespace Currency.Managers {
  public class TearsManager : MonoBehaviour {
    #region READONLY_DICTIONARIES
    private readonly IReadOnlyDictionary<Hero.RARITY, int> reincarnationRewards =
      new Dictionary<Hero.RARITY, int>() {
        {Hero.RARITY.COMMON, 1 },
        {Hero.RARITY.UNCOMMON, 2 },
        {Hero.RARITY.RARE, 5 }
    };

    private readonly IReadOnlyDictionary<Hero.RARITY, int> maxReincarnationRewards = 
      new Dictionary<Hero.RARITY, int>() {
        {Hero.RARITY.COMMON, 5 },
        {Hero.RARITY.UNCOMMON, 10 },
        {Hero.RARITY.RARE, 15 }
    };
		#endregion

		[Header("Tears")]
    [ReadOnly, SerializeField]private int tearsCount;
    public int Tears { get { return tearsCount; } }

    private void OnEnable() => Hero.OnTryReincarnate += AddTearsFromRepeat;

    private void OnDisable() => Hero.OnTryReincarnate -= AddTearsFromRepeat;

		#region LISTENERS
		private void AddTearsFromRepeat(Hero.RARITY rarity, bool reincarnated) {
			if (reincarnated) {
        AddTearsFromDictionary(rarity, reincarnationRewards);
        return;
			}
      AddTearsFromDictionary(rarity, maxReincarnationRewards);
    }
		#endregion

    private void AddTearsFromDictionary(Hero.RARITY rarity, IReadOnlyDictionary<Hero.RARITY, int> tearsDictionary) {
      if (tearsDictionary.TryGetValue(rarity, out int tearsToAdd)) {
        AddTears(tearsToAdd);
      }
    }

		private void AddTears(int value) {
      tearsCount += value;
		}  
  }
}

