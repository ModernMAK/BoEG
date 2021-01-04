using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [Serializable]
    public struct MovableData : IMovableData
    {
#pragma warning disable 649
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;
#pragma warning restore 649

        public float MoveSpeed => _moveSpeed;

        public float TurnSpeed => _turnSpeed;

        public static MovableData Default => new MovableData()
        {
            _moveSpeed = 2f,
            _turnSpeed = 360f,
        };
    }
}