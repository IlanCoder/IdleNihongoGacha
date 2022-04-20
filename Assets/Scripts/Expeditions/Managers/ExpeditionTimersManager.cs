using System;
using System.Collections.Generic;
using UnityEngine;
using Expedition.Scriptable;

namespace Expedition.Managers {
  public class ExpeditionTimersManager : MonoBehaviour {
    [Header("Expedition List")]
    [SerializeField][ReadOnly]List<BasicExpedition> expeditionsInProgress = new List<BasicExpedition>();

    float currentSecDelta = 0;
    TimeSpan second = new TimeSpan(0, 0, 1);

    private void OnEnable() => BasicExpedition.OnExpeditionSateChange += ChangeInProgressList;

    private void Update() {
      if (!IsSecondOver()) return;
      SendASecondToExpeditions();
    }

    private bool IsSecondOver() {
      currentSecDelta += Time.deltaTime;
      if (currentSecDelta < 1) return false;
      currentSecDelta--;
      return true;
    }

    void SendASecondToExpeditions() {
      foreach(BasicExpedition expedition in expeditionsInProgress) {
        expedition.ReduceExpeditionTime(second);
      }
    }

    void ChangeInProgressList(BasicExpedition expedition) {
      if (expedition.OnExpedition) {
        expeditionsInProgress.Add(expedition);
        return;
      }
      expeditionsInProgress.Remove(expedition);
    }
  }
}
