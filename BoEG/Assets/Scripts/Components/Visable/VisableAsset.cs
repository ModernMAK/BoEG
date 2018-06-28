using UnityEngine;

namespace Components.Visable
{
    [CreateAssetMenu(menuName = "Component/Visable")]
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