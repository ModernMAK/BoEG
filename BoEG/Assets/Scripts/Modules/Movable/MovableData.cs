using System;
using UnityEngine;

namespace Modules.Movable
{
    [Serializable]
    public struct MovableData : IMovableData
    {
        public static MovableData Default
        {
            get
            {
                return new MovableData()
                {
                    _moveSpeed = 5f,
                    _turnSpeed = 5f
                };
            }
        }
        
        [SerializeField] private float _moveSpeed;

        [SerializeField] private float _turnSpeed;


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