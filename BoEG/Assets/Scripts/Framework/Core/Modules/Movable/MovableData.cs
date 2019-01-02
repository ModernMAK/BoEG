using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct MovableData : IMovableData
    {
        [SerializeField]
        private float _moveSpeed;
        [SerializeField]
        private float _turnSpeed;

        public float MoveSpeed
        {
            get { return _moveSpeed; }
        }

        public float TurnSpeed
        {
            get { return _turnSpeed; }
        }
    }
}