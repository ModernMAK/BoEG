using Modules.Abilityable;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(fileName = "FlameWitch_Ignite",menuName = "Ability/FlameWitch/Ignite")]
    public class Ignite : Ability
    {
        [SerializeField] private float _manaCost = 100f;
        [SerializeField] private float _manaSteal = 50f;
        [SerializeField] private float _manaStealSearchRadius = 5f;
        [SerializeField] private float _castRange = 5f;	
        
        
        public override void Initialize(GameObject go)
        {
            throw new System.NotImplementedException();
        }

        public override void Terminate()
        {
            throw new System.NotImplementedException();
        }

        public override void Trigger()
        {
            throw new System.NotImplementedException();
        }
    }
}