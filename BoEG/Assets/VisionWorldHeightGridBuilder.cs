using MobaGame.Assets.Scripts.Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace MobaGame
{
	public class LevelVisionData : ScriptableObject
	{
		[Serializable]
		public struct GridData
		{
			public byte Height;
		}
		public Grid<GridData> Grid;
		/// <summary>
		/// Matrix from World To Local Space
		/// </summary>
		public Matrix4x4 WorldToLocal;
		public Matrix4x4 LocalToWorld => WorldToLocal.inverse;
		public int3 CellCount;
		public Vector3 CellSize;

		//Is Cell valid XYZ?
		public bool IsValidCell(int3 point) => math.all(int3.zero < point) && math.all(point < CellCount);
		//Is cell valid XZ?
		public bool IsValidCell(int2 point) => IsValidCell(new int3(point.x, 0, point.y));

		public int3 GetCell3d(Vector3 point)
		{
			var localPoint = WorldToLocal.MultiplyPoint(point);
			var intPoint = new int3(localPoint);
			return intPoint;
		}
		public int2 GetCell2d(Vector3 point)
		{
			var cell = GetCell3d(point);
			return new int2(cell.x, cell.z);
		}
	}

	[RequireComponent(typeof(VisionWorldMapperBuilder))]
    [ExecuteInEditMode]
    public class VisionWorldHeightGridBuilder : MonoBehaviour
    {
		[SerializeField]
		private bool _run;
		public bool HideGizmos;
		public bool ShowInvalid;
		private VisionWorldMapper _map;
		private VisionHeightGrid _height;

		public VisionWorldMapper Map => _map;
		public VisionHeightGrid HeightMap => _height;

		private VisionWorldMapperBuilder _builder;
		private void Awake()
		{
			_builder = GetComponent<VisionWorldMapperBuilder>();
		}

		private void Update()
		{
			if(_run)
			{
				_map = _builder.Build();
				_height = VisionHeightGrid.BuildHeightGrid(_map);
				_run = false;
			}
		}
		private void OnDrawGizmosSelected()
		{
			if (HideGizmos)
				return;
			if (_map == null || _height == null)
				return;
			var divSize = _map.DivisionSize;
			var invalidDivSize = new Vector3(divSize.x, 0, divSize.z);
			for (var x = 0; x < _map.Divisions.x; x++)
				for (var z = 0; z < _map.Divisions.z; z++)
				{
					var boxSize = divSize;
					var y = _height[x, z];
					var center = _map.ConvertToWorldPoint(new int3(x, y, z), true);
					if (y < 0)
						if (!ShowInvalid)
							continue;
						else
						{
							Gizmos.color = Color.red;
						}
					else
						Gizmos.color = Color.white;
					Gizmos.DrawWireCube(center, boxSize);
				}
					
		}


	}
}
