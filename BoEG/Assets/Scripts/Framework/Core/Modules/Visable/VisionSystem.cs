using MobaGame.Framework.Core.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MobaGame.Assets.Scripts.Framework.Core
{
	public class VisionSystem : MonoBehaviour
	{
		/// <summary>
		/// This vision reveals fog of war
		/// </summary>
		public VisionGrid8 BasicVision { get; private set; }
		/// <summary>
		/// This vision reveals invisible units
		/// </summary>
		public VisionGrid8 TrueVision { get; private set; }
		public VisionDictionary<TeamData> TeamLookup { get; private set; }

		
		public bool IsVisible(int x, int y, TeamData team)
		{
			var index = TeamLookup[team];
			return BasicVision.GetBit(x, y, index);
		}
		public bool IsSpotted(int x, int y, TeamData team)
		{
			var index = TeamLookup[team];
			return TrueVision.GetBit(x, y, index);
		}
		public int2 Remap(Vector3 position)
		{
			Vector2 worldPos = new Vector2(position.x, position.z);
			return new int2(worldPos);
		}
	}
	public class VisionDictionary<T> : IReadOnlyDictionary<T, int>
	{
		public VisionDictionary(int size)
		{
			Backing = new Dictionary<T, int>(size);
			Size = size;
		}
		public int Size { get; private set; }
		public bool TryAdd(T item)
		{
			if (TryGetValue(item, out _))
				return true;
			if (Backing.Count == Size)
				return false;
			Backing[item] = Backing.Count;
			return true;
		}
		public int this[T key] => ((IReadOnlyDictionary<T, int>)Backing)[key];

		public IEnumerable<T> Keys => ((IReadOnlyDictionary<T, int>)Backing).Keys;

		public IEnumerable<int> Values => ((IReadOnlyDictionary<T, int>)Backing).Values;

		public int Count => ((IReadOnlyCollection<KeyValuePair<T, int>>)Backing).Count;

		private Dictionary<T,int> Backing { get; set; }

		public bool ContainsKey(T key)
		{
			return ((IReadOnlyDictionary<T, int>)Backing).ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<T, int>>)Backing).GetEnumerator();
		}

		public bool TryGetValue(T key, out int value)
		{
			return ((IReadOnlyDictionary<T, int>)Backing).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Backing).GetEnumerator();
		}
	}
	[Serializable]
	public class Grid<T> : IList<T>, IReadOnlyList<T>
	{
		public Grid(int2 size)
		{
			_internal = new T[size.x * size.y];
			_size = size;
		}
		public Grid(int w, int h) :this(new int2(w,h))
		{
		}
		[SerializeField]
		private T[] _internal;
		[SerializeField]
		private int2 _size;
		public T[] Internal => _internal;
		public int2 Size => _size;
		public int Width => Size.x;
		public int Height => Size.y;

		public int Count => _internal.Length;

		public bool IsReadOnly => throw new NotImplementedException();

		protected int CalculateIndex(int x, int y) => x + y * Width;
		public T this[int i]
		{
			get => Internal[i];
			set => Internal[i] = value;
		}
		public T this[int x, int y]
		{
			get => this[CalculateIndex(x, y)];
			set => this[CalculateIndex(x, y)] = value;
		}
		public T this[int2 point]
		{
			get => this[point.x, point.y];
			set => this[point.x, point.y] = value;
		}

		public IEnumerable<int2> EnumeratePoints()
		{
			for (var x = 0; x < Width; x++)
				for (var y = 0; y < Height; y++)
					yield return new int2(x, y);
		}

		public bool IsValid(int2 point) => math.all(int2.zero <= point) && math.all(point < Size);

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)_internal).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internal.GetEnumerator();
		}

		public int IndexOf(T item) => ((IList<T>)_internal).IndexOf(item);

		public void Insert(int index, T item) => throw new NotSupportedException("Grid does not support Insert!");
		

		public void RemoveAt(int index) => throw new NotSupportedException("Grid does not support RemoveAt!");

		public void Add(T item) => throw new NotSupportedException("Grid does not support Add!");

		public void Clear() => ((IList<T>)_internal).Clear();

		public bool Contains(T item) => ((IList<T>)_internal).Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => _internal.CopyTo(array, arrayIndex);

		public bool Remove(T item) => throw new NotSupportedException("Grid does not support Remove!");
	}
	public abstract class BitGrid<T> : Grid<T>
	{
		public BitGrid(int w, int h) : base(w, h)
		{ 
		}
		public abstract void SetBit(int i, int bit, bool value);
		public abstract bool GetBit(int i, int bit);
		public void SetBit(int x, int y, int bit, bool value) => SetBit(CalculateIndex(x, y), bit, value);
		public bool GetBit(int x, int y, int bit) => GetBit(CalculateIndex(x, y), bit);
		public void SetBit(int2 point, int bit, bool value) => SetBit(point.x,point.y, bit, value);
		public bool GetBit(int2 point, int bit) => GetBit(point.x, point.y, bit);


		public abstract Texture2D CreateTexture();
		public abstract void InitializeTexture(Texture2D tex);
		public void FillTexture(Texture2D tex) => tex.SetPixelData(Internal, 0);

	}
	public class VisionGrid32 : BitGrid<int>
	{
		public VisionGrid32(int w, int h) : base(w, h)
		{
		}
		public override void SetBit(int i, int bit, bool value)
		{
			var mask = 1 << bit;
			if (value)
				this[i] |= mask;
			else
				this[i] &= ~mask;
		}
		public override bool GetBit(int i, int bit)
		{
			var mask = 1 << bit;
			return (this[i] & mask) == mask;
		}

		public override Texture2D CreateTexture() => new Texture2D(Width, Height, TextureFormat.RGBA32, false);
		public override void InitializeTexture(Texture2D tex) => tex.Resize(Width, Height, TextureFormat.BGRA32, false);
	}
	public class VisionGrid8 : BitGrid<byte>
	{
		public VisionGrid8(int w, int h) : base(w, h)
		{
		}
		public override void SetBit(int i, int bit, bool value)
		{
			int flags = this[i];
			var mask = 1 << bit;
			if (value)
				flags |= mask;
			else
				flags &= ~mask;
			this[i] = (byte)flags;
		}
		public override bool GetBit(int i, int bit)
		{
			var mask = 1 << bit;
			return (this[i] & mask) == mask;
		}

		public override Texture2D CreateTexture() => new Texture2D(Width, Height, TextureFormat.Alpha8, false);
		public override void InitializeTexture(Texture2D tex) => tex.Resize(Width, Height, TextureFormat.Alpha8, false);
	}
}