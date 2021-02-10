using System;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Types;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MobaGame.Framework.Logging
{
    //TODO a better solution
    public static class CombatLog
    {
        public static string FormatActor(string actor) => RichText.Bold(RichText.Yellow(actor));

        public static string FormatDamage(Damage damage) =>
            RichText.Bold($"{damage.Value} {FormatDamage(damage.Type)} {RichText.Italic(damage.Flags.ToString())}");
        public static string FormatDamage(DamageType damage)
        {
            switch (damage) 
            {
                case DamageType.Physical:
                    return RichText.Red("Physical");
                case DamageType.Magical:
                    return RichText.Navy("Magical");
                case DamageType.Pure:
                    return RichText.White("Pure");
                case DamageType.Modification:
                    return RichText.Purple("Modification");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        public static string FormatHeal(string str) => RichText.Green(str);
    }
}