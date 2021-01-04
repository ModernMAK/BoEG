namespace MobaGame.Framework.Types
{
    public readonly struct SourcedDamage<TSource>
    {
        public SourcedDamage(Damage damage, TSource source)
        {
            Damage = damage;
            Source = source;
        }

        public Damage Damage { get; }
        public TSource Source { get; }

        public SourcedDamage<TSource> SetDamage(Damage damage) => new SourcedDamage<TSource>(damage, Source);
        public SourcedDamage<TSource> SetSource(TSource source) => new SourcedDamage<TSource>(Damage, source);
    }
}