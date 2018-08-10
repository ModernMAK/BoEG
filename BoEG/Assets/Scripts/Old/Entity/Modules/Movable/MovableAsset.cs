using UnityEngine;

namespace Old.Entity.Modules.Movable
{
    [CreateAssetMenu(fileName = "Movable.asset", menuName = "Entity/Module/Movable")]
    public class MovableAsset : ScriptableObject, IMovableData
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