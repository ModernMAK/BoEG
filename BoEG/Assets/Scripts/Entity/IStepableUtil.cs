using System.Collections.Generic;

namespace Entity
{
    public static class IStepableUtil{
        public static void PreStep<T>(this IEnumerable<T> self, float deltaStep) where T : IStepableOld
        {
            foreach (var steppable in self)
            {
                steppable.PreStep(deltaStep);
            }
        }
        public static void Step<T>(this IEnumerable<T> self, float deltaStep) where T : IStepableOld
        {
            foreach (var steppable in self)
            {
                steppable.Step(deltaStep);
            }
        }
        public static void PostStep<T>(this IEnumerable<T> self, float deltaStep) where T : IStepableOld
        {
            foreach (var steppable in self)
            {
                steppable.PostStep(deltaStep);
            }
        }
        public static void PhysicsStep<T>(this IEnumerable<T> self, float deltaStep) where T : IStepableOld
        {
            foreach (var steppable in self)
            {
                steppable.PhysicsStep(deltaStep);
            }
        }
        
    }
}