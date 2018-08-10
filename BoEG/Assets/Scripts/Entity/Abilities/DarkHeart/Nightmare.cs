using Core;
using Modules;
using Modules.Healthable;
using Modules.Abilityable;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(fileName = "DarkHeart_Nightmare.asset", menuName = "Ability/DarkHeart/Nightmare")]
    public class Nightmare : Ability
    {
        private float _duration;

        private float _damageOnCompletion;

        private float _damageWakenThreshold;
        //Nightmare Duration
        //Nightmare AttackDamage On Completion
        //Nightmare AttackDamage Threshold

        public override void Terminate()
        {
            
        }
        
        
        public override void Initialize(GameObject go)
        {
//            throw new System.NotImplementedException();
        }

        public override void Trigger()
        {
            //          throw new System.NotImplementedException();
        }

        public void ApplyNightmare(GameObject go)
        {
        }
    }

//    public class NightmareEffect : Effect
//    {
//        public NightmareEffect(float duration, float completionDamage)
//        {
//            _timeDuration = duration;
//            _completionDamage = completionDamage;
//        }
//        private float _timeStarted, _timeDuration;
//        private float _completionDamage;
//        private IHealthable _targetHealthable;
//        public override void Initialize(GameObject target)
//        {
//            base.Initialize(target);
//            _timeStarted = Time.time;
//            _targetHealthable = Target.GetComponent<IHealthable>();
//        }
//
//        public override void PreTick()
//        {
//            _timeStarted
//        }
//
//        public override void PostTick()
//        {
//            if (_timeStarted + _timeDuration <= Time.time)
//            {
//                
//                Terminate();   
//            }
//        }
//
//        private void ApplyDamage()
//        {
//            _targetHealthable.TakeDamage(new Damage());
//        }
//    }
}