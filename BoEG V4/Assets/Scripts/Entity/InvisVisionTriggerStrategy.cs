using Entity;
using UnityEngine;

public class InvisVisionTriggerStrategy : AreaOfEffectStrategy
{
    public InvisVisionTriggerStrategy(Component owner) : this(owner.gameObject)
    {
    }

    public InvisVisionTriggerStrategy(GameObject owner)
    {
        Owner = owner;
    }

    public GameObject Owner { get; private set; }

    public Teamable Teamable
    {
        get { return Owner.GetComponent<Teamable>(); }
    }

    public override bool ShouldEnter(GameObject go)
    {
        var obs = go.GetComponent<Observable>();
        var teamable = go.GetComponent<Teamable>();
        return obs != null && !Teamable.IsAlly(teamable) && base.ShouldEnter(go);
    }

    public override void Enter(GameObject go)
    {
        base.Enter(go);
    }
}