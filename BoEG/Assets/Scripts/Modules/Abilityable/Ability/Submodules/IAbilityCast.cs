using UnityEngine;

namespace Modules.Abilityable.Ability
{
    public interface IAbilityCast
    {
        bool Preparing { get; }
        bool CanCast { get; }
        void Prepare();
        void CancelPrepare();
        void Cast();
        void Trigger();
        void GroundCast(Vector3 point);
        void UnitCast(GameObject target);
    }
}