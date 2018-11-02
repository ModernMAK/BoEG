//using System.Collections.Generic;
//
//namespace Networking
//{
//    public class ReliableOrderedChannel : NetworkChannel
//    {
//        private readonly Queue<byte[]> _payloads;
//
//        public ReliableOrderedChannel()
//        {
//            _payloads = new Queue<byte[]>();
//        }
//
//        protected void Send(byte[] payload)
//        {
//            _payloads.Enqueue(payload);
//        }
//
//        protected void Recieve(byte buffer)
//        {
//        }
//    }
//}