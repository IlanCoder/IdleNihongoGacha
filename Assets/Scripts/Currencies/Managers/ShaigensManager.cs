using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Currency.Managers {
    [DisallowMultipleComponent]
    public class ShaigensManager : MonoBehaviour {
        [Header("Shaigens")]
        [ReadOnly, SerializeField] uint _shaigensCount;
        public uint Shiagens { get { return _shaigensCount; } }

        private void AddShaigens(uint value) {
            _shaigensCount += value;
        }

        private bool RemoveShaigens(uint value) {
            if (_shaigensCount < value) return false;
            _shaigensCount -= value;
            return true;
        }
    }
}
