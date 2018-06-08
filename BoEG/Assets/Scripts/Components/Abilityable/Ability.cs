using UnityEngine;


namespace Components.Abilityable
{
    public abstract class Ability : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public float Cooldown;
        public int Level;
        public AbilityCast CastType;
        public abstract void Initialize(GameObject go);

        protected struct CastData
        {
            public CastData(AbilityCast castType) : this()
            {
                CastType = castType;
            }

            public CastData(Vector3 target) : this(AbilityCast.TargetPoint)
            {
                TargetPoint = target;
            }

            public CastData(Ray ray) : this(AbilityCast.TargetRay)
            {
                TargetRay = ray;
            }

            public CastData(GameObject target) : this(AbilityCast.TargetUnit)
            {
                TargetUnit = target;
            }

            public AbilityCast CastType { get; private set; }
            public Vector3 TargetPoint { get; private set; }
            public Ray TargetRay { get; private set; }
            public GameObject TargetUnit { get; private set; }
        }
//
//        public bool CanNoTargetCast
//        {
//            get { return CastType.HasFlag(AbilityCast.NoTarget); }
//        }
//
//        public void NoTargetCast()
//        {
//            Trigger(new CastData(AbilityCast.NoTarget));
//        }
//
//        public bool CanTargetPointCast
//        {
//            get { return CastType.HasFlag(AbilityCast.TargetPoint); }
//        }
//
//        public virtual void TargetPointCast(Vector3 target)
//        {
//            Trigger(new CastData(target));
//        }
//
//        public bool CanTargetRayCast
//        {
//            get { return CastType.HasFlag(AbilityCast.TargetRay); }
//        }
//
//        public virtual void TargetRayCast(Ray ray)
//        {
//            Trigger(new CastData(ray));
//        }
//
//        public bool CanTargetUnitCast
//        {
//            get { return CastType.HasFlag(AbilityCast.TargetUnit); }
//        }
//
//        public void TargetUnitCast(GameObject unit)
//        {
//            Trigger(new CastData(unit));
//        }

        protected abstract void Trigger();

        protected abstract void Trigger(CastData castData);
//        public abstract void Upgrade();
//        
//        public void NoTargetCast()
//        {
//            
//        }
//
//        public void TargetPointCast(Vector3 target)
//        {
//            
//        }
//
//        public void TargetRay(Ray ray)
//        {
//            
//        }
//
//        public void TargetUnit(Unit unit)
//        {
//            
//        }
    }
}