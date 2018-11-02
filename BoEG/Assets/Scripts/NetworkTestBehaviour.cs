//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using Networking;
//using UnityEditor;
//using UnityEngine;
//
//public class NetworkTestBehaviour : MonoBehaviour
//{
//    private Transport _transport;
//    private IPEndPoint _endPoint;
//
//    //
//    //Fragmented -> 
//    //Orderered
//    //Reliable
//
//    // Use this for initialization
//    void Start()
//    {
//        _endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
//        _transport = new Transport(_endPoint);
//
////        Test(buffer, WritePacketABuilder(), ReadPacketA);
//        var size = DetermineMaxSize();
//        byte[] buffer = new byte[size];
//        Test(buffer, ((buff) => WritePacketA(buff, size)), ReadPacketA);
//    }
//
//
//
//    private void Test(byte[] buffer, Action<byte[]> write, Func<byte[], string> read)
//    {
//        write(buffer);
//        Debug.Log("Wrote : " + read(buffer));
//        _transport.Send(buffer, _endPoint);
//        //Cleared Buffer
//        for (var i = 0; i < buffer.Length; i++)
//            buffer[i] = 0;
//        IPEndPoint sender;
//        _transport.Recieve(buffer, out sender);
//        Debug.Log("Read : " + read(buffer));
//    }
//
//    private ushort DetermineMaxSize()
//    {
//        ushort min = 0;
//        ushort next = 1;
//        ushort max = ushort.MaxValue;
//        IPEndPoint temp;
//        while (min < max)
//        {
//            var size = (ushort) Mathf.RoundToInt((min + max + next) / 3f);
//            byte[] buffer = new byte[size];
//            try
//            {
//                _transport.Send(buffer, _endPoint);
//                _transport.Recieve(buffer, out temp);
//                min = size;
//                next = (ushort) (size + 1);
//                if (next == max)
//                    break;
//            }
//            catch (SocketException ex)
//            {
//                next = (ushort) (size - 1);
//                max = size;
//                if (next == min)
//                    break;
//            }
//        }
//
//        return min;
//    }
//
//    private Action<byte[]> WritePacketABuilder(ushort min = ushort.MinValue,
//        ushort max = ushort.MaxValue)
//    {
//        return (buff) => WritePacketA(buff, min, max);
//    }
//
//    private void WritePacketA(byte[] buffer, ushort size)
//    {
//        using (var stream = new MemoryStream(buffer))
//        using (var writer = new BinaryWriter(stream))
//        {
//            var rand = new System.Random();
//            size -= sizeof(ushort);
//            var packet = new byte[size];
//            rand.NextBytes(packet);
//
//            writer.Write(size);
//            writer.Write(packet);
//        }
//    }
//
//    private void WritePacketA(byte[] buffer, ushort min, ushort max)
//    {
//        using (var stream = new MemoryStream(buffer))
//        using (var writer = new BinaryWriter(stream))
//        {
//            var rand = new System.Random();
//            var size = (ushort) rand.Next(min, max);
//            size -= sizeof(ushort);
//            var packet = new byte[size];
//            rand.NextBytes(packet);
//
//            writer.Write(size);
//            writer.Write(packet);
//        }
//    }
//
//    private string ToHex(byte data)
//    {
//        char[] hex = new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
//        byte upper = (byte) ((data >> 4) % 16);
//        byte lower = (byte) (data % 16);
//        return "" + hex[upper] + hex[lower];
//    }
//
//    private string ReadPacketA(byte[] buffer)
//    {
//        using (var stream = new MemoryStream(buffer))
//        using (var reader = new BinaryReader(stream))
//        {
//            var builder = new StringBuilder();
//            var size = reader.ReadUInt16();
//            builder = builder.Append("Size:");
//            builder = builder.Append(size);
//            builder = builder.Append("\n");
//
//            for (var i = 0; i < size; i++)
//            {
//                builder = builder.Append(ToHex(reader.ReadByte()));
//                if ((i + 1) % 256 == 0)
//                    builder = builder.Append(" ");
//                else if ((i + 1) % 32 == 0)
//                    builder = builder.Append(" ");
//                else if ((i + 1) % 4 == 0)
//                    builder = builder.Append(" ");
//            }
//
//            return builder.ToString();
//        }
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//    }
//}