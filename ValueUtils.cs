using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Psxbox.Utils;

public static class ValueUtils
{
    public static int GetValueSize(string valueType) => valueType.ToLower() switch
    {
        "byte" => 1,
        "int16" or "uint16" => 2,
        "int32" or "uint32" => 4,
        "int64" or "uint64" => 8,
        "float" or "single" => 4,
        "double" => 8,
        "half" => 2,
        _ => throw new("unsupported value type: " + valueType),
    };

    public static object ConvertValueByType(ReadOnlySpan<byte> valueData, string valueType) => valueType switch
    {
        "byte" => valueData[0],
        "int16" => BitConverter.ToInt16(valueData),
        "int32" => BitConverter.ToInt32(valueData),
        "int64" => BitConverter.ToInt64(valueData),
        "uint16" => BitConverter.ToUInt16(valueData),
        "uint32" => BitConverter.ToUInt32(valueData),
        "uint64" => BitConverter.ToUInt64(valueData),
        "half" => BitConverter.ToHalf(valueData),
        "float" or "single" => BitConverter.ToSingle(valueData),
        "double" => BitConverter.ToDouble(valueData),
        "ascii" => Encoding.ASCII.GetString(valueData),
        _ => throw new("Unsupported value type: " + valueType),
    };

    public static bool IsFinite(object value)
    {
        return value switch
        {
            float f => float.IsFinite(f),
            double d => double.IsFinite(d),
            Half h => Half.IsFinite(h),
            _ => true
        };
    }

    public static Span<byte> GetBytesFromArray<T>(T[] data) where T : struct
    {
        var result = new List<byte>();
        foreach (T item in data)
        {
            byte[] bytes = item switch
            {
                byte b => [b],
                char ch => BitConverter.GetBytes(ch),
                short s => BitConverter.GetBytes(s),
                ushort u => BitConverter.GetBytes(u),
                int i => BitConverter.GetBytes(i),
                uint u => BitConverter.GetBytes(u),
                long l => BitConverter.GetBytes(l),
                ulong l => BitConverter.GetBytes(l),
                float f => BitConverter.GetBytes(f),
                double d => BitConverter.GetBytes(d),
                Half h => BitConverter.GetBytes(h),
                _ => throw new ArgumentException("Unsupported type")
            };

            result.AddRange(bytes);
        }
        return result.ToArray();
    }

    public static byte[] GetBytesFromArrayUnsafe<T>(T[] data) where T : unmanaged
    {
        var result = new byte[data.Length * Unsafe.SizeOf<T>()];
        var span = MemoryMarshal.Cast<byte, T>(result.AsSpan());

        for (int i = 0; i < data.Length; i++)
        {
            span[i] = data[i];
        }

        return result;
    }
}
