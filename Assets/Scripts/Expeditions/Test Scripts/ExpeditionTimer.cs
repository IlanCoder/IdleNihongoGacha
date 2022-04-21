using System;
using System.Collections.Generic;
using UnityEngine;
using Expedition.Scriptable;
using TMPro;

namespace Expedition.View {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class ExpeditionTimer : MonoBehaviour {
    private TextMeshProUGUI timerText;

    private List<TimeSpan> timers = new List<TimeSpan>() {
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
      timerText = this.GetComponent<TextMeshProUGUI>();
    }

    void UpdateTimer(TimeSpan timer) {
      if (timer <= timers[0]) {
        timerText.text = "Any time Now";
      }
      for(int i = 1; i < timers.Count; i++) {
        if (timer <= timers[i]) {
          timerText.text = "Aprox. " + timers[i].ToString();
          return;
        }
      }
      timerText.text = "Come Back Tomorrow";
    }
  }
}
