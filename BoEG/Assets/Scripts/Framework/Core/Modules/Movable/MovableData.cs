using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct MovableData : IMovableData
    {
#pragma warning disable 649
        [SerializeField]
        private float _moveSpeed;
        [SerializeField]
        private float _turnSpeed;
#pragma warning restore 649

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