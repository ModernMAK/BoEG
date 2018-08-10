using UnityEngine;

namespace Old.Entity.Modules.Visable
{
    public struct VisableData : IVisableData
    {
        [SerializeField] private bool _hasSpotted;
        [SerializeField] private bool _hasInvisability;
        [SerializeField] private bool _hasHidden;

        public bool HasSpotted
        {
            get { return _hasSpotted; }
        }

        public bool HasInvisability
        {
            get { return _hasInvisability; }
        }

        public bool HasHidden
        {
            get { return _hasHidden; }
        }
    }
}