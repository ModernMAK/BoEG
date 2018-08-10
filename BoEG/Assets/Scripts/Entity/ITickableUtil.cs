using System.Collections.Generic;

namespace Entity
{
    public static class ITickableUtil{
        public static void PreTick<T>(this IEnumerable<T> self, float deltaTick) where T : ITickable
        {
            foreach (var tickable in self)
            {
                tickable.PreTick(deltaTick);
            }
        }
        public static void Tick<T>(this IEnumerable<T> self, float deltaTick) where T : ITickable
        {
            foreach (var tickable in self)
            {
                tickable.Tick(deltaTick);
            }
        }
        public static void PostTick<T>(this IEnumerable<T> self, float deltaTick) where T : ITickable
        {
            foreach (var tickable in self)
            {
                tickable.PostTick(deltaTick);
            }
        }
        public static void PhysicsTick<T>(this IEnumerable<T> self, float deltaTick) where T : ITickable
        {
            foreach (var tickable in self)
            {
                tickable.PhysicsTick(deltaTick);
            }
        }
        
    }
}