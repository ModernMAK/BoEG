namespace Framework.Core.Modules
{
    public abstract class ValueBuffCalculator<T> : BuffCalculator<T>, IValueBuffCalculator
    {

        protected ValueBuffCalculator(IBuffList<T> buffList) : base(buffList)
        {
        }
        public float Value { get; protected set; }
    }    
    
}