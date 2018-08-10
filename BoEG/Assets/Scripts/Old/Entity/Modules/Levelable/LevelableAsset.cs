using UnityEngine;

namespace Old.Entity.Modules.Levelable
{
    [CreateAssetMenu(fileName = "Levelable.asset", menuName = "Entity/Module/Levelable")]
    public class LevelableAsset : ScriptableObject, ILevelableData
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