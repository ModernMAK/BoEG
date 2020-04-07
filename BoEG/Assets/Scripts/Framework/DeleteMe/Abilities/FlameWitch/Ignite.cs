//using Core;
//using Framework.Types;
//using Modules.Abilityable;
//using Modules.Healthable;
//using Triggers;
//using UnityEngine;
//using Util;
//
//namespace Entity.Abilities.FlameWitch
//{
//    [CreateAssetMenu(fileName = "FlameWitch_Ignite.asset", menuName = "Ability/FlameWitch/Ignite")]
//    public class Ignite : Ability
//    {
//        [SerializeField] private float _manaCost = 100f;
//        [SerializeField] private float _damage = 100f;
//        [SerializeField] private float _castRange = 5f;
//
//
//        public override float ManaCost
//        {
//            get { return _manaCost; }
//        }
//
//        public override float CastRange
//        {
//            get { return _castRange; }
//        }
//
//
//        public override void Terminate()
//        {
//            //I'll Be Back
//            //To add stuff
//        }
//
//        protected override void Cast()
//        {
//            RaycastHit hit;
//            if (!RaycastCamera(out hit, LayerMaskHelper.Entity))
//                return;
//
//            if (!InCastRange(hit.point))
//                return;
//
//            SpendMana();
//            UnitCast(GetEntity(hit));
//        }
//
//        protected override void Prepare()
//        {
//        }
//
//        public override void UnitCast(GameObject target)
//        {
//            var damage = new Damage(_damage, DamageType.Magical, Self);
//            var healthable = target.GetComponent<IHealthable>();
//            healthable.TakeDamage(damage);
//        }
//
//        public static bool RaycastCamera(out RaycastHit hitinfo, LayerMaskHelper layerMask = (LayerMaskHelper) 0)
//        {
//            var lm = (int) layerMask;
//            if (lm == 0)
//                lm = (int) (LayerMaskHelper.Entity | LayerMaskHelper.Trigger | LayerMaskHelper.World);
//            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo, lm);
//        }
//
//        public static GameObject GetEntity(RaycastHit hit)
//        {
//            var go = hit.collider.gameObject;
//            var rb = hit.collider.GetComponent<Rigidbody>();
//
//            if (rb != null)
//                go = rb.gameObject;
//
//            var entity = go.GetComponent<Entity>();
//
//
//            if (entity == null)
//                entity = go.GetComponentInChildren<Entity>();
//
//            if (entity == null)
//                entity = go.GetComponentInParent<Entity>();
//
//            if (entity != null)
//                return entity.gameObject;
//            else return null;
//        }
//    }
//}