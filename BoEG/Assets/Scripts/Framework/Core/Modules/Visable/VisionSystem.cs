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
	public class Grid<T>
	{
		public Grid(int w, int h)
		{
			_internal = new T[w * h];
			_width = w;
			_height = h;
		}
		[SerializeField]
		private T[] _internal;
		[SerializeField]
		private int _width;
		[SerializeField]
		private int _height;
		public T[] Internal => _internal;
		public int Width => _width;
		public int Height => _height;
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

		internal bool IsValid(int2 point) => 
			(0 <= point.x && point.x < Width) && 
			(0 <= point.y && point.y < Height);



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