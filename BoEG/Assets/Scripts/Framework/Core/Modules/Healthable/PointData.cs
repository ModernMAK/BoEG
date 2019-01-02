namespace Framework.Core.Modules
{
    public struct PointData
    {
        public PointData(PointData data) : this(data.Capacity, data.Generation, data.Percentage)
        {
        }

        public PointData(float capacity, float generation = 0f, float percentage = 1f) : this()
        {
            Capacity = capacity;
            Generation = generation;
            Percentage = percentage;
        }

        public float Percentage { get; private set; }

        public float Points
        {
            get { return Percentage * Capacity; }
            private set { Percentage = value / Capacity; }
        }

        public float Capacity { get; private set; }
        public float Generation { get; private set; }

        public PointData SetPercentage(float value)
        {
            return new PointData(this)
            {
                Percentage = value
            };
        }

        public PointData SetCapacity(float value)
        {
            return new PointData(this)
            {
                Capacity = value
            };
        }

        public PointData SetGeneration(float value)
        {
            return new PointData(this)
            {
                Generation = value
            };
        }

        public PointData SetPoints(float value)
        {
            return new PointData(this)
            {
                Points = value
            };
        }
    }
}