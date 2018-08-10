using UnityEngine;
using UnityEngine.Networking;

namespace Old.Entity.Core
{
    public class Entity : NetworkBehaviour, IDataGroup
    {
//        /// <summary>
//        /// This is called only once
//        /// Recommended place to create instances of Modules
//        /// </summary>
//        protected virtual void Setup()
//        {
//        }
//
//        /// <summary>
//        /// This is called ???
//        /// </summary>
//        private void Initialize()
//        {
//            foreach (var module in Modules)
//            {
//                module.Initialize(this);
//            }
//        }
//        
//        /// <summary>
//        /// Dedicated time at the start of the tick
//        /// Used for logic before something has happened
//        /// </summary>
//        protected virtual void PreTick()
//        {
//            foreach (var module in Modules)
//            {
//                module.PreTick();
//            }
//        }
//        /// <summary>
//        /// Dedicated time at the start of the tick
//        /// Used for logic before something has happened
//        /// </summary>
//        protected virtual void Tick()
//        {
//            foreach (var module in Modules)
//            {
//                module.Tick();
//            }
//        }
//
//        protected virtual void PostTick()
//        {
//            foreach (var module in Modules)
//            {
//                module.PostTick();
//            }
//        }
//
//        /// <summary>
//        /// A 'Tick" to check if we need to serialize
//        /// </summary>
//        private void SerializationTick()
//        {
//            //IF we already know that we need to serialize, then skip
//            if(syncVarDirtyBits != 0)
//                return;            
//            //Otherwise, check if we need to serialize
//            foreach (var module in Modules)
//            {
//                if(module.IsDirty())
//                    SetDirtyBit(0x01);
//            }
//        }
//        
//        public T GetModule<T>()
//        {
//            foreach (var module in Modules)
//            {
//                if (module is T)
//                    return (T) module;
//            }
//            return default(T);
//        }
//        public T[] GetModules<T>()
//        {
//            return Modules.OfType<T>().ToArray();
//        }
//            
//        public sealed override bool OnSerialize(NetworkWriter writer, bool initState)
//        {
//            return Modules.Aggregate(false, (current, module) => current | module.Serialize(writer, initState));
//        }
//        public sealed override void OnDeserialize(NetworkReader reader, bool initState)
//        {
//            foreach (var module in Modules)
//            {
//                module.Deserialize(reader, initState);
//            }
//        }
//
//        /// <summary>
//        /// Registered Modules
//        /// All modules returned are Ticked and initialized automatically
//        /// </summary>
//        protected virtual IEnumerable<IModule> Modules
//        {
//            get { return Enumerable.Empty<IModule>(); }
//        }

//        private void Awake()
//        {
//            Setup();
//        }
//
//        private void Start()
//        {
//            Initialize();
//        }
//
//        private void Update()
//        {
//            Tick();
//        }
//
//        private void LateUpdate()
//        {
//            PostTick();
//            SerializationTick();
//            PreTick();
//        }
        
        
        [SerializeField] private DataGroupAsset _data;


        public T GetData<T>()
        {
            return _data.GetData<T>();
        }
    }
}