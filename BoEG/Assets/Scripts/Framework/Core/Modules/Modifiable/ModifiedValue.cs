namespace MobaGame.Framework.Core.Modules
{
	public class ModifiedValue : IModifiedValue<float>
	{
        public float Base { get; set; }

        public Modifier Modifier { get; set; }
        public float Bonus => Modifier.Calculate(Base);
        public float Total => Base + Bonus;
    }

}