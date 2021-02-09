using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.GrimDeath
{
    [CreateAssetMenu(menuName = "Ability/GrimDeath/Reap")]
    public class Reap : AbilityObject, IVectorTargetAbility
    {
        [SerializeField]
        private float _searchRange;

        [SerializeField]
        [Tooltip("0-360")]
        private float _sideAngle;
        //-1 = 180
        //0 = 90
        //1 = 0
        private float NormalizedSideAngle => 1 - (_sideAngle / 2f) / 90f;

        [SerializeField]
        private float _damage;


        public void CastVectorTarget(Vector3 worldPos, Vector3 direction)
        {
            var searchRange = _searchRange;
            var minDot = NormalizedSideAngle;
            var hasTeamable = Modules.TryGetCached<ITeamable>(out var teamable);

            var colliders = Physics.OverlapSphere(worldPos, searchRange, (int)LayerMaskHelper.Entity);
            foreach (var col in colliders)
            {
                //Actor check
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    continue;
                //Outside of angle range check
                var fwd = actor.transform.position - worldPos;
                if(Vector3.Dot(worldPos,fwd) < minDot)
                    continue;
                
                //Ally check
                if(hasTeamable)
                    if(actor.TryGetModule<ITeamable>(out var otherTeamable))
                        if(teamable.IsAlly(otherTeamable))
                           continue; 
            
                //Valid target, hit the actor
                TryHitActor(actor);
                
            }
        }

        private void TryHitActor(Actor target)
        {
            var damage = new SourcedDamage(Self,new Damage(_damage,DamageType.Pure,DamageFlags.Ability));
            if (target.TryGetModule<IDamageable>(out var damageable))
                damageable.TakeDamage(damage);
        }
    }
}