namespace Old.Entity.Modules.Visable
{
    public class Visable : FullModule, IVisable
    {
        protected override  void Initialize()
        {
            _data = GetData<IVisableData>();
        }

        private IVisableData _data;

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