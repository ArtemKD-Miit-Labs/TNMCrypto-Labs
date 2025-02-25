using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Введите первое число: ");
        BigInteger x = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("First input is null"));

        Console.Write("Введите второе число: ");
        BigInteger y = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("Second input is null"));

        BigInteger result = ToomCookMultiply(x, y);
        Console.WriteLine($"Результат умножения: {result}");
    }

    public static BigInteger ToomCookMultiply(BigInteger a, BigInteger b)
    {
        int n = Math.Max(a.ToString().Length, b.ToString().Length);
        if (n < 3)
        {
            return a * b;
        }

        int m = n / 3;

        BigInteger a0 = a % BigInteger.Pow(10, m);
        BigInteger a1 = (a / BigInteger.Pow(10, m)) % BigInteger.Pow(10, m);
        BigInteger a2 = a / BigInteger.Pow(10, 2 * m);

        BigInteger b0 = b % BigInteger.Pow(10, m);
        BigInteger b1 = (b / BigInteger.Pow(10, m)) % BigInteger.Pow(10, m);
        BigInteger b2 = b / BigInteger.Pow(10, 2 * m);

        BigInteger p0 = a0 * b0;
        BigInteger p1 = (a0 + a1 + a2) * (b0 + b1 + b2);
        BigInteger p2 = (a0 + a1 * 2 + a2 * 4) * (b0 + b1 * 2 + b2 * 4);
        BigInteger p3 = (a0 + a1 * 3 + a2 * 9) * (b0 + b1 * 3 + b2 * 9);
        BigInteger p4 = (a2) * (b2);

        BigInteger r0 = p0;
        BigInteger r4 = p4;
        BigInteger r3 = (p3 - p1) / 2;
        BigInteger r2 = (p2 - p1) / 2;
        BigInteger r1 = p1 - r0 - r4;
        r3 = (r3 - r1) / 2;
        r2 = r2 - r1 - r3;

        return r0 + (r1 * BigInteger.Pow(10, m)) + (r2 * BigInteger.Pow(10, 2 * m)) +
               (r3 * BigInteger.Pow(10, 3 * m)) + (r4 * BigInteger.Pow(10, 4 * m));
    }
}