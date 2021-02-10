using MobaGame.Framework.Types;
using System;
using MobaGame.Framework.Logging;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Healable : ActorModule, IHealable
    {
        private readonly IHealthable _healthable;

        public Healable(Actor self, IHealthable healthable) : base(self)
        {
            _healthable = healthable;
        }

        public event EventHandler<SourcedHeal> Healing;
        public event EventHandler<SourcedHeal> Healed;
        public event EventHandler<ChangableEventArgs<SourcedHeal>> HealingModifiers;

        public void Heal(SourcedHeal heal)
        {
            HealableLog.LogHealing(heal,Actor);
            OnHealing(heal);
            heal = CalculateHealingModifiers(heal);
            _healthable.Value += heal.Value;
            HealableLog.LogHealed(heal,Actor);
            OnHealed(heal);
        }

        protected virtual void OnHealing(SourcedHeal heal) => Healing?.Invoke(this, heal);
        protected virtual void OnHealed(SourcedHeal heal) => Healed?.Invoke(this, heal);

        protected virtual SourcedHeal CalculateHealingModifiers(SourcedHeal heal) =>
            HealingModifiers.CalculateChange(this, heal);
    }

    public static class RichText
    {
        public static string Tag(string str, string tag) => $"<{tag}>{str}</{tag}>";
        public static string Tag(string str, string tag, object value) => $"<{tag}={value}>{str}</{tag}>";
        #region Colors
        public static string Color(string str, string color) => Tag(str,"color",color);

        public static string Color(string str, Color color)
        {
            var colorStr = EncodeColor(color);
            return Color(str, colorStr);
        }

        public static string Color(string str, Color32 color)
        {
            var colorStr = EncodeColor(color);
            return Color(str, colorStr);
        }

        private static string EncodeColor(Color color) => $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        private static string EncodeColor(Color32 color) => $"#{ColorUtility.ToHtmlStringRGBA(color)}";

        #region Literals
        public static string Aqua(string str) => Color(str, "aqua");
        public static string Black(string str) => Color(str, "black");
        public static string Blue(string str) => Color(str, "blue");
        public static string Brown(string str) => Color(str, "brown");
        public static string Cyan(string str) => Color(str, "cyan");
        public static string DarkBlue(string str) => Color(str, "darkblue");
        public static string Fuchsia(string str) => Color(str, "fuchsia");
        public static string Green(string str) => Color(str, "green");
        public static string Grey(string str) => Color(str, "grey");
        public static string LightBlue(string str) => Color(str, "lightblue");
        public static string Lime(string str) => Color(str, "lime");
        public static string Magenta(string str) => Color(str, "magenta");
        public static string Maroon(string str) => Color(str, "maroon");
        public static string Navy(string str) => Color(str, "navy");
        public static string Olive(string str) => Color(str, "olive");
        public static string Orange(string str) => Color(str, "orange");
        public static string Purple(string str) => Color(str, "purple");
        public static string Red(string str) => Color(str, "red");
        public static string Silver(string str) => Color(str, "silver");
        public static string Teal(string str) => Color(str, "teal");
        public static string White(string str) => Color(str, "white");
        public static string Yellow(string str) => Color(str, "yellow");
        #endregion

        #endregion

        public static string Size(string str, int size) => Tag(str,"size",size);
        public static string Bold(string str) => Tag(str,"b");
        public static string Italic(string str) => Tag(str,"i");
        public static string Material(string str, int material) => Tag(str,"material",material);
        public static string Quad(string str, int material, int size, float x, float y, float width, float height) =>
            $"<quad material={material} size={size} x={x} y={y} width={width} height={height}>{str}</quad>";
    }

    public static class HealableLog
    {
        
        public static void LogHealing(SourcedHeal heal, Actor actor)
        {
            var msg =
                $"{CombatLog.FormatActor(heal.Source.name)} Healing {CombatLog.FormatActor(actor.name)} for {CombatLog.FormatHeal(heal.Value.ToString())}";
            CombatLog.Log(msg);
        }

        public static void LogHealed(SourcedHeal heal, Actor actor)
        {
            var msg =
                $"{CombatLog.FormatActor(heal.Source.name)} Healed {CombatLog.FormatActor(actor.name)} for {CombatLog.FormatHeal(heal.Value.ToString())}";
            CombatLog.Log(msg);
        }
    }
}