using AeronDockerExample.Protocol.Sbe;
using Org.SbeTool.Sbe.Dll;

namespace Subscriber
{
    public class MessageDecoder
    {
        private readonly MessageHeader _messageHeader = new MessageHeader();
        private readonly DataValue _dataValue = new DataValue();
        
        public DataValue Decode(DirectBuffer buffer)
        {
            _messageHeader.Wrap(buffer, 0, DataValue.SchemaVersion);
            _dataValue.WrapForDecode(buffer, MessageHeader.Size, _messageHeader.BlockLength, _messageHeader.Version);

            return _dataValue;
        }
    }
}