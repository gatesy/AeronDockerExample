using System;
using AeronDockerExample.Protocol.Sbe;
using Org.SbeTool.Sbe.Dll;

namespace Publisher
{
    public class MessageEncoder
    {
        private Guid _guid;
        private readonly int _id;
        private readonly Random _randomValues = new Random();
        
        private int _currentIndex;
        
        public MessageEncoder(int id)
        {
            _id = id;
            _guid = Guid.NewGuid();
        }

        public int WriteNext(DirectBuffer buffer)
        {
            var header = new MessageHeader();
            header.Wrap(buffer, 0, DataValue.SchemaVersion);
            header.BlockLength = DataValue.BlockLength;
            header.TemplateId = DataValue.TemplateId;
            header.SchemaId = DataValue.SchemaId;
            header.Version = DataValue.SchemaVersion;

            var dataValue = new DataValue();
            dataValue.WrapForEncode(buffer, MessageHeader.Size);
            dataValue.Id = _id;
            dataValue.Index = ++_currentIndex;
            dataValue.Value = _randomValues.NextDouble();
            dataValue.Timestamp = (ulong) DateTime.UtcNow.ToBinary();
            _guid.TryWriteBytes(dataValue.InstanceAsSpan());
            
            return MessageHeader.Size + DataValue.BlockLength;
        }
    }
}