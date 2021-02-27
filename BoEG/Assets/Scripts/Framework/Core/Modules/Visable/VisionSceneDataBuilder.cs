using System;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor;
#endif

namespace MobaGame.Assets.Scripts.Framework.Core
{
	public static class Int3Hack{
		public static Vector3 AsVector3(this int3 p) => new Vector3(p.x, p.y, p.z);
	}
	[ExecuteInEditMode]
	public class VisionSceneDataBuilder : MonoBehaviour
	{
		[Header("Run")]
		public bool _create;
		public bool _build;
		[Header("Settings")]
		public int3 CellCount;
		public BoxCollider Bound;
		[Header("Results")]
		public LevelVisionData SceneAsset;
		[Header("Debug")]
		public bool HideGizmos;
#if UNITY_EDITOR
		public void Create()
		{
			var origin = Bound.center - Bound.size / 2f;
			var rot = Bound.transform.rotation;
			var scale = Vector3.Scale(Bound.size, CellCount.AsVector3().Inverse());
			if (SceneAsset == null)
			{
				SceneAsset = LevelVisionData.Create(origin, rot, scale, CellCount);
				const string RootPath = "Assets/Game/Data/Vision";
				const string AssetExtension = ".asset";
				var sceneName = SceneManager.GetActiveScene().name;
				var assetName = sceneName + AssetExtension;
				var path = Path.Combine(RootPath, assetName);
				AssetDatabase.CreateAsset(SceneAsset,path);
			}
			else
			{
				SceneAsset.SetMatrix(origin, rot, scale);
				SceneAsset.SetCellCount(CellCount);
			}
		}
		public void Build()
		{
			if (SceneAsset != null)
				LevelVisionBuilder.BuildHeightGrid(SceneAsset); 
		}

#endif
		public void Update()
		{
			if (_create)
			{
				_create = false;
				Create();
			}
			if (_build)
			{
				_build = false;
				Build();
			}
		}
		private void OnDrawGizmosSelected()
		{
			if (HideGizmos)
				return;
			if (SceneAsset == null)
				return;
			DrawCells(SceneAsset, Color.white, Color.red);
		}
		private static void DrawCells(LevelVisionData data, Color fill, Color invalid)
		{
			var origin = (Vector3)data.CellToWorld.GetColumn(3);
			var cellCount = data.CellCount;
			var cellSize = data.CellSize;
			var cachedColor = Gizmos.color;
			var xzOffset = new Vector3(0.5f, 0, 0.5f);
			for (var x = 0; x < cellCount.x; x++)
				for (var z = 0; z < cellCount.z; z++)
				{
					var cell = data.Grid[x, z];
					var y = cell.Height;
					var validHeight = y >= 0;
					if (validHeight)
					{
						Gizmos.color = fill;
						var baseCenter = data.GetWorld(new int3(x, 0, z), xzOffset);
						if(y == 0)
						{
							var size = new Vector3(cellSize.x, 0, cellSize.z);
							Gizmos.DrawWireCube(baseCenter,size);
						}
						else
						{
							var topCenter = data.GetWorld(new int3(x, y, z), xzOffset);
							var center = (topCenter + baseCenter) / 2;
							var size = new Vector3(cellSize.x, topCenter.y - baseCenter.y, cellSize.z);
							Gizmos.DrawWireCube(center, size);
						}
					}
					else
					{
						Gizmos.color = invalid;
						var center = data.GetWorld(new int3(x, 0, z), xzOffset);
						var size = new Vector3(cellSize.x, 0, cellSize.z);
						Gizmos.DrawWireCube(center, size);
					}
				}
			Gizmos.color = cachedColor;
		}
	}
}