using System;
using UnityEngine;
using Heroes.Scriptable;

namespace Expeditions.Scriptable {
    [CreateAssetMenu(fileName = "New_Expedition", menuName = "Expedition/Basic Expedition", order = 1)]
    public class BasicExpedition : BasicMission {
        #region ENUMS
        enum ScalingFactor {
            Attack,
            Defence,
            Mixed
        }
        #endregion
        
        #region VARS
        const uint TickTime = 10;

        bool _canEarlyFinish;
        TimeSpan _initExplorationTime;
        readonly TimeSpan _earlyFinishCheck = new TimeSpan(12, 0, 0);

        [Header("Expedition Details")]
        [SerializeField] uint _damagePerTick;
        [SerializeField] ScalingFactor _scalingFactor;
        #endregion

        #region OBSERVERS
        public static event Action<BasicExpedition> OnExpeditionSateChange;
        public static event Action<bool> OnEarlyFinishChange;
        public static event Action<uint> OnExpeditionRewards;
        #endregion

        #region PUBLIC_FUNCTIONS
        override public bool ReduceExpeditionTime(TimeSpan timeElapsed) {
            if (base.ReduceExpeditionTime(timeElapsed)) return true;
            CheckIfEarlyFinish();
            return false;
        }

        public void StartExpedition() {
            if(CanStartMission()) return;
            OnMission = true;
            CalculateExpeditionTime();
            OnExpeditionSateChange?.Invoke(this);
        }

        public void FinishEarly() {
            if (!_canEarlyFinish) return;
            FinishExpedition();
        }

        public void FinishExpedition() {
            if (!OnMission) return;
            OnMission = false;
            _canEarlyFinish = false;
            OnEarlyFinishChange?.Invoke(_canEarlyFinish);
            OnExpeditionSateChange?.Invoke(this);
            CalculateFinalRewards();
        }
        #endregion

        #region PRIVATE_FUNCTIONS
        void CalculateFinalRewards() {
            float timeScaling = GetTimeScaling();
            float statsScaling = _scalingFactor switch {
                ScalingFactor.Attack => GetPartyAtk() / (float)_requiredPower,
                ScalingFactor.Defence => GetPartyHp() / (float)_requiredPower,
                ScalingFactor.Mixed => GetPartyHp() + GetPartyAtk() / (float)_requiredPower,
                _ => throw new ArgumentOutOfRangeException()
            };
            var rewards = (uint)(_baseRewards * statsScaling * timeScaling);
            OnExpeditionRewards?.Invoke(rewards);
        }

        float GetTimeScaling() {
            TimeSpan expeditionLength = _initExplorationTime - _explorationTime;
            return _baseRewards * (float)expeditionLength.TotalHours;
        }

        void CalculateExpeditionTime() {
            uint partyHp = GetPartyHp();
            float ticksToSurvive = partyHp / (float)_damagePerTick;
            _explorationTime = TimeSpan.FromSeconds(ticksToSurvive * TickTime);
            _initExplorationTime = _explorationTime;
            CallOnTimerChange();
        }

        void CheckIfEarlyFinish() {
            TimeSpan passedTime = _initExplorationTime - _explorationTime;
            if (passedTime < _earlyFinishCheck) return;
            _canEarlyFinish = true;
            OnEarlyFinishChange?.Invoke(_canEarlyFinish);
        }
        #endregion
    }
}