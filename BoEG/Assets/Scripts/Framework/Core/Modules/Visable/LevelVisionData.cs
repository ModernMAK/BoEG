using MobaGame.Assets.Scripts.Framework.Core;
using System;
using Unity.Mathematics;
using UnityEngine;


namespace MobaGame
{

	public class LevelVisionData  : ScriptableObject
	{
		public static Matrix4x4 CreateMatrix(Vector3 origin, Quaternion rotation, Vector3 cellSize) => Matrix4x4.TRS(-origin, Quaternion.Inverse(rotation), cellSize.Inverse());
		public static LevelVisionData Create(Vector3 origin, Quaternion rotation, Vector3 cellSize, int3 cellCount)
		{
			var cellMatrix = CreateMatrix(origin, rotation, cellSize);
			return Create(cellMatrix,cellCount);
		}
		//This is private because we shouldn't trust matrixes that aren't crafter properly
		private static LevelVisionData Create(Matrix4x4 cellMatrix, int3 cellCount)
		{
			if (cellCount.y > sbyte.MaxValue)
				throw new Exception();

			var asset = CreateInstance<LevelVisionData>();
			asset._grid = new Grid<VisionCellData>(cellCount.x, cellCount.z);
			asset._maxHeight = (sbyte)cellCount.y;
			asset._cellToWorld = cellMatrix;
			return asset;
		}

		#region EDITOR_ONLY
#if UNITY_EDITOR
		public void SetMatrix(Vector3 origin, Quaternion rotation, Vector3 cellSize)
		{
			asset._cellToWorld = CreateMatrix(origin,rotation,cellSize);
		}
		public void SetCellCount(int3 cellCount)
		{
			asset._grid = new Grid<VisionCellData>(cellCount.x,cellCount.z);
			asset._maxHeight = cellSize.y;
		}


#endif
		#endregion
		#region Variables
		[SerializeField]
		private Grid<VisionCellData> _grid;
		[SerializeField]
		private Matrix4x4 _cellToWorld;
		[SerializeField]
		private sbyte _maxHeight;
#endregion

#region Properties
		public Grid<VisionCellData> Grid => _grid;
		/// <summary>
		/// Matrix from World To 'Cell' Space
		/// </summary>
		/// <remarks>
		/// Position is the origin of the Cell Space
		/// Rotation is the rotation 
		///</remarks>
		public Matrix4x4 WorldToCell => _cellToWorld.inverse;
		public Matrix4x4 CellToWorld => _cellToWorld;
		public int3 CellCount => new int3(_grid.Width, _maxHeight, _grid.Height);
		public Vector3 CellSize => _cellToWorld.inverse.lossyScale;

		public Quaternion CellOrientatoin => _cellToWorld.inverse.rotation;
#endregion
		//Is Cell valid XYZ?
		public bool IsValidCell(int3 point) => math.all(int3.zero < point) && math.all(point < CellCount);
		//Is cell valid XZ?
		public bool IsValidCell(int2 point) => IsValidCell(new int3(point.x, 0, point.y));

		public int3 GetCell3d(Vector3 point)
		{
			var localPoint = WorldToCell.MultiplyPoint(point);
			var intPoint = new int3(localPoint);
			return intPoint;
		}
		public int2 GetCell2d(Vector3 point)
		{
			var cell = GetCell3d(point);
			return new int2(cell.x, cell.z);
		}
		public Vector3 GetWorld(int3 point, bool centered = false)
		{
			var pos = point.AsVector3();
			if (centered)
				pos += Vector3.one / 2f;
			return CellToWorld.MultiplyPoint(pos);
		}
		public Vector3 GetWorld(int3 point, Vector3 offset)
		{
			var pos = point.AsVector3() + offset;
			return CellToWorld.MultiplyPoint(pos);
		}
	}
}
