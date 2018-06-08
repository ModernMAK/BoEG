using UnityEngine;

namespace Components.Levelable
{
    public class LevelableDataAsset : ILevelableData
    {
        [SerializeField] private LevelableData _data;

        public int InitialLevel
        {
            get { return _data.InitialLevel; }
        }

        public int MaxLevel
        {
            get { return _data.MaxLevel; }
        }

        public int[] ExperienceCurve
        {
            get { return _data.ExperienceCurve; }
        }
    }
}