using System;
using UnityEngine;

namespace Old.Entity.Modules.Levelable
{
    [Serializable]
    public class Levelable : Module, ILevelable
    {
        protected override void Initialize()
        {
            _data = GetData<ILevelableData>();
            _level = _data.InitialLevel;
            _experience = 0f;
        }
        
        [SerializeField] private ILevelableData _data;
        [SerializeField] private float _experience;
        [SerializeField] private int _level;

        public float Experience
        {
            get { return _experience; }
        }

        public int Level
        {
            get { return _level; }
        }
    }
}