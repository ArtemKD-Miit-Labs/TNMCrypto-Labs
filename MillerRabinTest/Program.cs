using System.Numerics;
using System.Security.Cryptography;

internal class Program
{
    public static bool IsProbablePrime(BigInteger n, int iterations = 10)
    {
        if (n == 2 || n == 3)
            return true;
        if (n < 2 || n % 2 == 0)
            return false;

        BigInteger d = n - 1;
        int s = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            s++;
        }

        using (var rng = RandomNumberGenerator.Create())
        {
            for (int i = 0; i < iterations; i++)
            {
                BigInteger a = GenerateRandomBigInteger(2, n - 2, rng);
                BigInteger x = BigInteger.ModPow(a, d, n);

                if (x == 1 || x == n - 1)
                    continue;

                for (int j = 0; j < s - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
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
        Console.WriteLine("Тест Миллера-Рабина на простоту числа");
        Console.Write("Введите число для проверки: ");
        BigInteger number = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("Empty input"));

        Console.Write("Введите количество итераций (по умолчанию 10): ");
        var iterationsInput = Console.ReadLine();
        var iterations = string.IsNullOrEmpty(iterationsInput) ? 10 : int.Parse(iterationsInput);

        bool isPrime = IsProbablePrime(number, iterations);

        Console.WriteLine($"Число {number} {(isPrime ? "вероятно простое" : "составное")}");

        BigInteger[] carmichaelNumbers = { 561, 1105, 1729, 2465, 2821, 6601, 8911, 10585, 15841, 29341 };
        Console.WriteLine("\nПроверка некоторых чисел Кармайкла:");
        foreach (var num in carmichaelNumbers)
        {
            var result = IsProbablePrime(num) ? "Простое" : "Составное";
            Console.WriteLine($"{num}: {result}");
        }
    }
}