using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MobaGame.Assets.Scripts.Framework.Core
{
	public static class LineOfSightUtil
	{
		public static void Swap<T>(ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}
		public static IEnumerable<int2> BresenhamLine(int2 a, int2 b) => BresenhamLine(a.x, a.y, b.x, b.y);
		public static IEnumerable<int2> BresenhamLine(int x1, int y1, int x2, int y2)
		{
			var dy = Math.Abs(y2 - y1);
			var dx = Mathf.Abs(x2 - x1);
			var stepX = (x1 < x2) ? 1 : -1;
			var stepY = (y1 < y2) ? 1 : -1;
			var error = (dx > dy ? dx : -dy) / 2;

			yield return new int2(x1, y1);
			while (true)
			{
				if (x2 == x1 && y2 == y1)
					break;
				var errorNow = error;
				if (errorNow > -dx)
				{
					error -= dy;
					x1 += stepX;
				}
				if (errorNow < dy)
				{
					error += dx;
					y1 += stepY;
				}
				yield return new int2(x1, y1);
			}

			//var steep = dy > Mathf.Abs(dx);
			//if(steep)
			//{
			//	Swap(ref x1, ref y1);
			//	Swap(ref x2, ref y2);
			//}
			//if(x1 > x2)
			//{
			//	Swap(ref x1, ref x2);
			//	Swap(ref y1, ref y2);
			//}
			//var error = dx / 2;
			//var yStep = y1 < y2 ? 1 : -1;
	
			//var y = y1;
			//for(var x = x1; x <= x2; x++)
			//{
			//	if (steep)
			//		yield return new int2(y, x);
			//	else
			//		yield return new int2(x, y);
			//	error -= dy;
			//	if(error < 0)
			//	{
			//		y += yStep;
			//		error += dx;
			//	}
			//}
			
		}
		private static bool DistanceCheck(int2 a, int2 b, int sqrMag)
		{
			var delta = a - b;
			var deltaSqrMag = delta.x * delta.x + delta.y * delta.y;
			return deltaSqrMag <= sqrMag;
		}
		private static bool DistanceCheck(int2 a, int2 b, float sqrMag)
		{
			var delta = a - b;
			var deltaSqrMag = delta.x * delta.x + delta.y * delta.y;
			return deltaSqrMag <= sqrMag;
		}
		public static IEnumerable<int2> NaiveCircleFill(int2 point, int radius, bool applyShift = false) => NaiveCircleFill(point, (float)radius, applyShift);
		public static IEnumerable<int2> NaiveCircleFill(int x, int y, int r, bool applyShift = false) => NaiveCircleFill(new int2(x, y), r, applyShift);
		public static IEnumerable<int2> NaiveCircleFill(int2 point, float radius, bool applyShift = false)
		{
			if (applyShift)
				radius += 0.48f;
			var intRadius = Mathf.CeilToInt(radius);
			var sqrRadius = radius * radius;
			var deltaRadius = new int2(intRadius, intRadius);
			var min = point - deltaRadius;
			var max = point + deltaRadius;

			for (var x = min.x; x <= max.x; x++)
				for (var y = min.y; y <= max.y; y++)
				{
					var testPoint = new int2(x, y);
					if (!DistanceCheck(point, testPoint, sqrRadius))
						continue;
					yield return testPoint;
				}
			
		}
		public static IEnumerable<int2> NaiveCircleFill(int x, int y, float r, bool applyShift = false) => NaiveCircleFill(new int2(x, y), r, applyShift);
		//Points are not ordered
		public static IEnumerable<int2> BresenhamCircleOutline(int2 point, int radius)
		{
			var x = radius;
			var y = 0;

			yield return point + new int2(x, y);

			if (radius > 0)
			{
				yield return point + new int2(-x, y);
				yield return point + new int2(y, x);
				yield return point + new int2(y, -x);
			}
			var P = 1 - radius;
			while (x > y)
			{
				y++;
				if (P <= 0)
				{
					P = P + 2 * y + 1;
				}
				else
				{
					x--;
					P = P + 2 * y - 2 * x + 1;
				}
				if (x < y)
					break;


				yield return point + new int2(x, y);
				yield return point + new int2(x, -y);
				yield return point + new int2(-x, -y);
				yield return point + new int2(-x, y);
				if(x != y)
				{
					yield return point + new int2(y, x);
					yield return point + new int2(y, -x);
					yield return point + new int2(-y, -x);
					yield return point + new int2(-y, x);
				}
			}
		}
		public static IEnumerable<int2> BresenhamCircleOutline(int2 point, float radius)
		{
			var intRadius = Mathf.CeilToInt(radius);
			return BresenhamCircleOutline(point, intRadius);
		}
		public static IEnumerable<int2> BresenhamCircleOutline(int x, int y, float radius) => BresenhamCircleOutline(new int2(x, y), radius);
		public static IEnumerable<int2> BresenhamCircleOutline(int x, int y, int radius) => BresenhamCircleOutline(new int2(x, y), radius);

		//Draws lines horizontally
		public static IEnumerable<int2> BresenhamCircleFill(int2 point, int radius)
		{
			var x = radius;
			var y = 0;

			if (radius == 0)
			{
				yield return point + new int2(x, y);
			}
			if (radius > 0)
			{
				for (var i = -x; i <= x; i++)
					yield return point + new int2(i, y);
				//The two vertical points should always be on a line 
				//	A Bresnam circle of radius 1 is a square
				//	Therefore; when filling, we exclude them
					
				//	//Two vertical points; we don't know if they are lines or points until later
				//	yield return point + new int2(y, x);
				//	yield return point + new int2(y, -x);
			}
			var P = 1 - radius;
			while (x > y)
			{
				y++;
				if (P <= 0)
				{
					P = P + 2 * y + 1;
				}
				else
				{
					x--;
					P = P + 2 * y - 2 * x + 1;
				}


				if (x == y)
				{
					for (var i = -x; i <= x; i++)
					{
						yield return point + new int2(i, y);
						yield return point + new int2(i, -y);
					}
				}
				else
				{
					for (var i = -x; i <= x; i++)
					{
						yield return point + new int2(i, y);
						yield return point + new int2(i, -y);
					}
					for(var i = -y; i <= y; i++)
					{
						yield return point + new int2(i, x);
						yield return point + new int2(i, -x);
					}
				}
			}
		}
		public static IEnumerable<int2> BresenhamCircleFill(int2 point, float radius)
		{
			var intRadius = Mathf.CeilToInt(radius);
			return BresenhamCircleFill(point, intRadius);
		}


	}
	[Serializable]
	[Obsolete]
	public class VisionWorldMapper 
	{
		public VisionWorldMapper(Bounds bound, Quaternion rotation, int3 divisions)
		{
			_boundary = bound;
			_rotation = rotation;
			_divisions = divisions;
		}
		[SerializeField]
		private Bounds _boundary;
		[SerializeField]
		private Quaternion _rotation;
		[SerializeField]
		private int3 _divisions;
		private Bounds Boundary => _boundary;
		public Quaternion Rotation => _rotation;
		public Vector3 Size => Boundary.size;
		public Vector3 Origin => Boundary.center - Boundary.extents;
		public int3 Divisions => _divisions;
		public Vector3 DivisionSize => new Vector3(Size.x / Divisions.x, Size.y / Divisions.y, Size.z / Divisions.z);		

		public Vector3 ConvertToWorldPoint(int3 point, bool center = false)
		{
			var divSize = DivisionSize;
			var worldPos = Vector3.Scale(divSize, (float3)point);
			if (center)
				worldPos += divSize / 2f;
			worldPos = Rotation * worldPos;
			worldPos += Origin;
			return worldPos;
		}

		public Vector3 ConvertToLocalPoint(Vector3 point) => Quaternion.Inverse(Rotation) * point - Origin;
		public Vector3 ConvertToBoundPoint(Vector3 point) {

			var localSpace = ConvertToLocalPoint(point);
			var invSize = new Vector3(
				1f / Size.x,
				1f / Size.y,
				1f / Size.z
			);
			var boundSpace = Vector3.Scale(localSpace, invSize);
			return boundSpace;
		}
		public int3 ConvertToDivisionPoint(Vector3 point)
		{
			var boundSpace = ConvertToBoundPoint(point);
			var divSpace = Vector3.Scale(boundSpace, (float3)Divisions);
			//Truncate divspace
			//Int3
			var truncDivSpace = new int3(
				Mathf.FloorToInt(divSpace.x),
				Mathf.FloorToInt(divSpace.y),
				Mathf.FloorToInt(divSpace.z)
			);
			return truncDivSpace;
		}
		public bool InBound(Vector3 point)
		{
			var bound = ConvertToBoundPoint(point);
			return (bound.x >= 0f && bound.x <= 1f) &&
				(bound.y >= 0f && bound.y <= 1f) &&
				(bound.z >= 0f && bound.z <= 1f);
		}
		public int3 ConvertToGridPoint(Vector3 point)
		{
			var divSpace = ConvertToDivisionPoint(point);
			//Fix cases where the divspace is On N
			//Int3
			var gridSpace = new int3(
				Mathf.Clamp(divSpace.x, 0, Divisions.x-1),
				Mathf.Clamp(divSpace.y, 0, Divisions.y-1),
				Mathf.Clamp(divSpace.z, 0, Divisions.z-1)
			);
			return gridSpace;



		}

	}
}