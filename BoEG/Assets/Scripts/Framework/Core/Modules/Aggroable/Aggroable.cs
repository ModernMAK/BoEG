using UnityEngine;

namespace Framework.Core.Modules
{
    public class Aggroable : Module, IAggroable
    {
        protected override void Instantiate()
        {
            base.Instantiate();
            GetData(out _data);
        }

        private IAggroable _data;
        
        public float AggroRange
        {
            get { return IsInitialized ? _data.AggroRange : 0f; }
        }


        public bool InAggro(GameObject go)
        {
            var delta = go.transform.position - transform.position;
            return delta.sqrMagnitude <= AggroRange * AggroRange;
        }
    }
}