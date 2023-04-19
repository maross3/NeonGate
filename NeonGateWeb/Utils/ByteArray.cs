using System.Text;
using NeonGateWeb.Global;

namespace NeonGateWeb.Utils;

/// <summary>
/// byte[] wrapper. Reset before use. The max packet length is 65535
/// </summary>
public class ByteArray
{

    public const int MSG_HEAD_LEN = 8;

    public short PacketId { get; private set; }
    public int DataSize => _writePos;
    public byte[] BufferArray => _byteArray;

    /// <summary>
    /// Packet Structure<br></br>
    /// =====================<br></br>
    /// Packet Length: 2 bytes<br></br>
    /// Packet Id: 2 bytes<br></br>
    /// Redundancy: 4 bytes<br></br>
    /// Packet Data: payload size<br></br>
    /// </summary>
    private byte[] _byteArray;

    private int _length;
    private int _writePos = 0;
    private int _readPos = 0;

    public ByteArray()
    {
        _byteArray = new byte[256];
        _length = 256;
        Reset();
    }

    public ByteArray(int size)
    {
        if (size < 32) size = 32;
        _byteArray = new byte[size];
        _length = size;
        Reset();
    }

    public ByteArray(byte[] bytes) =>
        _byteArray = bytes;

    public void Reset()
    {
        _writePos = 0;

        // todo, process msg head, send type
        _readPos = MSG_HEAD_LEN;
        // _readPos = 0;
        PacketId = 0;
    }


    public void CopyFrom(byte[] source, int srcIndex, int length)
    {
        if (!CheckWrite(length))
            throw new ArgumentOutOfRangeException();
        Array.Copy(source, srcIndex, _byteArray, 0, length);
    }

    public void ReadHead()
    {
        _readPos = 2;
        PacketId = ReadShort();
        
        // todo interpret head
        _readPos += 4;
    }

    private void Write(byte[] b, int offset)
    {
        Array.Copy(b, 0, _byteArray, _writePos, offset);
        _writePos += offset;
    }

    public void WriteHead(short packetId, int flag = 0)
    {
        var b = BitConverter.GetBytes((short) _writePos);
        Array.Copy(b, 0, _byteArray, 0, 2);
        PacketId = packetId;
        b = BitConverter.GetBytes(packetId);
        Array.Copy(b, 0, _byteArray, 2, 2);
    }

    public void WriteHead()
    {
        var b = BitConverter.GetBytes((short) _writePos);
        Array.Copy(b, 0, _byteArray, 0, 2);
        b = BitConverter.GetBytes(PacketId);
        Array.Copy(b, 0, _byteArray, 2, 2);
    }

    public void WriteByte(byte val)
    {
        if (!CheckWrite(1))
            throw new ArgumentOutOfRangeException();

        _byteArray[_writePos] = val;
        _writePos += 1;
    }

    public void WriteBool(bool val) =>
        WriteByte((byte) (val ? 1 : 0));

    public void WriteShort(short val)
    {
        if (!CheckWrite(2))
            throw new ArgumentOutOfRangeException();

        var b = BitConverter.GetBytes(val);
        Write(b, 2);
    }

    public void WriteInt(int val)
    {
        if (!CheckWrite(4))
            throw new ArgumentOutOfRangeException();

        var b = BitConverter.GetBytes(val);
        Write(b, 4);
    }

    public void WriteInt64(long val)
    {
        if (!CheckWrite(8))
            throw new ArgumentOutOfRangeException();

        var b = BitConverter.GetBytes(val);
        Write(b, 8);
    }

    public void WriteFloat(float val)
    {
        if (!CheckWrite(4))
            throw new ArgumentOutOfRangeException();

        var b = BitConverter.GetBytes(val);

        Array.Copy(b, 0, _byteArray, _writePos, 4);
        _writePos += 4;
    }

    public void WriteString(string val)
    {
        var b = Encoding.UTF8.GetBytes(val);

        if (!CheckWrite(2 + b.Length))
            throw new ArgumentOutOfRangeException();

        var bl = BitConverter.GetBytes((short) b.Length);
        Write(bl, 2);
        Write(b, b.Length);
    }

    public byte ReadByte()
    {
        if (!CheckRead(1))
            throw new ArgumentOutOfRangeException();

        var b = _byteArray[_readPos];
        _readPos += 1;
        return b;
    }

    public bool ReadBool() =>
        ReadByte() > 0;

    public short ReadShort()
    {
        if (!CheckRead(2))
            throw new ArgumentOutOfRangeException();

        var val = BitConverter.ToInt16(_byteArray, _readPos);
        _readPos += 2;
        return val;
    }

    public int ReadInt()
    {
        if (!CheckRead(4))
            throw new ArgumentOutOfRangeException();

        var val = BitConverter.ToInt32(_byteArray, _readPos);
        _readPos += 4;
        return val;
    }

    public long ReadInt64()
    {
        if (!CheckRead(8))
            throw new ArgumentOutOfRangeException();

        var val = BitConverter.ToInt64(_byteArray, _readPos);
        _readPos += 8;
        return val;
    }

    public float ReadFloat()
    {
        if (!CheckRead(4))
            throw new ArgumentOutOfRangeException();

        var val = BitConverter.ToSingle(_byteArray, _readPos);
        _readPos += 4;
        return val;
    }

    public string ReadString()
    {
        var strLength = ReadShort();
        if (!CheckRead(strLength))
            throw new ArgumentOutOfRangeException();

        var val = Encoding.UTF8.GetString(_byteArray, _readPos, strLength);
        _readPos += strLength;
        return val;
    }

    private bool CheckWrite(int length)
    {
        var newLength = _writePos + length;
        if (newLength > DatagramConstants.MAX_PACKET_LEN) return false;

        if (newLength <= _length) return true;

        var oldBytes = _byteArray;
        var oldLength = _length;

        _length += _length / 2;

        if (_length < newLength) _length = newLength + 1;

        if (_length > DatagramConstants.MAX_PACKET_LEN)
            _length = DatagramConstants.MAX_PACKET_LEN;

        _byteArray = new byte[_length];
        Array.Copy(oldBytes, _byteArray, oldLength);

        return true;
    }

    private bool CheckRead(int length)
    {
        return (_readPos + length) <= DatagramConstants.MAX_PACKET_LEN;
    }
}

public static class ByteArrayExtensions
{
    /*
     example usage:
        var bit = new byte[1];
        var newBit = bit.Convert();
        newBit.ReadHead();
    */
    public static ByteArray Convert(this byte[] bytes) => 
        new(bytes);

    public static byte[] Convert(this ByteArray arr) =>
        arr.BufferArray;
}