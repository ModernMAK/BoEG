using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class ArmorableModule : IModule, IArmorable
    {
        private readonly IArmorable _armorable;

        public ArmorableModule(IArmorable armorable)
        {
            _armorable = armorable;
        }
        
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

        public void PreStep(float delta)
        {
            throw new NotImplementedException();
        }

        public void Step(float delta)
        {
            throw new NotImplementedException();
        }

        public void PostStep(float delta)
        {
            throw new NotImplementedException();
        }

        public void PhysicsStep(float delta)
        {
            throw new NotImplementedException();
        }

        public void Spawn()
        {
            throw new NotImplementedException();
        }

        public void Despawn()
        {
            throw new NotImplementedException();
        }

        public void Instantiate()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}