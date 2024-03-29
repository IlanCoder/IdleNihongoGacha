using System;
using Expeditions.Scriptable;
using UnityEngine;

namespace Currencies.Managers {
    [DisallowMultipleComponent]
    public class ShaigensManager : MonoBehaviour {
        public uint Shaigens { get; private set; }

        void OnEnable() => BasicExpedition.OnShaigensRewards += AddShaigens;
        void OnDisable() => BasicExpedition.OnShaigensRewards -= AddShaigens;

        void AddShaigens(uint value) {
            Shaigens += value;
        }

        bool GetShaigens(uint value) {
            if (Shaigens < value) return false;
            Shaigens -= value;
            return true;
        }
    }
}
