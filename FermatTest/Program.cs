using System.Numerics;

internal class Program
{
    public static bool IsProbablePrime(BigInteger n, int iterations = 5)
    {
        if (n == 2 || n == 3)
            return true;
        if (n < 2 || n % 2 == 0)
            return false;

        Random random = new Random();

        for (int i = 0; i < iterations; i++)
        {
            BigInteger a = GenerateRandomBigInteger(2, n - 2, random);

            if (BigInteger.ModPow(a, n - 1, n) != 1)
                return false;
        }

        return true;
    }

    private static BigInteger GenerateRandomBigInteger(BigInteger minValue, BigInteger maxValue, Random random)
    {
        byte[] bytes = maxValue.ToByteArray();
        BigInteger result;

        do
        {
            random.NextBytes(bytes);
            bytes[bytes.Length - 1] &= 0x7F;
            result = new BigInteger(bytes);
        } while (result < minValue || result > maxValue);

        return result;
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Тест Ферма на простоту числа");
        Console.Write("Введите число для проверки: ");
        BigInteger number = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("Empty input"));

        Console.Write("Введите количество итераций (по умолчанию 5): ");
        var iterationsInput = Console.ReadLine();
        var iterations = string.IsNullOrEmpty(iterationsInput) ? 5 : int.Parse(iterationsInput);

        bool isPrime = IsProbablePrime(number, iterations);

        Console.WriteLine($"Число {number} {(isPrime ? "вероятно простое" : "составное")}");
    }
}