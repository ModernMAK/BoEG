using Framework.Types;
using UnityEngine;

namespace Framework.Core
{
    [DisallowMultipleComponent]
    public class Unit : MonoBehaviour
    {
        private IStepable[] _stepables;
        private ISpawnable[] _spawnables;
        private IInstantiable[] _instantiables;
        
//        protected virtual void Awake()
//        {
//        }

        private void Start()
        {
            _stepables = GetComponents<IStepable>();
            _spawnables = GetComponents<ISpawnable>();
            _instantiables = GetComponents<IInstantiable>();
            StartLogic();
        }

        protected virtual void StartLogic()
        {
            
        }

//        protected virtual void OnEnable()
//        {
//            //Do nothing
//        }
//
//        protected virtual void OnDisable()
//        {
//            //Do something
//        }

        private void Update()
        {
            foreach (var stepable in _stepables)
            {
                stepable.Step(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            foreach (var stepable in _stepables)
            {
                stepable.PostStep(Time.deltaTime);
            }

            foreach (var stepable in _stepables)
            {
                stepable.PreStep(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (var stepable in _stepables)
            {
                stepable.PhysicsStep(Time.fixedDeltaTime);
            }
        }

        public void Instanitate()
        {
            foreach (var instantiable in _instantiables)
            {
                instantiable.Instantiate();                
            }
        }
        public void Terminate()
        {
            foreach (var instantiable in _instantiables)
            {
                instantiable.Terminate();                
            }
        }

        public void Spawn()
        {
            foreach (var spawnable in _spawnables)
            {
                spawnable.Spawn();                
            }
        }
        public void Despawn()
        {
            foreach (var spawnable in _spawnables)
            {
                spawnable.Despawn();                
            }
        }
    }
}