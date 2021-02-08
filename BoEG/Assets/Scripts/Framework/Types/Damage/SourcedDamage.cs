using MobaGame.Framework.Core;

namespace MobaGame.Framework.Types
{
    public readonly struct SourcedDamage : ISourcedValue<Actor, Damage>
    {
        public SourcedDamage(Actor source, Damage value)
        {
            Source = source;
            Value = value;
        }

        public Actor Source { get; }

        public Damage Value { get; }

        public SourcedDamage SetActor(Actor actor) => new SourcedDamage(actor, Value);
        public SourcedDamage SetDamage(Damage damage) => new SourcedDamage(Source, damage);
    }
}