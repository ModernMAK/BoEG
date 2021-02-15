using UnityEngine;

namespace MobaGame.Framework.Core.Trigger
{
	public class TriggerHelper<T> where T : Collider
    {
        public T Collider { get; set; }
        public Trigger Trigger { get; set; }
    }
}