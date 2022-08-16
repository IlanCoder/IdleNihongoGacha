using System;
using Heroes.Scriptable;
using UnityEngine;

namespace Expeditions.Scriptable
{
    [CreateAssetMenu(fileName = "New_Expedition", menuName = "Expedition/Basic Expedition", order = 1)]
    public class BasicExpedition : ScriptableObject
    {
        #region ENUMS
        public enum Field
        {
            Plains = Hero.Element.Wind,
            Mountain = Hero.Element.Earth,
            Forest = Hero.Element.Nature,
        }

        #endregion

        #region VARS

        const uint PartySize = 4;
        const uint TickTime = 10;
        const float FriendshipBoost = .25f;
        const float ElementBoost = .5f;

        public bool OnExpedition { get; private set; }
        bool _canEarlyFinish;
        readonly Hero[] _party = new Hero[PartySize];
        TimeSpan _explorationTime;
        TimeSpan _initExplorationTime;
        readonly TimeSpan _earlyFinishCheck = new TimeSpan(12, 0, 0);

        [Header("Expedition Details")] [SerializeField, Min(1)]
        uint _damagePerTick;

        [SerializeField] Field _field;

        public Field ExpeditionField { get { return _field; } }

        #endregion

        #region OBSERVERS

        public static event Action<TimeSpan> OnTimerChange;
        public static event Action<BasicExpedition> OnExpeditionSateChange;
        public static event Action<bool> OnEarlyFinishChange;

        #endregion

        #region PUBLIC_FUNCTIONS

        public bool ReduceExpeditionTime(TimeSpan timeElapsed) {
            if (!OnExpedition) return false;
            _explorationTime -= timeElapsed;
            OnTimerChange?.Invoke(_explorationTime);
            CheckIfEarlyFinish();
            return _explorationTime.TotalSeconds <= 0;
        }

        public void StartExpedition()
        {
            if (OnExpedition) return;
            if (IsPartyEmpty()) return;
            CalculateExpeditionTime();
            OnExpedition = true;
            OnExpeditionSateChange?.Invoke(this);
        }

        public void FinishEarly() {
            if (!_canEarlyFinish) return;
            FinishExpedition();
        }

        public void FinishExpedition()
        {
            if (!OnExpedition) return;
            OnExpedition = false;
            _canEarlyFinish = false;
            OnEarlyFinishChange?.Invoke(_canEarlyFinish);
            OnExpeditionSateChange?.Invoke(this);
        }

        public void AddHero(Hero newHero)
        {
            if (OnExpedition) return;
            if (newHero == null) return;
            if (newHero.OnExpedition) return;
            for (var i = 0; i < PartySize; i++)
            {
                if (_party[i] != null) continue;
                _party[i] = newHero;
                newHero.AddToExpedition();
                return;
            }
        }

        public void RemoveHero(Hero hero)
        {
            if (hero == null) return;
            for (var i = 0; i < PartySize; i++) {
                if (_party[i] != hero) continue;
                _party[i].RemoveFromExpedition();
                _party[i] = null;
                return;
            }
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        void CalculateExpeditionTime()
        {
            uint partyHp = GetPartyHp();
            float ticksToSurvive = partyHp / (float)_damagePerTick;
            _explorationTime = TimeSpan.FromSeconds(ticksToSurvive * TickTime);
            _initExplorationTime = _explorationTime;
            OnTimerChange?.Invoke(_explorationTime);
        }

        void CheckIfEarlyFinish()
        {
            TimeSpan passedTime = _initExplorationTime - _explorationTime;
            if (passedTime < _earlyFinishCheck) return;
            _canEarlyFinish = true;
            OnEarlyFinishChange?.Invoke(_canEarlyFinish);
        }

        uint GetPartyHp()
        {
            uint partyHp = 0;
            foreach (Hero hero in _party)
            {
                if (hero == null) continue;
                partyHp += GetBoostedStat(hero, hero.GetFinalHp());
            }

            return partyHp;
        }

        uint GetBoostedStat(Hero hero, uint statToBoost)
        {
            if (hero.HeroElement == Hero.Element.Friendship)
            {
                return Convert.ToUInt32(statToBoost * (1 + FriendshipBoost));
            }

            if ((int)hero.HeroElement == (int)_field)
            {
                return Convert.ToUInt32(statToBoost * (1 + ElementBoost));
            }

            return Convert.ToUInt32(statToBoost * ElementBoost);
        }

        bool IsPartyEmpty()
        {
            foreach (Hero hero in _party)
            {
                if (hero != null) return false;
            }

            return true;
        }

        #endregion

        #region UNITY_EDITOR_FUNCTIONS

#if UNITY_EDITOR
        [ContextMenu("Empty Party")]
        public void EmptyParty()
        {
            for (int i = 0; i < PartySize; i++)
            {
                RemoveHero(_party[i]);
            }
        }

        [ContextMenu("Reset Expedition")]
        public void ResetExpedition()
        {
            OnExpedition = false;
            EmptyParty();
        }
#endif

        #endregion
    }
}