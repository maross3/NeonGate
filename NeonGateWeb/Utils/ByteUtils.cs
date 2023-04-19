namespace NeonGateWeb.Utils;

public static class ByteUtils
{
    /// <summary>
    /// Extremely fast way of creating a new byte[] by concatenating 2 arrays
    /// </summary>
    /// <param name="first">The first array to concatenate</param>
    /// <param name="second">The second array to concatenate</param>
    public static byte[] ConcatByteArrays(byte[] first, byte[] second)
    {
        var dst = new byte[first.Length + second.Length];
        Buffer.BlockCopy((Array) first, 0, (Array) dst, 0, first.Length);
        Buffer.BlockCopy((Array) second, 0, (Array) dst, first.Length, second.Length);
        return dst;
    }

    /// <summary>
    /// Extremely fast way of creating a new byte[] by concatenating 3 arrays
    /// </summary>
    /// <param name="first">The first array to concatenate</param>
    /// <param name="second">The second array to concatenate</param>
    /// <param name="third">The third array to concatenate</param>
    public static byte[] ConcatByteArrays(byte[] first, byte[] second, byte[] third)
    {
        var dst = new byte[first.Length + second.Length + third.Length];
        Buffer.BlockCopy((Array) first, 0, (Array) dst, 0, first.Length);
        Buffer.BlockCopy((Array) second, 0, (Array) dst, first.Length, second.Length);
        Buffer.BlockCopy((Array) third, 0, (Array) dst, first.Length + second.Length, third.Length);
        return dst;
    }

    /// <summary>
    /// Extremely fast way of creating a new byte[] by concatenating a N number of arrays
    /// </summary>
    /// <param name="arrays">an array of arrays to concatenate together</param>
    public static byte[] ConcatByteArrays(params byte[][] arrays)
    {
        var dst = new byte[arrays.Sum((Func<byte[], int>) (x => x.Length))];
        var dstOffset = 0;
        for (var index = 0; index < arrays.Length; ++index)
        {
            Buffer.BlockCopy((Array) arrays[index], 0, (Array) dst, dstOffset, arrays[index].Length);
            dstOffset += arrays[index].Length;
        }

        return dst;
    }

    /// <summary>
    /// Extremely fast way of getting a sub byte[] from another byte[]
    /// </summary>
    /// <param name="sourceArray">The source array to get the sub array from</param>
    /// <param name="index">the index to start the sub array from the sourceArray</param>
    /// <param name="length">how many bytes to get from the sourceArray</param>
    public static byte[] SubByteArray(byte[] sourceArray, int index, int length = -1)
    {
        if (index > sourceArray.Length || index < 0)
            return (byte[]) null;
        if (length < 0)
            length = sourceArray.Length - index;
        
        var dst = new byte[length];
        Buffer.BlockCopy((Array) sourceArray, index, (Array) dst, 0, length);
        return dst;
    }
}