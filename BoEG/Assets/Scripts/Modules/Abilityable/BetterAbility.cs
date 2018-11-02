//using System;
//using Modules.Magicable;
//using UnityEngine;
//using System.Runtime;
//
//namespace Modules.Abilityable
//{
//    public abstract class BetterAbility : Ability
//    {
//        protected GameObject Self { get; private set; }
//        protected IMagicable Magicable { get; private set; }
//
//
//        public override void Initialize(GameObject go)
//        {
//            Self = go;
//            Magicable = go.GetComponent<IMagicable>();
//            _level = 0;
//            StopChannel(); //You can stop a channel, 
//            ResetCooldown(); //but stopping a cooldown sounds like pausing; so we Reset it instead
//        }
//
//        #region IconInfo
//
//        [SerializeField] private Sprite _icon;
//
//        public Sprite Icon
//        {
//            get { return _icon; }
//        }
//
//        #endregion
//
//        #region ManaInfo
//
//        public virtual float ManaCost
//        {
//            get { return 0; }
//        }
//
//        public bool RequiresMana
//        {
//            get { return ManaCost > 0f; }
//        }
//
//        public bool HasEnoughMana
//        {
//            get { return Magicable.ManaPoints >= ManaCost; }
//        }
//
//        protected void SpendMana()
//        {
//            Magicable.ModifyMana(-ManaCost, Self);
//        }
//
//        #endregion
//
//        #region LevelInfo
//
//        private int _level;
//
//        public int Level
//        {
//            get { return _level; }
//            protected set { _level = value; }
//        }
//
//        protected virtual int MaxLevel
//        {
//            get { return 0; }
//        }
//
//        public bool IsLeveled
//        {
//            get { return Level > 0; }
//        }
//
//        public virtual bool CanLevelUp
//        {
//            get { return Level < MaxLevel; }
//        }
//
//        public sealed override void LevelUp()
//        {
//            if (CanLevelUp)
//                _level++;
//        }
//
//
//        protected T GetLeveledData<T>(T[] data, T def = default(T))
//        {
//            var lvl = Level;
//            if (lvl <= 0 || data.Length == 0)
//                return def; //Use default
//            if (lvl >= data.Length)
//                return data[data.Length - 1]; //Use highest value
//            return data[lvl - 1]; //Evaluate
//        }
//
//        #endregion
//
//        #region CastRangeInfo
//
//        public virtual float CastRange
//        {
//            get { return 0; }
//        }
//
//        public bool InCastRange(Vector3 point)
//        {
//            var range = CastRange;
//            return ((point - Self.transform.position).sqrMagnitude <= (range * range));
//        }
//
//        #endregion
//
//
//        #region CastInfo
//
//        private bool _isPreparing;
//
//        public bool Preparing
//        {
//            get { return _isPreparing; }
//            protected set { _isPreparing = value; }
//        }
//
//
//        public virtual bool CanCast
//        {
//            get { return IsLeveled && HasEnoughMana && OffCooldown && !IsChanneling; }
//        }
//
//        //
//        protected virtual void Prepare()
//        {
//        }
//
//        protected virtual void CancelPrepare()
//        {
//        }
//
//        protected virtual void Cast()
//        {
//        }
//
//        public sealed override void Trigger()
//        {
//            if (CanCast && !Preparing)
//            {
//                Preparing = true;
//                Prepare();
//            }
//            else if (Preparing)
//            {
//                Preparing = false;
//                if (CanCast)
//                    Cast();
//                CancelPrepare();
//            }
//        }
//
//        //Some spells rely on other spells to function,
//        //If this spell needs to be ground casted, this will be called.
//        //Despite being defined, there is no garuntee the cast will be Implimented
//        //This method should not Spend Mana, Begin Channeling The Spell, Raise A Spell Cast Event, Or Put The Spell On Cooldown
//        public virtual void GroundCast(Vector3 point)
//        {
//            throw new NotImplementedException();
//        }
//
//        //Some spells rely on other spells to function,
//        //If this spell needs to be unit casted, this will be called.
//        //Despite being defined, there is no garuntee the cast will be Implimented
//        //This method should not Spend Mana, Begin Channeling The Spell, Raise A Spell Cast Event, Or Put The Spell On Cooldown
//        public virtual void UnitCast(GameObject target)
//        {
//            throw new NotImplementedException();
//        }
//
//        #endregion
//
//
//        #region ChannelInfo
//
//        private float _myChannelStart;
//        private bool _isChanneling;
//
//        public virtual float MaxChannelDuration
//        {
//            get { return 0; }
//        }
//
//        public bool IsChanneling
//        {
//            get { return _isChanneling; }
//        }
//
//        protected void StartChannel()
//        {
//            _myChannelStart = Time.time;
//            _isChanneling = true;
//        }
//
//        protected void StopChannel()
//        {
//            _myChannelStart = 0f;
//            _isChanneling = false;
//        }
//
//        public float ChannelProgress
//        {
//            get { return IsChanneling ? 0f : Mathf.Min((Time.time - _myChannelStart), MaxChannelDuration); }
//        }
//
//        //0 -> the channel began (Or is not being channeled)
//        //1 -> the channel completed
//        public float ChannelProgressNormalized
//        {
//            get
//            {
//                var mcd = MaxChannelDuration;
//                return mcd > 0f ? (ChannelProgress / mcd) : 1f;
//            }
//        }
//
//        #endregion
//
//        #region CooldownInfo
//
//        private float _myCooldownStart;
//
//        public virtual float Cooldown
//        {
//            get { return 0; }
//        }
//
//        protected void StartCooldown()
//        {
//            _myCooldownStart = Time.time;
//        }
//
//        protected void ResetCooldown()
//        {
//            _myCooldownStart = 0f;
//        }
//
//        public float CooldownRemaining
//        {
//            get { return Mathf.Max(Cooldown - (Time.time - _myCooldownStart), 0f); }
//        }
//
//        //1 -> the full cooldown remains
//        //0 -> the cooldown was completed
//        public float CooldownRemainingNormalized
//        {
//            get
//            {
//                var cd = Cooldown;
//                return cd > 0f ? (CooldownRemaining / cd) : 0f;
//            }
//        }
//
//        public bool OffCooldown
//        {
//            get { return Cooldown <= 0f || CooldownRemaining <= 0f; }
//        }
//
//        #endregion
//    }
//}