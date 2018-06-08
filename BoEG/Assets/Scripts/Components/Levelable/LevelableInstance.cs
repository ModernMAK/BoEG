using System;
using UnityEngine;

namespace Components.Levelable
{
    [Serializable]
    public class LevelableInstance : Module, ILevelableInstance
    {
        public LevelableInstance(ILevelableData data)
        {
            _data = data;
            _level = data.InitialLevel;
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