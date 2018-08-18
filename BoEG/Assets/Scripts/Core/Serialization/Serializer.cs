public class Serializer : ISerializer, IDisposable
{
	public Serializer()
	{	
		_stream = new MemoryStream();
		_writer = new BinaryWriter(_stream);
	}
	
	private MemoryStream _stream;
	private BinaryWriter _writer;
	
	public void Dispose()
	{
		_stream.Dispose();
		_writer.Dispose();
	}
	
	public void Write(bool value)
	{
		//byte
		Write(BitConverter.GetBytes(value));
	}
	public void Write(byte value)
	{
		_writer.Write(value);
	}
	public void Write(sbyte value)
	{
		_writer.Write(value);
	}
	public void Write(byte[] value)
	{
		_writer.Write(value);
	}
	public void Write(byte[] value, int start, int len)
	{
		_writer.Write(value,start,len);
	}
	public void Write(char value)
	{
		_writer.Write(value);
	}
	public void Write(char[] value)
	{
		_writer.Write(value);
	}
	public void Write(char[] value, int start, int len)
	{
		_writer.Write(value, start, len);
	}
	public void Write(short value)
	{
		_writer.Write(value);
	}
	public void Write(ushort value)
	{
		_writer.Write(value);
	}
	public void Write(float value)
	{
		_writer.Write(value);
	}
	public void Write(int value)
	{
		_writer.Write(value);
	}
	public void Write(uint value)
	{
		_writer.Write(value);
	}
	public void Write(double value)
	{
		_writer.Write(value);
	}
	public void Write(long value)
	{		
		_writer.Write(value);
	}
	public void Write(ulong value)
	{
		_writer.Write(value);
	}
	public void Write(decimal value)
	{
		_writer.Write(value);
	}
	public byte[] Output()
	{
		return _stream.ToArray();
	}
}

// public static classs SerialExtensions
// {
	// public static void Write(this ISerializer serializer, ISerializable serializable)
	// {
		// serializable.Serialize(serializer);
	// }
	// public static void Read(this IDeserializer deserializer, ISerializable serializable)
	// {
		// serializable.Deserialize(deserializer);
	// }
// }



