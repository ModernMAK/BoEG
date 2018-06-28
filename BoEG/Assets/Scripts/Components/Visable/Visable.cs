namespace Components.Visable
{
    public class Visable : FullModule, IVisable
    {
        public Visable(IVisableData data)
        {
            _data = data;
        }

        private readonly IVisableData _data;

        public bool IsInvisible
        {
            get { return _data.HasInvisability; }
        }

        public bool IsHidden
        {
            get { return _data.HasHidden; }
        }

        public bool IsSpotted
        {
            get { return _data.HasSpotted; }
        }
    }
}