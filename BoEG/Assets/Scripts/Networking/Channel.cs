//namespace Networking
//{
//    public class Channel
//    {
//        public Channel(Transport transport, ushort bufferSize = 1024, ushort sequenceBufferSize = 1024)
//        {
//            _buffer = new byte[bufferSize];
//            _transport = transport;
//            _sequence = new ushort[8];
//            _sequenceBuffer = new SequenceBuffer[8];
//            for (var i = 0; i < 8; i++)
//                _sequenceBuffer[i] = new SequenceBuffer(sequenceBufferSize);
//        }
//
//
//        private readonly byte[] _buffer;
//        private readonly Transport _transport;
//        private readonly SequenceBuffer[] _sequenceBuffer;
//        private ushort[] _sequence;
//
//
//        private void SendReliableOrderedFragmented(byte[] payload)
//        {
//            PacketType packet = PacketType.Ordered | PacketType.Reliable | PacketType.Fragmented;
//        }
//
//        private void SendReliableOrderedUnfragmented(byte[] payload)
//        {
//        }
//
//        private void SendReliableUnorderedFragmented(byte[] payload)
//        {
//        }
//
//        private void SendReliableUnorderedUnfragmented(byte[] payload)
//        {
//        }
//
//        private void SendUnreliableOrderedFragmented(byte[] payload)
//        {
//        }
//
//        private void SendUnreliableOrderedUnfragmented(byte[] payload)
//        {
//        }
//
//        private void SendUnreliableUnorderedFragmented(byte[] payload)
//        {
//        }
//
//        private void SendUnreliableUnorderedUnfragmented(byte[] payload)
//        {
//        }
//    }
//}