using MobaGame.Framework.Types;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace MobaGame.Assets.Scripts.Framework.Core
{

	[Obsolete]
	[Serializable]
	public class VisionHeightGrid : Grid<int>
	{
		[Obsolete]
		public static VisionHeightGrid BuildHeightGrid(VisionWorldMapper mapper)
		{
			var origin = mapper.Origin;
			var divs = mapper.Divisions;
			var grid = new VisionHeightGrid(divs.x, divs.z);
			var divSize = mapper.DivisionSize;
			var divHalfSize = divSize / 2f;
			var rot = mapper.Rotation;
			const int layerMask = (int)LayerMaskFlag.World; 
			for (var x = 0; x < divs.x; x++)
				for (var z = 0; z < divs.z; z++)
				{
					//Works top down
					var height = -1;
					for (var y = divs.y - 1; y >= 0; y--)
					{
						var point = new int3(x, y, z);
						var center = Vector3.Scale((float3)point, divSize);
						center += divHalfSize;
						center = rot * center;
						center += origin;
						if (Physics.CheckBox(center, divHalfSize, rot, layerMask))
						{
							height = y;
							break;
						}
					}
					grid[x, z] = height;
				}
			return grid;
		}
		[Obsolete]
		public VisionHeightGrid(int w, int h) : base(w,h)
		{
		}
	}
}