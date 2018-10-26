//namespace Networking
//{
//    public class FragmentBuffer
//    {
//        public FragmentBuffer(ushort bufferSize)
//        {
//            _buffer = new Data[bufferSize];
//        }
//
//        public struct Data
//        {
//            public Data(ushort sequence) : this()
//            {
//                Sequence = sequence;
//                Acked = new bool[byte.MaxValue];
//                Payloads = new byte[byte.MaxValue][];
//            }
//
//            public ushort Sequence { get; private set; }
//            
////            public Data SetSequence(ushort sequence)
////            {
////                return new Data(sequence, Acked);
////            }
//            
//            
//            public byte[][] Payloads { get; private set; }
//
//
//            public bool[] Acked { get; private set; }
//
////            public Data SetAcked(bool acked)
////            {
////                return new Data(Sequence, acked);
////            }
//        }
//
//        private readonly Data[] _buffer;
//
//        private int GetIndex(ushort index)
//        {
//            return index % _buffer.Length;
//        }
//
//        public Data Get(ushort index)
//        {
//            return _buffer[GetIndex(index)];
//        }
//
//        public void Set(ushort index, Data data)
//        {
//            _buffer[GetIndex(index)] = data;
//        }
//
//        public uint GetAcks(ushort index)
//        {
//            var startingAck = GetIndex(index) - 32;
//            if (startingAck < 0)
//                startingAck += _buffer.Length;//Cant use ushort.maxValue because it might not divide evenly into the buffer
//            var ack = 0;
//            for (var i = 0; i < 32; i++)
//            {
//                if (Get((ushort) (startingAck + i)).Acked)
//                    ack |= (1 << i);
//            }
//
//            return (uint) ack;
//        }
//    }
//    public class SequenceBuffer
//    {
//        public SequenceBuffer(ushort bufferSize)
//        {
//            _buffer = new Data[bufferSize];
//        }
//
//        public struct Data
//        {
//            public Data(ushort sequence, bool acked) : this()
//            {
//                Sequence = sequence;
//                Acked = acked;
//            }
//
//            public ushort Sequence { get; private set; }
//
//            public Data SetSequence(ushort sequence)
//            {
//                return new Data(sequence, Acked);
//            }
//
//            public bool Acked { get; private set; }
//
//            public Data SetAcked(bool acked)
//            {
//                return new Data(Sequence, acked);
//            }
//        }
//
//        private readonly Data[] _buffer;
//
//        private int GetIndex(ushort index)
//        {
//            return index % _buffer.Length;
//        }
//
//        public Data Get(ushort index)
//        {
//            return _buffer[GetIndex(index)];
//        }
//
//        public void Set(ushort index, Data data)
//        {
//            _buffer[GetIndex(index)] = data;
//        }
//
//        public uint GetAcks(ushort index)
//        {
//            var startingAck = GetIndex(index) - 32;
//            if (startingAck < 0)
//                startingAck += _buffer.Length;//Cant use ushort.maxValue because it might not divide evenly into the buffer
//            var ack = 0;
//            for (var i = 0; i < 32; i++)
//            {
//                if (Get((ushort) (startingAck + i)).Acked)
//                    ack |= (1 << i);
//            }
//
//            return (uint) ack;
//        }
//    }
//}