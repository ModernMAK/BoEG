using MobaGame.Assets.Scripts.Framework.Core;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace MobaGame
{

	[RequireComponent(typeof(VisionSceneDataBuilder))]
    [ExecuteInEditMode]
	[Obsolete]
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

		private VisionSceneDataBuilder _builder;
		private void Awake()
		{
			_builder = GetComponent<VisionSceneDataBuilder>();
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
