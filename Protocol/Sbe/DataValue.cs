/* Generated SBE (Simple Binary Encoding) message codec */

#pragma warning disable 1591 // disable warning on missing comments
using System;
using Org.SbeTool.Sbe.Dll;

namespace AeronDockerExample.Protocol.Sbe
{
    public sealed partial class DataValue
    {
        public const ushort BlockLength = (ushort)40;
        public const ushort TemplateId = (ushort)1;
        public const ushort SchemaId = (ushort)1;
        public const ushort SchemaVersion = (ushort)2;
        public const string SemanticType = "";

        private readonly DataValue _parentMessage;
        private DirectBuffer _buffer;
        private int _offset;
        private int _limit;
        private int _actingBlockLength;
        private int _actingVersion;

        public int Offset { get { return _offset; } }

        public DataValue()
        {
            _parentMessage = this;
        }

        public void WrapForEncode(DirectBuffer buffer, int offset)
        {
            _buffer = buffer;
            _offset = offset;
            _actingBlockLength = BlockLength;
            _actingVersion = SchemaVersion;
            Limit = offset + _actingBlockLength;
        }

        public void WrapForDecode(DirectBuffer buffer, int offset, int actingBlockLength, int actingVersion)
        {
            _buffer = buffer;
            _offset = offset;
            _actingBlockLength = actingBlockLength;
            _actingVersion = actingVersion;
            Limit = offset + _actingBlockLength;
        }

        public int Size
        {
            get
            {
                return _limit - _offset;
            }
        }

        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _buffer.CheckLimit(value);
                _limit = value;
            }
        }


        public const int InstanceId = 1;
        public const int InstanceSinceVersion = 0;
        public const int InstanceDeprecated = 0;
        public bool InstanceInActingVersion()
        {
            return _actingVersion >= InstanceSinceVersion;
        }

        public static string InstanceMetaAttribute(MetaAttribute metaAttribute)
        {
            switch (metaAttribute)
            {
                case MetaAttribute.Epoch: return "";
                case MetaAttribute.TimeUnit: return "";
                case MetaAttribute.SemanticType: return "";
                case MetaAttribute.Presence: return "required";
            }

            return "";
        }

        public const byte InstanceNullValue = (byte)255;
        public const byte InstanceMinValue = (byte)0;
        public const byte InstanceMaxValue = (byte)254;

        public const int InstanceLength = 16;

        public byte GetInstance(int index)
        {
            if ((uint) index >= 16)
            {
                ThrowHelper.ThrowIndexOutOfRangeException(index);
            }

            return _buffer.Uint8Get(_offset + 0 + (index * 1));
        }

        public void SetInstance(int index, byte value)
        {
            if ((uint) index >= 16)
            {
                ThrowHelper.ThrowIndexOutOfRangeException(index);
            }

            _buffer.Uint8Put(_offset + 0 + (index * 1), value);
        }

        public const int IdId = 2;
        public const int IdSinceVersion = 0;
        public const int IdDeprecated = 0;
        public bool IdInActingVersion()
        {
            return _actingVersion >= IdSinceVersion;
        }

        public static string IdMetaAttribute(MetaAttribute metaAttribute)
        {
            switch (metaAttribute)
            {
                case MetaAttribute.Epoch: return "";
                case MetaAttribute.TimeUnit: return "";
                case MetaAttribute.SemanticType: return "";
                case MetaAttribute.Presence: return "required";
            }

            return "";
        }

        public const int IdNullValue = -2147483648;
        public const int IdMinValue = -2147483647;
        public const int IdMaxValue = 2147483647;

        public int Id
        {
            get
            {
                return _buffer.Int32GetLittleEndian(_offset + 16);
            }
            set
            {
                _buffer.Int32PutLittleEndian(_offset + 16, value);
            }
        }


        public const int IndexId = 3;
        public const int IndexSinceVersion = 0;
        public const int IndexDeprecated = 0;
        public bool IndexInActingVersion()
        {
            return _actingVersion >= IndexSinceVersion;
        }

        public static string IndexMetaAttribute(MetaAttribute metaAttribute)
        {
            switch (metaAttribute)
            {
                case MetaAttribute.Epoch: return "";
                case MetaAttribute.TimeUnit: return "";
                case MetaAttribute.SemanticType: return "";
                case MetaAttribute.Presence: return "required";
            }

            return "";
        }

        public const int IndexNullValue = -2147483648;
        public const int IndexMinValue = -2147483647;
        public const int IndexMaxValue = 2147483647;

        public int Index
        {
            get
            {
                return _buffer.Int32GetLittleEndian(_offset + 20);
            }
            set
            {
                _buffer.Int32PutLittleEndian(_offset + 20, value);
            }
        }


        public const int ValueId = 4;
        public const int ValueSinceVersion = 0;
        public const int ValueDeprecated = 0;
        public bool ValueInActingVersion()
        {
            return _actingVersion >= ValueSinceVersion;
        }

        public static string ValueMetaAttribute(MetaAttribute metaAttribute)
        {
            switch (metaAttribute)
            {
                case MetaAttribute.Epoch: return "";
                case MetaAttribute.TimeUnit: return "";
                case MetaAttribute.SemanticType: return "";
                case MetaAttribute.Presence: return "required";
            }

            return "";
        }

        public const double ValueNullValue = double.NaN;
        public const double ValueMinValue = 4.9E-324d;
        public const double ValueMaxValue = 1.7976931348623157E308d;

        public double Value
        {
            get
            {
                return _buffer.DoubleGetLittleEndian(_offset + 24);
            }
            set
            {
                _buffer.DoublePutLittleEndian(_offset + 24, value);
            }
        }


        public const int TimestampId = 5;
        public const int TimestampSinceVersion = 2;
        public const int TimestampDeprecated = 0;
        public bool TimestampInActingVersion()
        {
            return _actingVersion >= TimestampSinceVersion;
        }

        public static string TimestampMetaAttribute(MetaAttribute metaAttribute)
        {
            switch (metaAttribute)
            {
                case MetaAttribute.Epoch: return "";
                case MetaAttribute.TimeUnit: return "";
                case MetaAttribute.SemanticType: return "";
                case MetaAttribute.Presence: return "required";
            }

            return "";
        }

        public const ulong TimestampNullValue = 0xffffffffffffffffUL;
        public const ulong TimestampMinValue = 0x0UL;
        public const ulong TimestampMaxValue = 0xfffffffffffffffeUL;

        public ulong Timestamp
        {
            get
            {
                if (_actingVersion < 2) return 0xffffffffffffffffUL;

                return _buffer.Uint64GetLittleEndian(_offset + 32);
            }
            set
            {
                _buffer.Uint64PutLittleEndian(_offset + 32, value);
            }
        }

    }
}
