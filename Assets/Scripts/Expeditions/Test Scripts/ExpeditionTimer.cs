using System;
using UnityEngine;
using Expedition.Scriptable;
using TMPro;

namespace Expedition.View {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class ExpeditionTimer : MonoBehaviour {
    private TextMeshProUGUI timerText;

    private void OnEnable() => BasicExpedition.OnTimerChange += UpdateTimer;
    private void OnDisable() => BasicExpedition.OnTimerChange -= UpdateTimer;

    private void Start() {
      timerText = this.GetComponent<TextMeshProUGUI>();
    }

    void UpdateTimer(TimeSpan timer) {
      timerText.text = timer.ToString();
    }
  }
}
