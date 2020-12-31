namespace Framework.Core.Serialization
{
    public interface ISerializer
    {
        void Write(bool value);
        void Write(byte value);
        void Write(sbyte value);
        void Write(byte[] value);
        void Write(byte[] value, int start, int len);

        void Write(char value);
        void Write(char[] value);
        void Write(char[] value, int start, int len);
        void Write(string value);

        void Write(float value);
        void Write(double value);
        void Write(decimal value);

        void Write(short value);
        void Write(ushort value);
        void Write(int value);
        void Write(uint value);
        void Write(long value);
        void Write(ulong value);

    }
}