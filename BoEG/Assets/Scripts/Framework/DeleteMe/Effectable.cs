using System;

namespace MobaGame.Framework.DeleteMe
{
    public class Effectable : IEffectable
    {
        public void ApplyEffect(Effect fx)
        {
            throw new NotImplementedException();
        }

        public void RemoveEffect(Effect fx)
        {
            throw new NotImplementedException();
        }
    }

    // public class Effect
    // {
    // }

    public interface IEffectable
    {
        void ApplyEffect(Effect fx);
        void RemoveEffect(Effect fx);
    }

//    public class Effectable : Module
//    {
//        protected Effectable(GameObject self) : base(self)
//        {
//        }
//        public void ApplyEffect(GameObject source, IEffect fx)
//        {
////            fx.Initialize(source,Self);
//        }
//        public void RemoveEffect(IEffect fx)
//        {
//            
////            fx.Terminate();
//            
//        }
//
//    }
}