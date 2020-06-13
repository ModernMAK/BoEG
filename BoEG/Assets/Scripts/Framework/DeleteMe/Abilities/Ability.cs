using UnityEngine;

namespace Framework.Ability
{
    public abstract class Ability : MonoBehaviour
    {
        //Methods of casting
        public virtual void UnitCast(GameObject target)
        {
        }

        public virtual void VectorCast(Vector3 origin, Vector3 direction)
        {
        }

        public virtual void GroundCast(Vector3 origin)
        {
        }

        public virtual void SelfCast()
        {
        }
    }
}