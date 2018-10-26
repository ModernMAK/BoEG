using Core;
using Modules.Healthable;
using Modules.Magicable;
using UnityEngine;

namespace Modules.Attackerable
{
    public interface IAttackable
    {
        void TargetForAttack(GameObject attacker);
        void RecieveAttack(Damage damage);
        event EndgameEventHandler TargetedForAttack;
        event DamageEventHandler IncomingAttackLanded;
    }

    public class Attackable : Module, IAttackable
    {
        public Attackable(GameObject self) : base(self)
        {
        }

        public void TargetForAttack(GameObject attacker)
        {
            var args = new EndgameEventArgs(attacker, Self);
            OnTargetedForAttack(args);
        }

        public void RecieveAttack(Damage damage)
        {
            var args = new DamageEventArgs(Self, damage);
            OnAttackLanded(args);
        }

        private void OnTargetedForAttack(EndgameEventArgs args)
        {
            if (TargetedForAttack != null)
                TargetedForAttack(args);
        }

        public event EndgameEventHandler TargetedForAttack;

        private void OnAttackLanded(DamageEventArgs args)
        {
            if (IncomingAttackLanded != null)
                IncomingAttackLanded(args);
        }

        public event DamageEventHandler IncomingAttackLanded;
    }

    public delegate void EndgameEventHandler(EndgameEventArgs args);
}