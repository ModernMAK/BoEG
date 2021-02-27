using System;
using UnityEngine;


namespace MobaGame
{
	[Serializable]
	public struct VisionCellData
	{
		[SerializeField]
		private sbyte _height;
		public sbyte Height => _height;
		public VisionCellData SetHeight(sbyte height) => new VisionCellData()
		{
			_height = height
		};
	}
}
