using System;

namespace MobaGame.Framework.Core.Modules
{
	/// <summary>
	/// A Self Contained Modifier List & Modified Value
	/// 
	/// </summary>
	/// <typeparam name="TModifier"></typeparam>
	public class ModifiedValueBoilerplate<TModifier> : IListener<IModifiable> where TModifier : IModifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <remarks>List defaults to a Mixed Modifier List.</remarks>
        public ModifiedValueBoilerplate(Func<TModifier, FloatModifier> func) : this(new MixedModifierList<TModifier>(), func) { }
        public ModifiedValueBoilerplate(ModifierList<TModifier> list, Func<TModifier,FloatModifier> func) 
		{
            Value = new ModifiedValue();
            List = list;
            GetModifier = func;
            List.ListChanged += RecalculateModifier;
            
		}
        private Func<TModifier, FloatModifier> GetModifier { get; }
        public ModifiedValue Value { get; }
        public ModifierList<TModifier> List { get; }

		void RecalculateModifier(object sender, EventArgs e) => Value.Modifier = List.SumModifiers(GetModifier);

		public void Register(IModifiable source) => List.Register(source);
		

		public void Unregister(IModifiable source) => List.Unregister(source);
    }
}