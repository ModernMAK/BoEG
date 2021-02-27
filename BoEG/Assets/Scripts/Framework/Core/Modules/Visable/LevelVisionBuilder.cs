using MobaGame.Framework.Types;
using Unity.Mathematics;
using UnityEngine;


namespace MobaGame
{
	public static class LevelVisionBuilder
	{


		public static LevelVisionData CreateVisionData(Vector3 origin, Quaternion rotation, Vector3 cellSize, int3 cellCount)
		{
			var asset = LevelVisionData.Create(origin, rotation, cellSize, cellCount);
			BuildHeightGrid(asset);
			return asset;
		}
		public static void BuildHeightGrid(LevelVisionData levelVisionData)
		{
			var cellHeight = levelVisionData.CellCount.y;
			var toWorldMatrix = levelVisionData.CellToWorld;
			var grid = levelVisionData.Grid;
			var cellHalfSize = levelVisionData.CellSize / 2f;
			var cellOrientation = levelVisionData.CellOrientatoin;
			const int layerMask = (int)LayerMaskFlag.World;
			foreach (var pos in grid.EnumeratePoints())
			{
				var cell = grid[pos];
				//Works top down
				sbyte height = -1;
				for (var y = cellHeight - 1; y >= 0; y--)
				{
					var point = new int3(pos.x, y, pos.y);
					var cellCenter = new Vector3(point.x, point.y, point.z) + Vector3.one / 2f;
					var worlCenter = toWorldMatrix.MultiplyPoint(cellCenter);
					if (Physics.CheckBox(worlCenter, cellHalfSize, cellOrientation, layerMask))
					{
						height = (sbyte)y;
						break;
					}
				}
				grid[pos] = cell.SetHeight(height);
			}
		}
	}
}
