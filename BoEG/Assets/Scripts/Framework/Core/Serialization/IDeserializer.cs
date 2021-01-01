namespace Framework.Core.Serialization
{
    public interface IDeserializer
    {
        bool ReadBool();
        byte ReadByte();
        sbyte ReadSByte();
        byte[] ReadBytes(int len);
        int ReadBytes(byte[] buffer, int offset, int len);

        char ReadChar();
        char[] ReadChars(int len);
        string ReadString();

        float ReadFloat();
        double ReadDouble();
        decimal ReadDecimal();

        short ReadShort();
        ushort ReadUShort();
        int ReadInt();
        uint ReadUInt();
        long ReadLong();
        ulong ReadULong();
    }
}