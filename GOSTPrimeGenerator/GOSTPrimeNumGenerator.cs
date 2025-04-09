using System.Numerics;
using System.Security.Cryptography;

namespace GOSTPrimeGenerator
{
    public class GOSTPrimeNumGenerator
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public static BigInteger GeneratePrime(int bitLength, int millerRabinRounds = 30)
        {
            if (bitLength < 2)
                throw new ArgumentException("Bit length must be at least 2.", nameof(bitLength));

            while (true)
            {
                var candidate = GenerateRandomOddBigInteger(bitLength);

                if (IsProbablyPrime(candidate, millerRabinRounds))
                    return candidate;
            }
        }

        private static BigInteger GenerateRandomOddBigInteger(int bitLength)
        {
            int byteLength = (bitLength + 7) / 8;
            byte[] bytes = new byte[byteLength];
            rng.GetBytes(bytes);

            // Установка старшего бита, чтобы число было нужной длины
            int mostSignificantBit = 1 << ((bitLength - 1) % 8);
            bytes[0] |= (byte)mostSignificantBit;

            // Установка младшего бита, чтобы число было нечётным
            bytes[^1] |= 0x01;

            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }

        private static bool IsProbablyPrime(BigInteger n, int rounds)
        {
            if (n < 2)
                return false;
            if (n == 2 || n == 3)
                return true;
            if (n % 2 == 0)
                return false;

            BigInteger d = n - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            byte[] buffer = new byte[n.GetByteCount()];
            for (int i = 0; i < rounds; i++)
            {
                BigInteger a;
                do
                {
                    rng.GetBytes(buffer);
                    a = new BigInteger(buffer, isUnsigned: true, isBigEndian: true);
                }
                while (a < 2 || a >= n - 2);

                BigInteger x = BigInteger.ModPow(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;

                bool continueOuter = false;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == n - 1)
                    {
                        continueOuter = true;
                        break;
                    }
                }

                if (continueOuter)
                    continue;

                return false;
            }

            return true;
        }
    }
}
