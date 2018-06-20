using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace GeohashCross.Model.Services
{
    public static class Hasher
    {
        public static async Task<List<Position>> GetCoordinates(DateTime? date = null, double? lat = null, double? lon = null, bool loadNeighbours = false)
        {
            date = date.HasValue ? date.Value.Date : DateTime.UtcNow.Date;

            var dija = await Webclient.GetDowJones(date);

            var offsets = calculateOffsets(date.Value, dija);

            if (lat == null || lon == null)
            {
                var currentLocation = await LocationService.GetLocation();

                lat = currentLocation.Latitude;


                lon = currentLocation.Longitude;


            }
            lat = lat > 0 ? Math.Floor(lat.Value) : Math.Ceiling(lat.Value);
            lon = lon > 0 ? Math.Floor(lon.Value) : Math.Ceiling(lon.Value);


            int latInt = Convert.ToInt32(lat);
            var lonInt = Convert.ToInt32(lon);

            var latDecString = $"{latInt}.{offsets[0]}";
            var lonDecString = $"{lonInt}.{offsets[1]}";

            var finalLat = Double.Parse(latDecString);
            var finalLon = Double.Parse(lonDecString);

            var pos = new Position(finalLat, finalLon);

            var locations = new List<Position>{pos};

            if(loadNeighbours)
            {
                locations.Add(new Position(pos.Latitude - 1, pos.Longitude - 1));//topLeft
                locations.Add(new Position(pos.Latitude, pos.Longitude - 1));//middleLeft
                locations.Add(new Position(pos.Latitude + 1, pos.Longitude - 1));//bottomLeft
                locations.Add(new Position(pos.Latitude - 1, pos.Longitude));//topCentre
                locations.Add(new Position(pos.Latitude + 1, pos.Longitude));//bottomCenter
                locations.Add(new Position(pos.Latitude - 1, pos.Longitude + 1));//topRight
                locations.Add(new Position(pos.Latitude, pos.Longitude + 1));//RightCenter
                locations.Add(new Position(pos.Latitude + 1, pos.Longitude + 1));//BottomRight
            }


            return locations;
        }

        private static string[] calculateOffsets(DateTime date, string djia)
        {
            string dateString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string fullString = dateString + '-' + djia;
            var md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(fullString);

            byte[] result = md5.ComputeHash(inputBytes);



            //byte[] result = MD5Bad.Digest(fullString);

            ulong part1 = ((ulong)result[0x0] << 0x38)
                + ((ulong)result[0x1] << 0x30)
                + ((ulong)result[0x2] << 0x28)
                + ((ulong)result[0x3] << 0x20)
                + ((ulong)result[0x4] << 0x18)
                + ((ulong)result[0x5] << 0x10)
                + ((ulong)result[0x6] << 0x08)
                + ((ulong)result[0x7] << 0x00);
            ulong part2 = ((ulong)result[0x8] << 0x38)
                + ((ulong)result[0x9] << 0x30)
                + ((ulong)result[0xA] << 0x28)
                + ((ulong)result[0xB] << 0x20)
                + ((ulong)result[0xC] << 0x18)
                + ((ulong)result[0xD] << 0x10)
                + ((ulong)result[0xE] << 0x08)
                + ((ulong)result[0xF] << 0x00);
            string appendix1 = ((part1 / 2.0) / (long.MaxValue + (ulong)1)).ToString(CultureInfo.InvariantCulture).Substring("0.".Length); // Some tricks are required to divide by ulong.MaxValue + 1
            string appendix2 = ((part2 / 2.0) / (long.MaxValue + (ulong)1)).ToString(CultureInfo.InvariantCulture).Substring("0.".Length);


            Debug.WriteLine($"appendinx 1: {appendix1}");
            Debug.WriteLine($"appendinx 2: {appendix2}");
            return new[] { appendix1, appendix2 };
        }
    }

    public static class MD5Bad
    {
        private static readonly uint[] T = new uint[65];

        static MD5Bad()
        {
            T[0] = 0; // never to be accessed
            for (int i = 1; i <= 64; i++)
                T[i] = (uint)(0x100000000 * Math.Abs(Math.Sin(i)));
        }

        public static byte[] Digest(string input)
        {
            unchecked
            {
                List<byte> bytes = new List<byte>(Encoding.UTF8.GetBytes(input));

                #region Step 1. Append Padding Bits

                bytes.Add(0x80);

                while (bytes.Count % 64 != 56)
                    bytes.Add(0x00);

                #endregion

                #region Step 2. Append Length

                int length = Encoding.UTF8.GetByteCount(input) * 8;

                bytes.Add((byte)(length >> 00));
                bytes.Add((byte)(length >> 08));
                bytes.Add((byte)(length >> 16));
                bytes.Add((byte)(length >> 24));

                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);

                #region Construct M (not an own section in the specification)
                int N = bytes.Count / 4;
                uint[] M = new uint[N];
                for (int i = 0; i < M.Length; i++)
                    M[i] = (uint)(bytes[4 * i + 3] << 24) + (uint)(bytes[4 * i + 2] << 16) + (uint)(bytes[4 * i + 1] << 8) + (uint)(bytes[4 * i] << 0);
                #endregion

                #endregion

                #region Step 3. Initialize MD Buffer

                uint A = (uint)0x67452301;
                uint B = (uint)0xefcdab89;
                uint C = (uint)0x98badcfe;
                uint D = (uint)0x10325476;

                #endregion

                #region Step 4. Process Message in 16-Word Blocks

                // Process each 16-word block.
                for (int i = 0; i < N / 16; i++)
                {
                    // Copy block i uinto X
                    uint[] X = new uint[16];
                    Array.Copy(M, i * 16, X, 0, 16);

                    // Save A as AA, B as BB, C as CC, and D as DD.
                    uint AA = A;
                    uint BB = B;
                    uint CC = C;
                    uint DD = D;

                    // Round 1.
                    // Do the following 16 operations.
                    round1(ref A, B, C, D, 0, 7, 1, X); round1(ref D, A, B, C, 1, 12, 2, X); round1(ref C, D, A, B, 2, 17, 3, X); round1(ref B, C, D, A, 3, 22, 4, X);
                    round1(ref A, B, C, D, 4, 7, 5, X); round1(ref D, A, B, C, 5, 12, 6, X); round1(ref C, D, A, B, 6, 17, 7, X); round1(ref B, C, D, A, 7, 22, 8, X);
                    round1(ref A, B, C, D, 8, 7, 9, X); round1(ref D, A, B, C, 9, 12, 10, X); round1(ref C, D, A, B, 10, 17, 11, X); round1(ref B, C, D, A, 11, 22, 12, X);
                    round1(ref A, B, C, D, 12, 7, 13, X); round1(ref D, A, B, C, 13, 12, 14, X); round1(ref C, D, A, B, 14, 17, 15, X); round1(ref B, C, D, A, 15, 22, 16, X);

                    // Round 2.
                    // Do the following 16 operations.
                    round2(ref A, B, C, D, 1, 5, 17, X); round2(ref D, A, B, C, 6, 9, 18, X); round2(ref C, D, A, B, 11, 14, 19, X); round2(ref B, C, D, A, 0, 20, 20, X);
                    round2(ref A, B, C, D, 5, 5, 21, X); round2(ref D, A, B, C, 10, 9, 22, X); round2(ref C, D, A, B, 15, 14, 23, X); round2(ref B, C, D, A, 4, 20, 24, X);
                    round2(ref A, B, C, D, 9, 5, 25, X); round2(ref D, A, B, C, 14, 9, 26, X); round2(ref C, D, A, B, 3, 14, 27, X); round2(ref B, C, D, A, 8, 20, 28, X);
                    round2(ref A, B, C, D, 13, 5, 29, X); round2(ref D, A, B, C, 2, 9, 30, X); round2(ref C, D, A, B, 7, 14, 31, X); round2(ref B, C, D, A, 12, 20, 32, X);

                    // Round 3.
                    // Do the following 16 operations.
                    round3(ref A, B, C, D, 5, 4, 33, X); round3(ref D, A, B, C, 8, 11, 34, X); round3(ref C, D, A, B, 11, 16, 35, X); round3(ref B, C, D, A, 14, 23, 36, X);
                    round3(ref A, B, C, D, 1, 4, 37, X); round3(ref D, A, B, C, 4, 11, 38, X); round3(ref C, D, A, B, 7, 16, 39, X); round3(ref B, C, D, A, 10, 23, 40, X);
                    round3(ref A, B, C, D, 13, 4, 41, X); round3(ref D, A, B, C, 0, 11, 42, X); round3(ref C, D, A, B, 3, 16, 43, X); round3(ref B, C, D, A, 6, 23, 44, X);
                    round3(ref A, B, C, D, 9, 4, 45, X); round3(ref D, A, B, C, 12, 11, 46, X); round3(ref C, D, A, B, 15, 16, 47, X); round3(ref B, C, D, A, 2, 23, 48, X);

                    // Round 4.
                    // Do the following 16 operations.
                    round4(ref A, B, C, D, 0, 6, 49, X); round4(ref D, A, B, C, 7, 10, 50, X); round4(ref C, D, A, B, 14, 15, 51, X); round4(ref B, C, D, A, 5, 21, 52, X);
                    round4(ref A, B, C, D, 12, 6, 53, X); round4(ref D, A, B, C, 3, 10, 54, X); round4(ref C, D, A, B, 10, 15, 55, X); round4(ref B, C, D, A, 1, 21, 56, X);
                    round4(ref A, B, C, D, 8, 6, 57, X); round4(ref D, A, B, C, 15, 10, 58, X); round4(ref C, D, A, B, 6, 15, 59, X); round4(ref B, C, D, A, 13, 21, 60, X);
                    round4(ref A, B, C, D, 4, 6, 61, X); round4(ref D, A, B, C, 11, 10, 62, X); round4(ref C, D, A, B, 2, 15, 63, X); round4(ref B, C, D, A, 9, 21, 64, X);

                    /* Then perform the following additions. (That is increment each
                       of the four registers by the value it had before this block
                       was started.) */
                    A = A + AA;
                    B = B + BB;
                    C = C + CC;
                    D = D + DD;
                } // end of loop on i

                #endregion

                #region Step 5. Output

                return new[]{
                    (byte)(A >> 0), (byte)(A >> 8), (byte)(A >> 16), (byte)(A >> 24),
                    (byte)(B >> 0), (byte)(B >> 8), (byte)(B >> 16), (byte)(B >> 24),
                    (byte)(C >> 0), (byte)(C >> 8), (byte)(C >> 16), (byte)(C >> 24),
                    (byte)(D >> 0), (byte)(D >> 8), (byte)(D >> 16), (byte)(D >> 24)};

                #endregion
            }
        }

        #region FGHI functions (step 4)
        private static uint F(uint X, uint Y, uint Z)
        {
            unchecked
            {
                return (X & Y) | (~X & Z);
            }
        }
        private static uint G(uint X, uint Y, uint Z)
        {
            unchecked
            {
                return (X & Z) | (Y & ~Z);
            }
        }
        private static uint H(uint X, uint Y, uint Z)
        {
            unchecked
            {
                return X ^ Y ^ Z;
            }
        }
        private static uint I(uint X, uint Y, uint Z)
        {
            unchecked
            {
                return Y ^ (X | ~Z);
            }
        }
        #endregion

        #region Step functions (step 4, [abcd k s i])
        private static void round1(ref uint a, uint b, uint c, uint d, int k, int s, int i, uint[] X)
        {
            a = b + leftRotate((a + F(b, c, d) + X[k] + T[i]), s);
        }
        private static void round2(ref uint a, uint b, uint c, uint d, int k, int s, int i, uint[] X)
        {
            a = b + leftRotate((a + G(b, c, d) + X[k] + T[i]), s);
        }
        private static void round3(ref uint a, uint b, uint c, uint d, int k, int s, int i, uint[] X)
        {
            a = b + leftRotate((a + H(b, c, d) + X[k] + T[i]), s);
        }
        private static void round4(ref uint a, uint b, uint c, uint d, int k, int s, int i, uint[] X)
        {
            a = b + leftRotate((a + I(b, c, d) + X[k] + T[i]), s);
        }
        #endregion

        private static uint leftRotate(uint value, int offset)
        {
            return (value << offset) | (value >> (32 - offset));
        }
    }
}
