using System;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class ArmorableModule : MonoBehaviour, IArmorable, IInitializable<IArmorableData>
    {
        private Armorable _armorable;
        public Armor Physical => _armorable.Physical;
        public Armor Magical => _armorable.Magical;

        public virtual Damage ResistDamage(Damage damage) => _armorable.ResistDamage(damage);

        public float CalculateReduction(Damage damage) => _armorable.CalculateReduction(damage);

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

        public void Initialize(IArmorableData data) => _armorable.Initialize(data);
    }
}