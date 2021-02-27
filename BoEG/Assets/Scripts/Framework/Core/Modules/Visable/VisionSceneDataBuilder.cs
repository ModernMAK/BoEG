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
		public bool _save;
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
			var shift = Bound.size / 2f;
			var t = Bound.transform;
			var xShift = t.right * shift.x;
			var yShift = t.up * shift.y;
			var zShift = t.forward * shift.z;
			var origin = Bound.center - (xShift + yShift + zShift) ;
			var rot = t.rotation;
			var scale = Vector3.Scale(Bound.size, CellCount.AsVector3().Inverse());
			if (SceneAsset == null)
			{
				SceneAsset = LevelVisionData.Create(origin, rot, scale, CellCount);
				Save();
			}
			else
			{
				SceneAsset.SetMatrix(origin, rot, scale);
				SceneAsset.SetCellCount(CellCount);
				EditorUtility.SetDirty(SceneAsset);
			}
		}
		public void Build()
		{
			if (SceneAsset != null)
			{
				SceneAsset.BuildHeightGrid();
				EditorUtility.SetDirty(SceneAsset);
			} 
		}
		public void Save()
		{
			if (SceneAsset != null)
			{
				const string RootPath = "Assets/Game/Data/Vision";
				const string AssetExtension = ".asset";
				var sceneName = SceneManager.GetActiveScene().name;
				var assetName = sceneName + AssetExtension;
				var path = Path.Combine(RootPath, assetName);
				AssetDatabase.CreateAsset(SceneAsset,path);
			}
		}

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

			if (_save)
			{
				_save = false;
				Save();
			}
			
		}
#endif
		private void OnDrawGizmosSelected()
		{
			if (HideGizmos)
				return;
			if (SceneAsset == null)
				return;
			DrawMatrix(SceneAsset);
			DrawCells(SceneAsset, Color.white, Color.red);
		}
		private static void DrawCells(LevelVisionData data, Color fill, Color invalid)
		{
			var cellCount = data.CellCount;
			var cellSize = data.CellSize;
			var cellHalfSize = cellSize / 1f;
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
							var size = new Vector3(cellHalfSize.x, 0, cellHalfSize.z);
							Gizmos.DrawWireCube(baseCenter,size);
						}
						else
						{
							var topCenter = data.GetWorld(new int3(x, y, z), xzOffset);
							var center = (topCenter + baseCenter) / 2;
							var size = new Vector3(cellHalfSize.x, topCenter.y - baseCenter.y, cellHalfSize.z);
							Gizmos.DrawWireCube(center, size);
						}
					}
					else
					{
						Gizmos.color = invalid;
						var center = data.GetWorld(new int3(x, 0, z), xzOffset);
						var size = new Vector3(cellHalfSize.x, 0, cellHalfSize.z);
						Gizmos.DrawWireCube(center, size);
					}
				}
			Gizmos.color = cachedColor;
		}

		private static void DrawMatrix(LevelVisionData data)
		{
			var main = data.WorldToCell;
			var sec = data.CellToWorld;
			DrawMatrix(main,Color.white,Color.red,Color.green,Color.blue);
			DrawMatrix(sec,Color.black,Color.cyan,Color.magenta,Color.yellow);
			Gizmos.color = Color.black;
			Gizmos.DrawWireCube(sec.GetColumn(3),Vector3.one);
		}
		private static void DrawMatrix(Matrix4x4 matrix, Color origin, Color colX, Color colY, Color colZ)
		{
			var pos = (Vector3)matrix.GetColumn(3);
			Gizmos.color = origin;
			Gizmos.DrawWireSphere(pos,1f);
			
			var x = (Vector3)matrix.GetColumn(0);
			Gizmos.color = colX;
			Gizmos.DrawRay(pos,x * 2);
			
			var y = (Vector3)matrix.GetColumn(1);
			Gizmos.color = colY;
			Gizmos.DrawRay(pos,y * 2);
			
			var z = (Vector3)matrix.GetColumn(2);
			Gizmos.color = colZ;
			Gizmos.DrawRay(pos,z * 2);
		}
	}
}