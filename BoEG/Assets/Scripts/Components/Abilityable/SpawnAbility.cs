using UnityEngine;

namespace Components.Abilityable
{
    [CreateAssetMenu(fileName = "SpawnAbility.asset", menuName = "Ability/SpawnAbility")]
    public class SpawnAbility : Ability
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _amount;

        private Transform _transform;

        public override void Initialize(GameObject go)
        {
            _transform = go.transform;
        }

        public override void Trigger()
        {
            AbiltiyUtility.Spawn(_transform, _prefab, _amount);
        }
    }
}
