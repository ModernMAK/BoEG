using UnityEngine;

namespace Components.Attackerable
{
    [CreateAssetMenu(menuName = "Component/Attackerable")]
    public class AttackerableAsset : ScriptableObject, IAttackerableData
    {
        [SerializeField] private AttackerableData _data;

        public float BaseDamage
        {
            get { return _data.BaseDamage; }
        }

        public float BaseAttackRange
        {
            get { return _data.BaseAttackRange; }
        }

        public float BaseAttackSpeed
        {
            get { return _data.BaseAttackSpeed; }
        }

        public float GainDamage
        {
            get { return _data.GainDamage; }
        }

        public float GainAttackRange
        {
            get { return _data.GainAttackRange; }
        }

        public float GainAttackSpeed
        {
            get { return _data.GainAttackSpeed; }
        }
    }
}