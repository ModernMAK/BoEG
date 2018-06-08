using UnityEngine;

namespace Components.Movable
{
    [CreateAssetMenu(menuName = "Component/Movable")]
    public class MovableDataAsset : ScriptableObject, IMovableData
    {
        [SerializeField] private MovableData _data;

        public float BaseMoveSpeed
        {
            get { return _data.BaseMoveSpeed; }
        }

        public float BaseTurnSpeed
        {
            get { return _data.BaseTurnSpeed; }
        }
    }
}