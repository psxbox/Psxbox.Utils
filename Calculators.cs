// Calculators class by Psxbox
// E-mail: adhamyu83@gmail.com

using System.Text;

namespace Psxbox.Utils
{
    public static class Calculators
    {
        /// <summary>
        /// Calculates the XOR of all the elements in the given byte array.
        /// </summary>
        /// <param name="arr">The byte array to calculate the XOR for.</param>
        /// <returns>The XOR result of all the elements in the byte array.</returns>
        public static int CalcXOR(byte[] arr)
        {
            int result = 0;
            foreach (var item in arr)
            {
                result ^= item;
            }
            return result;
        }

        /// <summary>
        /// Calculates the XOR value of a given string.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>The XOR value of the input string.</returns>
        public static int CalcXOR(string str)
        {
            return CalcXOR(Encoding.ASCII.GetBytes(str));
        }

        /// <summary>
        /// Calculates the Block Check Character (BCC) for the given byte array.
        /// </summary>
        /// <param name="msg">The byte array for which the BCC needs to be calculated.</param>
        /// <param name="res">The initial value of the BCC. Defaults to 0.</param>
        /// <returns>The calculated BCC value as a byte.</returns>
        public static byte CalcBCC(byte[] msg, byte res = 0)
        {
            byte result = res;
            foreach (byte b in msg)
                result ^= b;
            return result;
        }

        /// <summary>
        /// Calculates the 7-bit sum of the given byte array.
        /// </summary>
        /// <param name="arr">Byte array to calculate the 7-bit sum for.</param>
        /// <returns>7-bit sum of the given byte array.</returns>
        public static byte Cacl7BitSum(byte[] arr)
        {
            ushort sum = 0;
            foreach (byte b in arr)
                sum += b;
            return (byte)(sum & 0x7F);
        }

        // Span version of Cacl7BitSum
        public static byte Cacl7BitSum(ReadOnlySpan<byte> arr)
        {
            ushort sum = 0;
            foreach (byte b in arr)
                sum += b;
            return (byte)(sum & 0x7F);
        }

        /// <summary>
        /// Calculates the one's complement of a given unsigned short integer.
        /// </summary>
        /// <param name="n">The unsigned short integer to perform the one's complement on.</param>
        /// <returns>The one's complement of the given unsigned short integer.</returns>
        public static ushort OnesComplement(ushort n)
        {
            List<int> v = new();

            // convert to binary representation
            while (n != 0)
            {
                v.Add(n % 2);
                n = (ushort)Math.Floor(n / 2m);
            }
            v.Reverse();

            // change 1's to 0 and 0's to 1
            for (var i = 0; i < v.Count; i++)
            {
                if (v[i] == 0)
                    v[i] = 1;
                else
                    v[i] = 0;
            }

            // convert back to number representation
            var two = 1;
            for (var i = v.Count - 1; i >= 0; i--)
            {
                n = (ushort)(n + v[i] * two);
                two *= 2;
            }
            return n;
        }

        /// <summary>
        /// Calculates the modulo 256 of the sum of all elements in the given byte array.
        /// </summary>
        /// <param name="arr">The byte array to calculate the modulo 256 for.</param>
        /// <returns>The modulo 256 result of the sum of all elements in the byte array.</returns>
        public static byte Modulo256(byte[] arr)
        {
            var result = (byte)(arr.Sum(x => x) % 256);
            return result;
        }

        public static byte Modulo256(IEnumerable<byte> arr)
        {
            var result = (byte)(arr.Sum(x => x) % 256);
            return result;
        }

        /// <summary>
        /// Calculates the CRC16 of the given byte array.
        /// </summary>
        /// <param name="data">The byte array to calculate the CRC16 for.</param>
        /// <returns>The CRC16 result of the given byte array.</returns>
        public static ushort CalcModbusCRC(byte[] data)
        {
            // ushort crc = 0xFFFF; // Инициализируем значение CRC

            // foreach (byte b in data)
            // {
            //     crc ^= b; // XOR с байтом данных

            //     for (int i = 0; i < 8; i++)
            //     {
            //         if ((crc & 0x0001) != 0)
            //         {
            //             crc >>= 1;
            //             crc ^= 0xA001; // Полином MODBUS
            //         }
            //         else
            //         {
            //             crc >>= 1;
            //         }
            //     }
            // }

            // // Меняем байты местами для соответствия спецификации MODBUS
            // crc = (ushort)((crc << 8) | (crc >> 8));

            // return crc;

            return CalcModbusCRC(data.AsSpan());
        }

        // span version of CalcModbusCRC
        public static ushort CalcModbusCRC(ReadOnlySpan<byte> data)
        {
            ushort crc = 0xFFFF; // Инициализируем значение CRC

            foreach (byte b in data)
            {
                crc ^= b; // XOR с байтом данных

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001; // Полином MODBUS
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            // Меняем байты местами для соответствия спецификации MODBUS
            crc = (ushort)((crc << 8) | (crc >> 8));

            return crc;
        }

        /// <summary>
        /// Calculates the Longitudinal Redundancy Check (LRC) of the given byte array.
        /// </summary>
        /// <param name="request">The byte array to calculate the LRC for.</param>
        /// <param name="offset">The starting index of the segment of the byte array to calculate the LRC for.</param>
        /// <param name="requestLength">The length of the segment of the byte array to calculate the LRC for.</param>
        /// <returns>The LRC result of the given byte array segment.</returns>
        public static byte LRC(byte[] request, int offset, int requestLength)
        {
            byte lrc = 0;
            for (int i = offset; i < offset + requestLength; i++)
            {
                lrc += request[i];
            }
            return (byte)-lrc;
        }

        // Span version of LRC
        public static byte LRC(ReadOnlySpan<byte> request, int offset, int requestLength)
        {
            byte lrc = 0;
            for (int i = offset; i < offset + requestLength; i++)
            {
                lrc += request[i];
            }
            return (byte)-lrc;
        }
    }
}