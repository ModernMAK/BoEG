using Core;
using Modules.Healthable;
using Modules.Magicable;
using UnityEngine;

namespace Modules.Attackerable
{
    public interface IAttackable
    {
        void PrepareAttack(GameObject attacker);
        void RecieveAttack(Damage damage);
        event EndgameEventHandler AttackLaunchedAgainst;
        event DamageEventHandler IncomingAttackLanded;
    }

    public class Attackable : Module, IAttackable
    {
        public Attackable(GameObject self) : base(self)
        {
        }

        public void PrepareAttack(GameObject attacker)
        {
            var args = new EndgameEventArgs(attacker, Self);
            OnAttackLaunchedAgainst(args);
        }

        public void RecieveAttack(Damage damage)
        {
            var args = new DamageEventArgs(Self, damage);
            OnAttackLanded(args);
        }

        private void OnAttackLaunchedAgainst(EndgameEventArgs args)
        {
            if (AttackLaunchedAgainst != null)
                AttackLaunchedAgainst(args);
        }

        public event EndgameEventHandler AttackLaunchedAgainst;

        private void OnAttackLanded(DamageEventArgs args)
        {
            if (IncomingAttackLanded != null)
                IncomingAttackLanded(args);
        }

        public event DamageEventHandler IncomingAttackLanded;
    }

    public delegate void EndgameEventHandler(EndgameEventArgs args);
}