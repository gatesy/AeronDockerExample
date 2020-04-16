using System;
using AeronDockerExample.Protocol.Sbe;
using Org.SbeTool.Sbe.Dll;

namespace Publisher
{
    public class MessageEncoder
    {
        private readonly byte[] _guidBytes;
        private readonly int _id;
        private int _currentIndex = 0;
        private readonly Random _randomValues = new Random();

        public MessageEncoder(int id)
        {
            _id = id;
            _guidBytes = Guid.NewGuid().ToByteArray();
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

            for (var i = 0; i < _guidBytes.Length; ++i)
            {
                dataValue.SetInstance(i, _guidBytes[i]);
            }

            return MessageHeader.Size + DataValue.BlockLength;
        }
    }
}