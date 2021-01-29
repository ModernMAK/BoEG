namespace MobaGame.Framework.Core.Modules
{
	public class ModifiedValue : IModifiedValue<float>
	{
		
		public ModifiedValue(float baseValue = default, Modifier modifier = default)
		{
			this.Base = baseValue;
			this.Modifier = modifier;
		}

		public float Base { get; set; }

        public Modifier Modifier { get; set; }
        public float Bonus => Modifier.Calculate(Base);
        public float Total => Base + Bonus;

    }

}