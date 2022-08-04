using System;
using System.Collections.Generic;
using UnityEngine;
using Expedition.Scriptable;
using TMPro;

namespace Expedition.View {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ExpeditionTimer : MonoBehaviour {
        private TextMeshProUGUI _timerText;

        private List<TimeSpan> _timers = new List<TimeSpan>() {
            new TimeSpan(0,1,30),
            new TimeSpan(0,5,0),
            new TimeSpan(0,15,0),
            new TimeSpan(0,30,0),
            new TimeSpan(1,0,0),
            new TimeSpan(2,0,0),
            new TimeSpan(4,0,0),
            new TimeSpan(8,0,0),
            new TimeSpan(12,0,0),
            new TimeSpan(16,0,0),
        };

        private void OnEnable() => BasicExpedition.OnTimerChange += UpdateTimer;
        private void OnDisable() => BasicExpedition.OnTimerChange -= UpdateTimer;

        private void Start() {
            _timerText = this.GetComponent<TextMeshProUGUI>();
        }

        void UpdateTimer(TimeSpan timer) {
            if (timer <= _timers[0]) {
                _timerText.text = "Any time Now";
                return;
            }
            for (int i = 1; i < _timers.Count; i++) {
                if (timer <= _timers[i]) {
                    _timerText.text = "Aprox. " + _timers[i].ToString();
                    return;
                }
            }
            _timerText.text = "Come Back Tomorrow";
        }
    }
}
