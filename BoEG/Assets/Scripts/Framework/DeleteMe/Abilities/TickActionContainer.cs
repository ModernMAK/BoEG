using System.Collections.Generic;

namespace Entity.Abilities
{
    public class TickActionContainer<T> where T : TickAction
    {
        private readonly List<T> _internalContainer;

        public TickActionContainer()
        {
            _internalContainer = new List<T>();
        }

        public void Add(T instance)
        {
            _internalContainer.Add(instance);
        }

        public void Tick(float deltaTick)
        {
            for (var i = 0; i < _internalContainer.Count; i++)
            {
                var inst = _internalContainer[i];
                if (inst.DoneTicking)
                {
                    inst.Terminate();
                    _internalContainer.RemoveAt(i);
                    i--;
                }
                else
                {
                    inst.Tick(deltaTick);
                }
            }
        }

        public void Terminate()
        {
            foreach (var inst in _internalContainer)
            {
                inst.Terminate();
            }

            _internalContainer.Clear();
        }
    }
}