//using Modules.Healthable;
//using Modules.Magicable;
//using UnityEngine;
//
//namespace Modules.Abilityable
//{
//    public class LazyModuleHelper
//    {
//        public LazyModuleHelper(GameObject go)
//        {
//            _healthable = new LazyModule<IHealthable>(go);
//            _magicable = new LazyModule<IMagicable>(go);
//        }
//        
//
//        private class LazyModule<T> : Util.Lazy<T>
//        {
//            public LazyModule(GameObject go) : base(go.GetComponent<T>)
//            {
//                
//            }
//        }
//        
//        private readonly LazyModule<IHealthable> _healthable;
//        public IHealthable Healthable
//        {
//            get { return _healthable.Instance; }
//        }
//        private readonly LazyModule<IMagicable> _magicable;
//
//        public IMagicable Magicable
//        {
//            get { return _magicable.Instance; }
//        }
//    }
//}
//Why do i need this? I dont

namespace Modules.Abilityable.Ability
{
}