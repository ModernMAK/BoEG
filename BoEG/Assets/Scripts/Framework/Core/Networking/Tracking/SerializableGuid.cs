using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct SerializableGuid : IFormattable, IComparable, IComparable<SerializableGuid>, IComparable<Guid>,
        IEquatable<SerializableGuid>, IEquatable<Guid>
    {
        public static SerializableGuid Empty => Guid.Empty;

        public static SerializableGuid NewGuid() => Guid.NewGuid();
        public static SerializableGuid ParseExact(string input, string format) => Guid.ParseExact(input, format);
        public static SerializableGuid Parse(string input) => Guid.Parse(input);

        public static bool TryParse(string input, out SerializableGuid result)
        {
            var success = Guid.TryParse(input, out var temp);
            result = success ? temp : default;
            return success;
        }

        public static bool TryParseExact(string input, string format, out SerializableGuid result)
        {
            var success = Guid.TryParseExact(input, format, out var temp);
            result = success ? temp : default;
            return success;
        }


        public SerializableGuid(Guid guid) : this()
        {
            _guid = guid;
        }

        public SerializableGuid(SerializableGuid guid) : this()
        {
            _guid = guid._guid;
        }

        //GUID
        public Guid Guid => _guid;
        [FieldOffset(0)] private Guid _guid;

        //1 Int
        [FieldOffset(0)] [SerializeField] private int _a;

        //2 Shorts
        [SerializeField] [FieldOffset(4)] private short _b;

        [SerializeField] [FieldOffset(6)] private short _c;

        //8 Bytes
        [SerializeField] [FieldOffset(8)] private byte _d;
        [SerializeField] [FieldOffset(9)] private byte _e;
        [SerializeField] [FieldOffset(10)] private byte _f;
        [SerializeField] [FieldOffset(11)] private byte _g;
        [SerializeField] [FieldOffset(12)] private byte _h;
        [SerializeField] [FieldOffset(13)] private byte _i;
        [SerializeField] [FieldOffset(14)] private byte _j;
        [SerializeField] [FieldOffset(15)] private byte _k;

        public override string ToString() => _guid.ToString();
        public string ToString(string format) => _guid.ToString(format);

        public string ToString(string format, IFormatProvider formatProvider) => _guid.ToString(format, formatProvider);

        public int CompareTo(object obj)
        {
            return obj switch
            {
                SerializableGuid serializableGuid => CompareTo(serializableGuid),
                Guid guid => CompareTo(guid),
                _ => _guid.CompareTo(obj)
            };
        }

        public int CompareTo(SerializableGuid other) => _guid.CompareTo(other._guid);

        public int CompareTo(Guid other) => _guid.CompareTo(other);

        public bool Equals(SerializableGuid other) => _guid.Equals(other._guid);

        public bool Equals(Guid other) => _guid.Equals(other);

        public override bool Equals(object obj)
        {
            return obj switch
            {
                SerializableGuid serializableGuid => Equals(serializableGuid),
                Guid guid => Equals(guid),
                _ => _guid.Equals(obj)
            };
        }

        public override int GetHashCode()
        {
            // All properties in this structure are essentially read only, so we ignore the error
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _guid.GetHashCode();
        }

        public byte[] ToByteArray() => _guid.ToByteArray();
        
        public static implicit operator Guid(SerializableGuid guid) => guid._guid;
        public static implicit operator SerializableGuid(Guid guid) => new SerializableGuid(guid);
    }
}