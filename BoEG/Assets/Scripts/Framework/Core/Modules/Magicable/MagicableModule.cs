using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class MagicableModule : IModule, IMagicable
    {
        private readonly IMagicable _magicable;

        public MagicableModule(IMagicable magicable)
        {
            _magicable = magicable;
        }

//        protected override void PreStep(float delta)
//        {
//            if (IsSpawned)
//                GenerateMagic(delta);
//        }
//
//        private void GenerateMagic(float delta)
//        {
//            ModifyPoints(Magic.Generation * delta);
//        }
//
//        private void ModifyPoints(float deltaPoints)
//        {
//            var nPoints = Mathf.Clamp(Magic.Points + deltaPoints, 0f, Magic.Capacity);
//            Magic = Magic.SetPoints(nPoints);
//        }
//
//        /// <summary>
//        /// The Bit Mask used for serialization.
//        /// </summary>
//        private byte _dirtyMask;
//
////        private IMagicableData _data;
//
//        public PointData Magic { get; private set; }
//
//
//        public void ModifyMagic(float deltaValue)
//        {
//            if (!IsSpawned)
//                return;
//            OnMagicModifying(deltaValue);
//            ModifyPoints(deltaValue);
//            OnMagicModified(deltaValue);
//        }
//
//        public event MagicChangeHandler MagicModified;
//        public event MagicChangeHandler MagicModifying;
//
//        protected void OnMagicModified(float deltaValue)
//        {
//            if (MagicModified != null)
//                MagicModified(deltaValue);
//        }
//
//        protected void OnMagicModifying(float deltaValue)
//        {
//            if (MagicModifying != null)
//                MagicModifying(deltaValue);
//        }
//
//        protected override void Instantiate()
//        {
////            _data = GetData<IMagicableData>();
//        }
//
//        protected override void Spawn()
//        {
//            Magic = Magic.SetPercentage(1f);
//        }
        public float Mana => _magicable.Mana;

        public float ManaPercentage => _magicable.ManaPercentage;

        public float ManaCapacity => _magicable.ManaCapacity;

        public float ManaGeneration => _magicable.ManaGeneration;

        public void ModifyMana(float change)
        {
            _magicable.ModifyMana(change);
        }

        public void SetMana(float mana)
        {
            _magicable.SetMana(mana);
        }

        public event EventHandler<MagicableEventArgs> Modified
        {
            add => _magicable.Modified += value;
            remove => _magicable.Modified -= value;
        }

        public event EventHandler<MagicableEventArgs> Modifying
        {
            add => _magicable.Modifying += value;
            remove => _magicable.Modifying -= value;
        }

        public void PreStep(float deltaTime)
        {
            ModifyMana(ManaGeneration * deltaTime);
        }

        public void Step(float deltaTime)
        {
            //Do nothing
        }

        public void PostStep(float deltaTime)
        {
            //Do nothing
        }

        public void PhysicsStep(float deltaTime)
        {
            //Do nothing
        }

        public void Spawn()
        {
            SetMana(ManaCapacity);
        }

        public void Despawn()
        {
            //Do Nothing
        }

        public void Instantiate()
        {
            SetMana(ManaCapacity);
        }

        public void Terminate()
        {
            //Do Nothing
        }
    }
}