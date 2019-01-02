using Framework.Types;
using Framework.Utility;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Magicable : Module, IMagicable
    {
        protected override void PreStep(float delta)
        {
            if (IsSpawned)
                GenerateMagic(delta);
        }

        private void GenerateMagic(float delta)
        {
            ModifyPoints(Magic.Generation * delta);
        }

        private void ModifyPoints(float deltaPoints)
        {
            var nPoints = Mathf.Clamp(Magic.Points + deltaPoints, 0f, Magic.Capacity);
            Magic = Magic.SetPoints(nPoints);
        }

        /// <summary>
        /// The Bit Mask used for serialization.
        /// </summary>
        private byte _dirtyMask;

        private IMagicableData _data;

        public PointData Magic { get; private set; }


        public void ModifyMagic(float deltaValue)
        {
            if (!IsSpawned)
                return;
            OnMagicModifying(deltaValue);
            ModifyPoints(deltaValue);
            OnMagicModified(deltaValue);
        }

        public event MagicChangeHandler MagicModified;
        public event MagicChangeHandler MagicModifying;

        protected void OnMagicModified(float deltaValue)
        {
            if (MagicModified != null)
                MagicModified(deltaValue);
        }

        protected void OnMagicModifying(float deltaValue)
        {
            if (MagicModifying != null)
                MagicModifying(deltaValue);
        }

        protected override void Instantiate()
        {
            _data = GetData<IMagicableData>();
        }

        protected override void Spawn()
        {
            Magic = Magic.SetPercentage(1f);
        }
    }
}