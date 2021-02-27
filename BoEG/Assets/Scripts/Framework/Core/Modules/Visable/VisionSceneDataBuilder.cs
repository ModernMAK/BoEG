using System;
using Unity.Mathematics;
using UnityEngine;

namespace MobaGame.Assets.Scripts.Framework.Core
{
	public class VisionSceneDataBuilder : MonoBehaviour
	{
		public int3 CellCount;
		public BoxCollider Bound;
		public bool HideGizmos;
		private Vector2 AxisScale => Vector3.Scale(Bound.size,new Vector3(1f/CellCount.x,1f/CellCount.y,1f/CellCount.z));
		private void OnDrawGizmosSelected()
		{
			if (HideGizmos)
				return;
			if (Bound == null)
				return;
			var center = Bound.center;
			var scale = AxisScale;

			var origin = GetOrigin(center, scale, CellCount);
			var r =	new Color(1.0f,	0.5f, 0.5f);
			var g =	new Color(0.5f,	1.0f, 0.5f);
			var b =	new Color(0.5f, 0.5f, 1.0f);

			DrawGrid(origin, right, fwd, Width, Height, r,g,b);
			DrawUp(origin, right * Width, fwd * Height, up, Depth, r,g,b);
		}
		private static Vector3 GetOrigin(Vector3 center, Matrix4x4 transform, Vector3 scale, int3 cell)
		{
			return center - (transform.ri * w + fwd * h + up * d) / 2f;
		}
		private static void DrawGrid(Vector3 origin, Vector3 right, Vector3 fwd, int w, int h, Color xAxis, Color yAxis, Color zAxis)
		{
			//DO NOT NORMALIZE Up/Right
			var c = Gizmos.color;
			Gizmos.color = zAxis;				
			for (var x = 0; x <= w; x++)
			{
				var a = origin + right * x;
				var b = a + fwd * h;
				Gizmos.DrawLine(a, b);
			}
			Gizmos.color = xAxis;
			for (var y = 0; y <= h; y++)
			{

				var a = origin + fwd * y;
				var b = a + right * w;
				Gizmos.DrawLine(a, b);
			}
			Gizmos.color = c;
		}
		private static void DrawUp(Vector3 origin, Vector3 right, Vector3 fwd, Vector3 up, int d, Color xAxis, Color yAxis, Color zAxis)
		{
			var o = Gizmos.color;
			Gizmos.color = yAxis;
			Gizmos.DrawLine(origin, origin + up * d);
			for (var z = 0; z <= d; z++)
			{
				var b = origin + up * z;
				var a = b + right;
				var c = b + fwd;
				Gizmos.color = xAxis;
				Gizmos.DrawLine(a, b);
				Gizmos.color = zAxis;
				Gizmos.DrawLine(b, c);
			}
			Gizmos.color = o;
		}
		[Obsolete]
		public VisionWorldMapper Build() => new VisionWorldMapper(Bound.bounds, Bound.transform.rotation, new int3(Width, Depth, Height));
	}
}