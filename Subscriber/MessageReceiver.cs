using System;
using Adaptive.Aeron;
using Adaptive.Aeron.LogBuffer;
using Adaptive.Agrona;
using AeronDockerExample.Protocol.Sbe;
using Org.SbeTool.Sbe.Dll;

namespace Subscriber
{
    public class MessageReceiver
    {
        private readonly Subscription _subscription;
        private readonly MessageDecoder _decoder;
        private readonly Action<DataValue> _superFastCallback;

        public MessageReceiver(Subscription subscription, MessageDecoder decoder, Action<DataValue> superFastCallback)
        {
            _subscription = subscription;
            _decoder = decoder;
            _superFastCallback = superFastCallback;
        }
        
        public int Poll()
        {
            return _subscription.Poll(FragmentHandler, 1);
        }

        private unsafe void FragmentHandler(IDirectBuffer buffer, int offset, int length, Header header)
        {
            var bufferForSbe = new DirectBuffer(((byte*) buffer.BufferPointer) + offset, length);
            var dataValue = _decoder.Decode(bufferForSbe);

            _superFastCallback(dataValue);
        }
    }
}