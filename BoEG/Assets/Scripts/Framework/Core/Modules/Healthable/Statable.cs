using UnityEngine;

namespace Framework.Core.Modules
{
    public class Statable
    {
        protected Statable(float capacity, float generation)
        {
            _capacity = capacity;
            _normal = 1f;
            _generation = generation;
        }

        private float _normal;
        private float _capacity;
        private float _generation;

        protected float Value
        {
            get => Normal * Capacity;
            set => Normal = value / Capacity;
        }

        protected float Normal
        {
            get => _normal;
            set => _normal = Mathf.Clamp01(value);
        }

        protected float Capacity { 
            get => _capacity;
            set => _capacity = value; }

        protected float Generation
        {
            get => _generation;
            set => _generation = value;
        }
    }
}