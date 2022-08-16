using System.Collections.Generic;
using Heroes.Scriptable;
using UnityEngine;

namespace Currencies.Managers {
    [DisallowMultipleComponent]
    public class TearsManager : MonoBehaviour {
        #region READONLY_DICTIONARIES
        private readonly IReadOnlyDictionary<Hero.Rarity, uint> _reincarnationRewards =
          new Dictionary<Hero.Rarity, uint>() {
            {Hero.Rarity.Common, 1 },
            {Hero.Rarity.Uncommon, 2 },
            {Hero.Rarity.Rare, 5 }
          };

        private readonly IReadOnlyDictionary<Hero.Rarity, uint> _maxReincarnationRewards =
          new Dictionary<Hero.Rarity, uint>() {
            {Hero.Rarity.Common, 5 },
            {Hero.Rarity.Uncommon, 10 },
            {Hero.Rarity.Rare, 15 }
          };
        #endregion
        
        public uint Tears { get; private set; }

        void OnEnable() => Hero.OnTryReincarnate += AddTearsFromRepeat;

        void OnDisable() => Hero.OnTryReincarnate -= AddTearsFromRepeat;

        #region LISTENERS
        void AddTearsFromRepeat(Hero.Rarity rarity, bool reincarnated) {
            if (reincarnated) {
                AddTearsFromDictionary(rarity, _reincarnationRewards);
                return;
            }
            AddTearsFromDictionary(rarity, _maxReincarnationRewards);
        }
        #endregion

        void AddTearsFromDictionary(Hero.Rarity rarity, IReadOnlyDictionary<Hero.Rarity, uint> tearsDictionary) {
            if (tearsDictionary.TryGetValue(rarity, out uint tearsToAdd)) {
                AddTears(tearsToAdd);
            }
        }

        void AddTears(uint value) {
            Tears += value;
        }
    }
}

