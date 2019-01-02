namespace Framework.Types
{
    public class ConstantProc : RandomProc
    {
        public ConstantProc(float chance)
        {
            _chance = chance;
        }

        private readonly float _chance;
//        public float Chance
//        {
//            get { return _chance; }
//        }

//        public void SetChace(float chance)
//        {
//            Chance = chance;
//        }
        public override bool Proc()
        {
            return Random.NextDouble() <= _chance;
        }
    }
}