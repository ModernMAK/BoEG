using System;
using Modules.Abilityable.Ability;
using UnityEngine;

namespace Modules.Abilityable
{
    [Serializable]
    public class AbilityCast : IAbilityCast
    {

        public bool Preparing
        {
            get;
            protected set;
        }


        public virtual bool CanCast
        {
            get { return false; /*IsLeveled && HasEnoughMana && OffCooldown && !IsChanneling;*/ }
        }

        //
        public virtual void Prepare()
        {
        }

        public virtual void CancelPrepare()
        {
        }

        public virtual void Cast()
        {
        }

        public  void Trigger()
        {
            if (CanCast && !Preparing)
            {
                Preparing = true;
                Prepare();
            }
            else if (Preparing)
            {
                Preparing = false;
                if (CanCast)
                    Cast();
                CancelPrepare();
            }
        }

        //Some spells rely on other spells to function,
        //If this spell needs to be ground casted, this will be called.
        //Despite being defined, there is no garuntee the cast will be Implimented
        //This method should not Spend Mana, Begin Channeling The Spell, Raise A Spell Cast Event, Or Put The Spell On Cooldown
        public virtual void GroundCast(Vector3 point)
        {
            throw new NotImplementedException();
        }

        //Some spells rely on other spells to function,
        //If this spell needs to be unit casted, this will be called.
        //Despite being defined, there is no garuntee the cast will be Implimented
        //This method should not Spend Mana, Begin Channeling The Spell, Raise A Spell Cast Event, Or Put The Spell On Cooldown
        public virtual void UnitCast(GameObject target)
        {
            throw new NotImplementedException();
        }

    }
}