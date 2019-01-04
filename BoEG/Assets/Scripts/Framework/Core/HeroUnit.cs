using Framework.Types;
using UnityEngine.Serialization;

namespace Framework.Core
{
    //Creep
    //[ ] Armorable
    //[x] Attackerable
    //[/] Commandable
    //    -> 'Director' Issues Commands 
    //[x] Damagable
    //[x] Healthable
    //[/] Magicable
    //    -> Not All Creeps Are Magicable
    //[x] Targetable


    public class HeroUnit : Unit
    {
        protected override void StartLogic()
        {
            base.StartLogic();
            Instanitate();
            Spawn();
        }
    }
}