using MobaGame.Assets.Scripts.Framework.Core;
using System;
using MobaGame.Framework.Types;
using Unity.Mathematics;
using UnityEngine;


namespace MobaGame
{
    public class LevelVisionData : ScriptableObject
    {
        public static Quaternion FromTo(Quaternion from, Quaternion to)
        {
            return from * Quaternion.Inverse(to);
        }
        public static Vector3 InverseScale(Vector3 a, Vector3 b)
        {
            return Vector3.Scale(a, b.Inverse());
        }

        public static Matrix4x4 CreateMatrix(Vector3 cellOrigin, Vector3 worldOrigin, Quaternion cellRotation,
            Quaternion worldRotation, Vector3 cellSize, Vector3 worldSize, bool cellToWorld)
        {
            Vector3 origin;
            Quaternion rotation;
            Vector3 scale;
            if (cellToWorld)
            {
                origin = cellOrigin - worldOrigin;
                rotation = FromTo(cellRotation, worldRotation);
                scale = InverseScale(cellSize, worldSize);
            }
            else
            {
                origin = worldOrigin - cellOrigin;
                rotation = FromTo(worldRotation, cellRotation);
                scale = InverseScale(worldSize, cellSize);
            }

            return Matrix4x4.TRS(origin, rotation, scale);
        }

        public static Matrix4x4 CreateMatrix(Vector3 origin, Quaternion rotation, Vector3 cellSize, bool cellToWorld) => CreateMatrix(origin,Vector3.zero, rotation, Quaternion.identity, cellSize, Vector3.one, cellToWorld);

        public static LevelVisionData Create(Vector3 origin, Quaternion rotation, Vector3 cellSize, int3 cellCount)
        {
            var cellMatrix = CreateMatrix(origin, rotation, cellSize, true);
            var worldMatrix = CreateMatrix(origin, rotation, cellSize, false);
            return Create(cellMatrix, worldMatrix, cellCount);
        }

        //This is private because we shouldn't trust matrixes that aren't crafter properly
        private static LevelVisionData Create(Matrix4x4 cellMatrix, Matrix4x4 worldMatrix, int3 cellCount)
        {
            if (cellCount.y > sbyte.MaxValue)
                cellCount.y = sbyte.MaxValue;

            var asset = CreateInstance<LevelVisionData>();
            asset._grid = new Grid<VisionCellData>(cellCount.x, cellCount.z);
            asset._maxHeight = (sbyte) cellCount.y;
            asset._cellToWorld = worldMatrix;
            asset._worldToCell = cellMatrix;
            return asset;
        }

        #region EDITOR_ONLY

#if UNITY_EDITOR
        public void SetMatrix(Vector3 origin, Quaternion rotation, Vector3 cellSize)
        {
            _worldToCell = CreateMatrix(origin, rotation, cellSize, true);
             _cellToWorld  = CreateMatrix(origin, rotation, cellSize, false);
        }

        public void SetCellCount(int3 cellCount)
        {
            if (cellCount.y > sbyte.MaxValue)
                cellCount.y = sbyte.MaxValue;
            _grid = new Grid<VisionCellData>(cellCount.x, cellCount.z);
            _maxHeight = (sbyte) cellCount.y;
        }

        public void BuildHeightGrid() => BuildHeightGrid(this);

        public static void BuildHeightGrid(LevelVisionData levelVisionData)
        {
            var cellHeight = levelVisionData.CellCount.y;
            var grid = levelVisionData.Grid;
            var cellHalfSize = levelVisionData.CellSize / 2f;
            var cellOrientation = levelVisionData.CellOrientation;
            var centerShift = Vector3.one / 2f;
            const int layerMask = (int) LayerMaskFlag.World;
            foreach (var pos in grid.EnumeratePoints())
            {
                var cell = grid[pos];
                //Works top down
                sbyte height = -1;
                for (var y = cellHeight - 1; y >= 0; y--)
                {
                    var point = new int3(pos.x, y, pos.y);
                    var worlCenter = levelVisionData.GetWorld(point,centerShift);
                    if (Physics.CheckBox(worlCenter, cellHalfSize, cellOrientation, layerMask))
                    {
                        height = (sbyte) y;
                        break;
                    }
                }

                grid[pos] = cell.SetHeight(height);
            }
        }


#endif

        #endregion

        #region Variables

        [SerializeField] private Grid<VisionCellData> _grid;
        [SerializeField] private Matrix4x4 _worldToCell;
        [SerializeField] private Matrix4x4 _cellToWorld;
        [SerializeField] private sbyte _maxHeight;

        #endregion

        #region Properties

        public Grid<VisionCellData> Grid => _grid;

        /// <summary>
        /// Matrix from World To 'Cell' Space
        /// </summary>
        /// <remarks>
        /// Main Matrix
        /// Position is the origin of the Cell Space
        /// Rotation is the rotation 
        ///</remarks>
        public Matrix4x4 WorldToCell => _worldToCell;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Inverse Matrix
        /// </remarks>
        public Matrix4x4 CellToWorld => _cellToWorld;//We cant trust inverse

        public int3 CellCount => new int3(_grid.Width, _maxHeight, _grid.Height);
        public Vector3 CellOrigin => WorldToCell.GetColumn(3);
        public Vector3 CellSize => WorldToCell.lossyScale;

        public Quaternion CellOrientation => WorldToCell.rotation;

        #endregion

        //Is Cell valid XYZ?
        public bool IsValidCell(int3 point) => math.all(int3.zero < point) && math.all(point < CellCount);

        //Is cell valid XZ?
        public bool IsValidCell(int2 point) => IsValidCell(new int3(point.x, 0, point.y));

        public int3 GetCell3d(Vector3 point)
        {
            var localPoint = CellToWorld.MultiplyPoint(point);
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
            var offset = centered ? Vector3.one / 2f : Vector3.zero;
            return GetWorld(point, offset);
        }

        public Vector3 GetWorld(int3 point, Vector3 offset)
        {
            var pos = point.AsVector3() + offset;
            return WorldToCell.MultiplyPoint(pos);
        }
    }
}