//using System;
//using Module.Configuration;
//using UnityEngine;
//using UnityEngine.Networking;
//using Random = UnityEngine.Random;
//
//namespace Components.Abilityable
//{
//    [CreateAssetMenu(menuName = "Abilities/SpawnAbility")]
//    public class SpawnAbility : Ability
//    {
//        public GameObject SpawnPrefab;
//        public int SpawnAmount;
//
//        protected Transform Transform;
//        protected NetworkBehaviour NetworkBehaviour;
//
//        public override void Initialize(GameObject go)
//        {
//            Transform = go.transform;
//            NetworkBehaviour = go.GetComponent<NetworkBehaviour>();
//        }
//
//        [Obsolete("Use Trigger(CastData) instead")]
//        protected override void Trigger()
//        {
//        }
//
//        protected override void Trigger(CastData castData)
//        {
//            if (!NetworkBehaviour.isServer) return;
//            for (int i = 0; i < SpawnAmount; i++)
//            {
//                var spawned = Instantiate(SpawnPrefab, Transform.position + Random.onUnitSphere / 100f,
//                    Transform.rotation);
//                NetworkServer.Spawn(spawned);
//            }
//        }
//    }
//    
//    [CreateAssetMenu(menuName = "Abilities/Scar/Pounce")]
//    public class ScarPounceAbility : Ability
//    {
//        private IMovable _movable;
//        private ITeamable _teamable;
//        public float[] DamageScale = {1, 2, 3};
//        public override void Initialize(GameObject go)
//        {
//            _movable = go.GetComponent<IMovable>();
//            _teamable = go.GetComponent<ITeamable>();
//        }
//
//        protected override void Trigger()
//        {
//            GameObject target = null;
//            if (target == null)
//                return;
//
//            var targetTeamable = target.GetComponent<ITeamable>();
//            if (_teamable == null || targetTeamable == null || !_teamable.IsAlly(targetTeamable))
//            {
//                var lookPosition = target.transform.position;
//                var jumpPosition = lookPosition - target.transform.forward;
//                _movable.Teleport(jumpPosition);
//            }
//
//        }
//
//        protected override void Trigger(CastData castData)
//        {
//            if(!castData.CastType.HasFlag(AbilityCast.TargetUnit))
//                return;
//            
//            var target = castData.TargetUnit;                       
//            if (target == null)
//                return;
//
//            var targetTeamable = target.GetComponent<ITeamable>();
//            if (_teamable == null || targetTeamable == null || !_teamable.IsAlly(targetTeamable))
//            {
//                var lookPosition = target.transform.position;
//                var jumpPosition = lookPosition - target.transform.forward;
//                _movable.Teleport(jumpPosition);
//                
//            }
//        }
//    }
//}