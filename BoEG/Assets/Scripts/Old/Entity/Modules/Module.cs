using Old.Entity.Core;
using UnityEngine.Networking;

namespace Old.Entity.Modules
{
    public class Module : NetworkBehaviour
    {
        private void Awake()
        {
            Initialize();
        }
        private void Update()
        {
            Tick();
        }
        private void LateUpdate()
        {
            PostTick();
            PreTick();
        }
        public sealed override bool OnSerialize(NetworkWriter writer, bool initState)
        {
            return Serialize(writer, initState);
        }
        public sealed override void OnDeserialize(NetworkReader reader, bool initState)
        {
            Deserialize(reader, initState);
        }

        
        protected virtual void Initialize()
        {
        }
        
        protected virtual void PreTick()
        {
        }

        protected virtual void Tick()
        {
        }

        protected virtual void PostTick()
        {
        }

        protected virtual bool Serialize(NetworkWriter writer, bool initState)
        {
            return false;
        }


        protected virtual void Deserialize(NetworkReader reader, bool initState)
        {
        }

        protected T GetData<T>()
        {
            var dataGroup = GetComponent<IDataGroup>();
            return (dataGroup != null) ? dataGroup.GetData<T>() : default(T);
        }
    }
}