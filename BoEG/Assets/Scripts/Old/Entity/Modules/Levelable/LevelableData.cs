using UnityEngine;

namespace Old.Entity.Modules.Levelable
{
    [System.Serializable]
    public struct LevelableData : ILevelableData
    {
        [SerializeField] private int _initialLevel;
        [SerializeField] private int _maxLevel;
        [SerializeField] private int[] _experienceCurve;

        public int InitialLevel
        {
            get { return _initialLevel; }            
        }

        public int MaxLevel
        {
            get { return _maxLevel; }
        }

        public int[] ExperienceCurve
        {
            get { return _experienceCurve; }
        }
    }
}