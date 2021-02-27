using MobaGame.Assets.Scripts.Framework.Core;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MobaGame
{
    [Obsolete]
    [RequireComponent(typeof(VisionWorldHeightGridBuilder))]
    [ExecuteInEditMode]
    public class VisionWorldTest : MonoBehaviour
    {
        private VisionWorldHeightGridBuilder _builder;
        public Texture2D _tex;
        public int3 point;
        public float radius;

        public bool run;

        public MeshRenderer _mat;
        private Grid<byte> _grid;

        public bool hideGizmos;
        public bool hideDebugGizmos;
        public bool showDark;
        public bool showLight;
        

        void Awake()
		{
            _builder = GetComponent<VisionWorldHeightGridBuilder>();
		}

        const int centerFlag = 1 << 0;
        const int fillFlag = 1 << 1;
        const int outlineFlag = 1 << 5;
        const int lineFlag = 1 << 2;
        const int visFlag = 1 << 3;
        const int touchFlag = 1 << 4;
        private void OnDrawGizmosSelected()
		{
            if (hideGizmos)
                return;
            if (hideDebugGizmos && !showDark && !showLight)
                return;

            if (_grid == null)
                return;
            var hmap = _builder.HeightMap;
            var map = _builder.Map;
            var divSize = map.DivisionSize;
            for (var x = 0; x < _grid.Width; x++)
                for (var y = 0; y < _grid.Height; y++)
				{
                    var flags = _grid[x, y];
                    var center = map.ConvertToWorldPoint(new int3(x, 0, y), true);


                    if (!hideDebugGizmos)
                        if ((flags & lineFlag) == lineFlag)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(center, divSize * 1f);
                    }
                    if ((flags & centerFlag) == centerFlag)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireCube(center, divSize * .9f);
                    }
                    if (!hideDebugGizmos)
                        if ((flags & fillFlag) == fillFlag)
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireCube(center, divSize * .6f);
                    }
                    if ((flags & outlineFlag) == outlineFlag)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawWireCube(center, divSize * .8f);
                    }
                    if (!hideDebugGizmos)
                        if ((flags & touchFlag) == touchFlag)
                    {
                        Gizmos.color = Color.gray;
                        Gizmos.DrawWireCube(center, divSize * .4f);
                    }

                    if ((flags & visFlag) == visFlag)
                    {
                        if (showLight)
                        {
                            Gizmos.color = Color.white;
                            Gizmos.DrawCube(center, divSize * .5f);
                        }
                    }
					else
					{
                        if (showDark)
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawCube(center, divSize * .5f);
                        }
                    }

				}



        }


        public void DumpToTexture32(Texture2D texture2D)
        {
            Color32 show = Color.white;
            Color32 hide = Color.black;
            var buffer = texture2D.GetPixelData<Color32>(0);
            for (var i = 0; i < _grid.Internal.Length; i++)
            {
                if ((_grid[i] & visFlag) == visFlag)
                    buffer[i] = show;
                else 
                    buffer[i] = hide;                
            }
            texture2D.Apply();
        }
        
		// Update is called once per frame
		void Update()
        {
			if (run)
			{
                run = false;
                if (_builder.Map == null)
                    return;

                _grid = new Grid<byte>(_builder.Map.Divisions.x, _builder.Map.Divisions.z);
                UpdateGrid();
                if (_mat != null)
                    _mat.material.mainTexture = null;
                if (_tex != null)
                    DestroyImmediate(_tex);
                _tex = new Texture2D(_grid.Width, _grid.Height);
                if(_mat != null)
                    _mat.material.mainTexture = _tex;
                DumpToTexture32(_tex);
			}
        }

        void UpdateGrid()
		{
            var center = new int2(point.x, point.z);
            var heightMap = _builder.HeightMap;
            _grid[center] |= centerFlag;
            foreach (var dest in LineOfSightUtil.BresenhamCircleOutline(center, radius))
            {
                if (!_grid.IsValid(dest))
                    continue;
                _grid[dest] |= outlineFlag;
            }

            foreach (var dest in LineOfSightUtil.BresenhamCircleFill(center,radius))
            {
                if (_grid.IsValid(dest))
                {
                    _grid[dest] |= fillFlag;
                    if ((_grid[dest] & touchFlag) == touchFlag)
                        continue;
                }
                var blocked = false;
                foreach (var target in LineOfSightUtil.BresenhamLine(center, dest))
                {
                    if (!_grid.IsValid(target))
                        continue;
                    _grid[target] |= touchFlag;
                    _grid[target] |= lineFlag;
                    if (!blocked)
                    {   
                        var y = heightMap[target];           
                        if (y > point.y)
                            blocked = true;
                    }
                    if(!blocked)
                        _grid[target] |= visFlag;
                }
            }
		}
    }
}
