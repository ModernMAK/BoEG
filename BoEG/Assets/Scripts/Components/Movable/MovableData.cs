using UnityEngine;

namespace Components.Movable
{
    [System.Serializable]
    public struct MovableData : IMovableData
    {
        //BASE
        [SerializeField] private float _baseMoveSpeed;

        [SerializeField] private float _baseTurnSpeed;


        public float BaseMoveSpeed
        {
            get { return _baseMoveSpeed; }
        }

        public float BaseTurnSpeed
        {
            get { return _baseTurnSpeed; }
        }
    }
}