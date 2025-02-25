using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Введите первое число: ");
        BigInteger x = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("First input is null"));

        Console.Write("Введите второе число: ");
        BigInteger y = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("Second input is null"));

        BigInteger result = Karatsuba(x, y);
        Console.WriteLine($"Результат умножения: {result}");
    }

    public static BigInteger Karatsuba(BigInteger x, BigInteger y)
    {
        // Если числа маленькие, умножаем напрямую
        if (x < 10 || y < 10)
            return x * y;

        // Определяем длину максимального числа
        int n = Math.Max(x.ToString().Length, y.ToString().Length);
        int m = n / 2;

        // Разделяем x и y на две части
        BigInteger a = x / BigInteger.Pow(10, m);
        BigInteger b = x % BigInteger.Pow(10, m);
        BigInteger c = y / BigInteger.Pow(10, m);
        BigInteger d = y % BigInteger.Pow(10, m);

        // Вычисляем три произведения
        BigInteger p1 = Karatsuba(a, c);
        BigInteger p2 = Karatsuba(b, d);
        BigInteger p3 = Karatsuba(a + b, c + d) - p1 - p2;

        // Собираем результат
        return p1 * BigInteger.Pow(10, 2 * m) + p3 * BigInteger.Pow(10, m) + p2;
    }
}