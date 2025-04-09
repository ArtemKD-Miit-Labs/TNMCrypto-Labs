using System.Numerics;
using System.Security.Cryptography;

internal class Program
{
    private static int JacobiSymbol(BigInteger a, BigInteger n)
    {
        if (n <= 0 || n % 2 == 0)
            throw new ArgumentException("n должно быть нечётным положительным числом");

        a = a % n;
        int t = 1;
        BigInteger r;

        while (a != 0)
        {
            while (a % 2 == 0)
            {
                a /= 2;
                r = n % 8;
                if (r == 3 || r == 5)
                    t = -t;
            }
            r = n;
            n = a;
            a = r;
            if (a % 4 == 3 && n % 4 == 3)
                t = -t;
            a = a % n;
        }

        return n == 1 ? t : 0;
    }

    public static bool IsProbablePrime(BigInteger n, int iterations = 10)
    {
        if (n == 2)
            return true;
        if (n < 2 || n % 2 == 0)
            return false;

        using (var rng = RandomNumberGenerator.Create())
        {
            for (int i = 0; i < iterations; i++)
            {
                BigInteger a = GenerateRandomBigInteger(2, n - 1, rng);
                int jacobi = JacobiSymbol(a, n);
                BigInteger modPow = BigInteger.ModPow(a, (n - 1) / 2, n);

                if (jacobi == 0 || modPow != (jacobi + n) % n)
                    return false;
            }
        }

        return true;
    }

    private static BigInteger GenerateRandomBigInteger(BigInteger minValue, BigInteger maxValue, RandomNumberGenerator rng)
    {
        byte[] bytes = maxValue.ToByteArray();
        BigInteger result;

        do
        {
            rng.GetBytes(bytes);
            bytes[bytes.Length - 1] &= 0x7F;
            result = new BigInteger(bytes);
        } while (result < minValue || result > maxValue);

        return result;
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Тест Соловея-Штрассена на простоту числа");
        Console.Write("Введите число для проверки: ");
        BigInteger number = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("Empty input"));

        Console.Write("Введите количество итераций (по умолчанию 10): ");
        var iterationsInput = Console.ReadLine();
        int iterations = string.IsNullOrEmpty(iterationsInput) ? 10 : int.Parse(iterationsInput);

        bool isPrime = IsProbablePrime(number, iterations);

        Console.WriteLine($"Число {number} {(isPrime ? "вероятно простое" : "составное")}");

        // Проверка некоторых известных чисел
        BigInteger[] testNumbers = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 561, 1105, 1729 };
        Console.WriteLine("\nПроверка тестовых чисел:");
        foreach (var num in testNumbers)
        {
            Console.WriteLine($"{num}: {(IsProbablePrime(num, 10) ? "Простое" : "Составное")}");
        }
    }
}