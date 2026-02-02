// Converters class by Psxbox
// E-mail: adhamyu83@gmail.com

using System.ComponentModel;
using System.Text;

namespace Psxbox.Utils;

public static class Converters
{
    private static readonly char[] CP866SHARED =
    [
        'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П',
        'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я',
        'а', 'б', 'в', 'г', 'д', 'е', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п',
        '░', '▒', '▓', '│', '┤', '╡', '╢', '╖', '╕', '╣', '║', '╗', '╝', '╜', '╛', '┐',
        '└', '┴', '┬', '├', '─', '┼', '╞', '╟', '╚', '╔', '╩', '╦', '╠', '═', '╬', '╧',
        '╨', '╤', '╥', '╙', '╘', '╒', '╓', '╫', '╪', '┘', '┌', '█', '▄', '▌', '▐', '▀',
        'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я',
        'Ё', 'ё', 'Є', 'є', 'Ї', 'ї', 'Ў', 'ў', '°', '∙', '·', '√', '№', '¤', '■', ' '
    ];

    static Converters()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// HEX ko'rinishidagi stringni byte[] ga o'tkazish
    /// </summary>
    /// <param name="hex"> "A1B2D3" ko'rinishidagi string</param>
    /// <returns></returns>
    // public static byte[] HexStringToByteArray(string hex)
    // {
    //     return Enumerable.Range(0, hex.Length)
    //                      .Where(x => x % 2 == 0)
    //                      .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
    //                      .ToArray();
    // }

    //HexStringToByteArray overload returning a Span<byte>
    public static byte[] HexStringToByteArray(string hex)
    {
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Hex string must have an even length.");

        var result = new byte[hex.Length / 2];
        for (var i = 0; i < hex.Length; i += 2)
        {
            result[i / 2] = (byte)((GetHexValue(hex[i]) << 4) | GetHexValue(hex[i + 1]));
        }
        return result;
    }

    private static int GetHexValue(char hex)
    {
        return hex switch
        {
            >= '0' and <= '9' => hex - '0',
            >= 'a' and <= 'f' => hex - 'a' + 10,
            >= 'A' and <= 'F' => hex - 'A' + 10,
            _ => throw new ArgumentException($"Invalid hex character: {hex}")
        };
    }


    /// <summary>
    /// HEX ko'rinishidagi <b>string</b> ni <b>string</b> ga o'tkazish
    /// </summary>
    /// <param name="hex"> "A1B2D3" ko'rinishidagi string</param>
    /// <returns></returns>
    public static string HexStringToString(string hex)
    {
        var res = Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => (char)Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
        return new string(res);
    }

