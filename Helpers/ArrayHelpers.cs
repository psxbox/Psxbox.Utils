using System.Text;

namespace Psxbox.Utils.Helpers
{
    public static class ArrayHelpers
    {
        public static int Search<T>(this T[] array, T[] search, bool fromEnd = false) where T : struct
        {
            if (fromEnd)
            {
                for (int i = array.Length - search.Length; i >= 0; i--)
                {
                    if (Match(array, search, i))
                    {
                        return i;
                    }
                }
                return -1;
            }
            else
            {
                for (int i = 0; i <= array.Length - search.Length; i++)
                {
                    if (Match(array, search, i))
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        private static bool Match<T>(T[] array, T[] search, int start) where T : struct
        {
            if (search.Length + start > array.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < search.Length; i++)
                {
                    if (!search[i].Equals(array[i + start]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Baytlarni <see cref="byteOrder"/> da berilgan bo'yicha tartiblash
        /// </summary>
        /// <typeparam name="T">Tip</typeparam>
        /// <param name="array"><see cref="T"/> tipli massiv</param>
        /// <param name="byteOrder">Baytlar ketmaketligi. (M: 0123)</param>
        /// <returns>Tartiblangan <see cref="T"/> tipli massiv</returns>
        public static T[] GetOrdered<T>(this T[] array, string? byteOrder)
        {
            if (byteOrder?.Length == 0 || array.Length != byteOrder?.Length) return array;
            var result = new List<T>();
            foreach (var ch in byteOrder)
            {
                if (!char.IsDigit(ch))
                {
                    throw new($"Invalid byte order {byteOrder}");
                }
                result.Add(array[byte.Parse(new string(ch, 1))]);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Baytlarni <see cref="byteOrder"/> da berilgan bo'yicha tartiblash
        /// </summary>
        public static byte[] GetOrderedBytes(this byte[] data, int startIndex, int count, string? byteOrder)
        {
            ReadOnlySpan<byte> span = data;
            
            if (startIndex + count > span.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Not enough data in the buffer");
            }

            var result = new byte[count];
            var slice = span.Slice(startIndex, count);

            // Agar byte order yo'q bo'lsa, to'g'ridan-to'g'ri nusxa ko'chirish
            if (string.IsNullOrEmpty(byteOrder))
            {
                slice.CopyTo(result);
                return result;
            }

            // Byte order ni bir marta tekshirish
            var order = byteOrder.ToLowerInvariant();
            
            // Little-endian va sistema ham little-endian bo'lsa - oddiy nusxa
            if ((order is "little-endian" or "le") && BitConverter.IsLittleEndian)
            {
                slice.CopyTo(result);
                return result;
            }

            // Big-endian va sistema big-endian bo'lsa - oddiy nusxa
            if ((order is "big-endian" or "be") && !BitConverter.IsLittleEndian)
            {
                slice.CopyTo(result);
                return result;
            }

            slice.CopyTo(result);

            // Byte order ni qo'llash
            switch (order)
            {
                case "big-endian" or "be":
                    Array.Reverse(result);
                    break;

                case "little-endian" or "le":
                    Array.Reverse(result);
                    break;

                case "mid-big-endian" or "mbe":
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(result);
                    }

                    // Word swap - 2 baytli chunk'larni almashtirish
                    if ((count & 1) == 0) // count % 2 == 0 ning tezroq versiyasi
                    {
                        for (int i = 0; i < count; i += 2)
                        {
                            (result[i], result[i + 1]) = (result[i + 1], result[i]);
                        }
                    }
                    break;

                case "mid-little-endian" or "mle":
                    if (!BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(result);
                    }

                    // Word swap
                    if ((count & 1) == 0)
                    {
                        for (int i = 0; i < count; i += 2)
                        {
                            (result[i], result[i + 1]) = (result[i + 1], result[i]);
                        }
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Byte array ni T tipga konvert qiliash
        /// </summary>
        /// <typeparam name="T">Tip</typeparam>
        /// <param name="array">Byte array</param>
        /// <param name="index">Qaysi indexdan boshlab olish</param>
        /// <returns><see cref="T"/> tipdagi qiymat</returns>
        public static T ConvertTo<T>(this byte[] array, int index = 0) where T : struct
        {
            return typeof(T) switch
            {
                Type t when t == typeof(short) => (T)(object)BitConverter.ToInt16(array, index),
                Type t when t == typeof(ushort) => (T)(object)BitConverter.ToUInt16(array, index),
                Type t when t == typeof(byte) => (T)(object)array[0],
                Type t when t == typeof(int) => (T)(object)BitConverter.ToInt32(array, index),
                Type t when t == typeof(uint) => (T)(object)BitConverter.ToUInt32(array, index),
                Type t when t == typeof(float) => (T)(object)BitConverter.ToSingle(array, index),
                Type t when t == typeof(double) => (T)(object)BitConverter.ToDouble(array, index),
                Type t when t == typeof(Half) => (T)(object)BitConverter.ToHalf(array, index),
                Type t when t == typeof(string) => (T)(object)Encoding.ASCII.GetString(array),
                _ => throw new("Unsupported value type: " + typeof(T)),
            };
        }

        public static bool IsEqual<T>(this T[] arr1, T[] arr2) where T : struct
        {
            if (arr1.Length != arr2.Length) return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (!arr1[i].Equals(arr2[i])) return false;
            }
            return true;
        }
    }
}
