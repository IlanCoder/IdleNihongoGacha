using System;
using System.Collections.Generic;
using UnityEngine;
using Expedition.Scriptable;

namespace Expedition.Managers {
    [DisallowMultipleComponent]
    public class ExpeditionTimersManager : MonoBehaviour {
        [Header("Expedition List")]
        [SerializeField] [ReadOnly] List<BasicExpedition> _expeditionsInProgress = new List<BasicExpedition>();
        List<BasicExpedition> _finishedExpeditions = new List<BasicExpedition>();

        float _currentSecDelta = 0;
        TimeSpan _second = new TimeSpan(0, 0, 1);

        void OnEnable() => BasicExpedition.OnExpeditionSateChange += ChangeInProgressList;

        #region LISTENERS
        void ChangeInProgressList(BasicExpedition expedition) {
            if (expedition.OnExpedition) {
                _expeditionsInProgress.Add(expedition);
                return;
            }
            _expeditionsInProgress.Remove(expedition);
        }

        #endregion

        void Update() {
            if (!IsSecondOver()) return;
            SendASecondToExpeditions();
        }

        bool IsSecondOver() {
            _currentSecDelta += Time.deltaTime;
            if (_currentSecDelta < 1) return false;
            _currentSecDelta--;
            return true;
        }

        void SendASecondToExpeditions() {
            foreach (BasicExpedition expedition in _expeditionsInProgress) {
                if (!expedition.ReduceExpeditionTime(_second)) continue;
                _finishedExpeditions.Add(expedition);
            }
            ClearFinishedExpeditions();
        }

        void ClearFinishedExpeditions() {
            foreach (BasicExpedition expedition in _finishedExpeditions) {
                expedition.FinishExpedition();
            }
            _finishedExpeditions.Clear();
        }
    }
}