    /// <summary>
    /// DecodeCP866 decodes a byte array using the CP866 encoding and returns the decoded string.
    /// </summary>
    /// <param name="bytes">The byte array to be decoded.</param>
    /// <returns>The decoded string.</returns>
    public static string DecodeCP866(byte[] bytes)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var b in bytes)
        {
            if (b < 128)
            {
                builder.Append((char)b);
            }
            else
            {
                builder.Append(CP866SHARED[b - 128]);
            }
        }
        return builder.ToString();
    }

    /// <summary>
    /// Integer raqamni Bin string ga o'tkazish
    /// </summary>
    /// <param name="value">Integer raqam</param>
    /// <param name="bits">Bitlar soni</param>
    /// <returns></returns>
    public static string IntToBinStr(int value, int bits)
    {
        return Convert.ToString(value, 2).PadLeft(bits, '0');
    }

    /// <summary>
    /// Converts an integer value to a binary array representation.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <param name="bits">The number of bits in the binary representation.</param>
    /// <returns>A boolean array representing the binary value of the input integer.</returns>
    public static bool[] IntToBinArray(int value, int bits)
    {
        string binaryStr = IntToBinStr(value, bits);
        return binaryStr.Select(c => c == '1').ToArray();
    }

    /// <summary>
    /// Decodes a string encoded in Win1251 encoding.
    /// </summary>
    /// <param name="str">The string to be decoded.</param>
    /// <returns>The decoded string.</returns>
    public static string DecodeWin1251(string str)
    {
        var res = System.Text.RegularExpressions.Regex.Unescape(str);
        var bytes = res.Select(ch => (byte)ch).ToArray();
        var enc = Encoding.GetEncoding(1251);
        return enc.GetString(bytes);
    }

    /// <summary>
    /// Calculates the number of set bits (1-bits) in the provided byte.
    /// </summary>
    /// <param name="b">The byte value to count the set bits in.</param>
    /// <returns>The count of set bits in the byte.</returns>
    public static int SumOneBits(byte b)
    {
        var sum = 0;

        for (var i = 0; i < 8; i++)
        {
            sum += (b >> i) & 1;
        }
        return sum;
    }

    /// <summary>
    /// Converts a byte to its 7-bit representation with optional parity bit.
    /// </summary>
    /// <param name="b">The byte to be converted.</param>
    /// <param name="parity">
    /// 'n' or 'N' for no parity,
    /// 'e' or 'E' for even parity,
    /// 'o' or 'O' for odd parity.
    /// </param>
    /// <returns>The converted byte with the 7-bit representation and optional parity bit.</returns>
    public static byte ConvertTo7Bit(byte b, char parity = 'N')
    {
        int sum;

        switch (parity)
        {
            case 'e':
            case 'E':
            case 'o':
            case 'O':
                sum = SumOneBits(b);
                break;
            default:
                return (byte)(b & 0x7F);
        }

        bool checkByte = sum % 2 != 0;

        if (parity == 'o' || parity == 'O') checkByte = !checkByte;

        if (checkByte)
        {
            return (byte)(b | 0b10000000);
        }
        else
        {
            return (byte)(b & 0x7F);
        }
    }

    /// <summary>
    /// Baytlarni 7 bit qismini qoldirish
    /// </summary>
    /// <param name="bytes">byte massiv</param>
    /// <param name="parity">
    /// 'n' yoki 'N' - None,
    /// 'e' yoki 'E' - Even,
    /// 'o' yoki 'O' - Odd
    /// </param>
    /// <returns>byte massiv</returns>
    public static byte[] ConvertTo7Bit(this byte[] bytes, char parity = 'N')
    {
        // return bytes.Select(b => ConvertTo7Bit(b, parity)).ToArray();
        var result = new byte[bytes.Length];
        bytes.CopyTo(result, 0);
        ConvertTo7Bit(result.AsSpan(), parity);
        return result;
    }

    public static void ConvertTo7Bit(Span<byte> bytes, char parity = 'N')
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = ConvertTo7Bit(bytes[i], parity);
        }
    }

    /// <summary>
    /// Converts a string value to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value, or the default value for the type if conversion fails.</returns>
    /// <remarks>
    /// This method first checks if the type implements <see cref="IConvertible"/>,
    /// and if so, uses <see cref="Convert.ChangeType(object, Type)"/> to perform the conversion.
    /// If the type does not implement <see cref="IConvertible"/>, it uses a <see cref="TypeConverter"/>
    /// to perform the conversion, if one is available.
    /// </remarks>
    public static T? ConvertTo<T>(string? value)
    {
        if (value == null)
        {
            return default;
        }

        try
        {
            // Check if the type implements IConvertible
            if (typeof(IConvertible).IsAssignableFrom(typeof(T)))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                // Use TypeConverter for types that do not implement IConvertible
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (T?)converter.ConvertFromString(value);
                }
                else
                {
                    throw new InvalidOperationException($"No type converter found for type {typeof(T).Name}");
                }
            }
        }
        catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)
        {
            throw new InvalidOperationException($"Cannot convert '{value}' to {typeof(T).Name}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Baytlarni <see cref="byteOrder"/> da berilgan bo'yicha tartiblash
    /// </summary>
    /// <param name="span">Baytlar massiv</param>
    /// <param name="byteOrder">Baytlar ketmaketligi. (M: 0123)</param>
    /// <returns>Tartiblangan baytlar massiv</returns>
    public static Span<byte> GetOrdered(Span<byte> span, string? byteOrder)
    {
        if (byteOrder?.Length == 0 || span.Length != byteOrder?.Length) return span;
        var result = new List<byte>();
        foreach (var ch in byteOrder)
        {
            if (!char.IsDigit(ch))
            {
                throw new($"Invalid byte order {byteOrder}");
            }

            result.Add(span[byte.Parse(new string(ch, 1))]);
        }
        return result.ToArray();
    }

    /// <summary>
    /// Converts a byte array to a boolean array where each element is a bit of the original array.
    /// The boolean array is ordered by the least significant bit first by default. If the reverse parameter is true, the bits are ordered by the most significant bit first.
    /// </summary>
    /// <param name="span">The byte array to convert.</param>
    /// <param name="reverse">Whether to reverse the bit order. Default is false.</param>
    /// <returns>The boolean array.</returns>
    public static bool[] ByteArrayToBoolArray(Span<byte> span, bool reverse = false)
    {
        var result = new bool[span.Length * 8];
        for (int i = 0; i < span.Length; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var index = reverse ? 7 - j : j;
                result[index + i * 8] = (span[i] & (1 << j)) != 0;
            }
        }
        return result;
    }

    /// <summary>
    /// Converts a boolean array to a byte array.
    /// </summary>
    /// <param name="values">The boolean array to convert.</param>
    /// <param name="reverse">If set to <c>true</c>, the bits in each byte will be reversed.</param>
    /// <returns>The byte array representation of the boolean array.</returns>
    public static byte[] BoolArrayToByteArray(Span<bool> values, bool reverse = false)
    {
        var result = new byte[values.Length / 8 + (values.Length % 8 == 0 ? 0 : 1)];
        for (int i = 0; i < values.Length; i++)
        {
            if (reverse)
            {
                result[i / 8] |= (byte)(values[i] ? 1 << (7 - i % 8) : 0);
            }
            else
            {
                result[i / 8] |= (byte)(values[i] ? 1 << i % 8 : 0);
            }
        }
        return result;
    }
}
