using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    [AddComponentMenu("EndGame/Components/Armorable")]
    [DisallowMultipleComponent]
    public class ArmorableComponent : MonoBehaviour, IComponent<IArmorable>, IArmorable
    {
        private IArmorable _armorable;
        public Armor Physical => _armorable.Physical;
        public Armor Magical => _armorable.Magical;
        public Damage ResistDamage(Damage damage)
        {
            return _armorable.ResistDamage(damage);
        }
        public float CalculateReduction(Damage damage)
        {
            return _armorable.CalculateReduction(damage);
        }
        public event EventHandler<ArmorableEventArgs> Resisted
        {
            add => _armorable.Resisted += value;
            remove => _armorable.Resisted -= value;
        }
        public event EventHandler<ArmorableEventArgs> Resisting
        {
            add => _armorable.Resisting += value;
            remove => _armorable.Resisting -= value;
        }
        public void Initialize(IArmorable module)
        {
            _armorable = module;
        }
    }
}