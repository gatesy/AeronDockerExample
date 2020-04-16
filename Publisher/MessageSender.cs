using Adaptive.Aeron;
using Adaptive.Agrona;
using Adaptive.Agrona.Concurrent;
using Org.SbeTool.Sbe.Dll;

namespace Publisher
{
    public class MessageSender
    {
        private readonly Publication _publication;
        private readonly MessageEncoder _messageEncoder;
        private readonly byte[] _buffer = new byte[1408]; // Set to default Aeron MTU size
        private readonly DirectBuffer _bufferForSbe;
        private readonly IDirectBuffer _bufferForAeron;

        public MessageSender(Publication publication, MessageEncoder messageEncoder)
        {
            _publication = publication;
            _messageEncoder = messageEncoder;
            _bufferForSbe = new DirectBuffer(_buffer);
            _bufferForAeron = new UnsafeBuffer(_buffer);
        }

        public long Send()
        {
            var messageLength = _messageEncoder.WriteNext(_bufferForSbe);
            return _publication.Offer(_bufferForAeron, 0, messageLength);
        }
    }
}