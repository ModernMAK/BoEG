using UnityEngine;

namespace Old.Entity.Modules.Visable
{
    [CreateAssetMenu(fileName = "Visable.asset", menuName = "Entity/Module/Visable")]
    public class VisableAsset : ScriptableObject, IVisableData
    {
        [SerializeField] private VisableData _data;

        public bool HasSpotted
        {
            get { return _data.HasSpotted; }
        }

        public bool HasInvisability
        {
            get { return _data.HasInvisability; }
        }

        public bool HasHidden
        {
            get { return _data.HasHidden; }
        }
    }
}