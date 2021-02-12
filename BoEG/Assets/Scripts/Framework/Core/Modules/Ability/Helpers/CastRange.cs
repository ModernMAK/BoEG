using MobaGame.Framework.Core;
using UnityEngine;

namespace MobaGame.Assets.Scripts.Framework.Core.Modules.Ability.Helpers
{
	public class CastRange
	{
		public CastRange(Transform self)
		{
			Transform = self;
			MaxDistance = ushort.MaxValue;//Arbitrary big value
		}
		private Transform Transform { get; }
		private Vector3 Position => Transform.position;


		public float MaxDistance { get; set; }
		private float MaxDistanceSqr => MaxDistance * MaxDistance;
		private static bool DistanceCheck(Vector3 a, Vector3 b, float maxSqr)
		{
			var delta = a - b;
			var deltaDistanceSqr = delta.sqrMagnitude;
			return deltaDistanceSqr <= maxSqr ;
		}
		public bool InRange(Vector3 point) => DistanceCheck(Position, point, MaxDistanceSqr);
		public bool InRange(Transform transform) => InRange(transform.position);
		public bool InRange(Actor actor) => InRange(actor.transform);
	}
}