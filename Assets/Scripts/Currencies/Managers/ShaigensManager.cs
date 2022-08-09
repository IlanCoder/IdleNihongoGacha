using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Currency.Managers {
    [DisallowMultipleComponent]
    public class ShaigensManager : MonoBehaviour {
        public uint Shaigens { get; private set; }

        private void AddShaigens(uint value) {
            Shaigens += value;
        }

        private bool RemoveShaigens(uint value) {
            if (Shaigens < value) return false;
            Shaigens -= value;
            return true;
        }
    }
}
