using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Введите первое число: ");
        BigInteger x = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("First input is null"));

        Console.Write("Введите второе число: ");
        BigInteger y = BigInteger.Parse(Console.ReadLine() ?? throw new ArgumentNullException("Second input is null"));

        BigInteger result = SchonhageStrassen(x, y);
        Console.WriteLine($"Результат умножения: {result}");
    }

    public static BigInteger SchonhageStrassen(BigInteger a, BigInteger b)
    {
        int size = Math.Max(a.ToByteArray().Length, b.ToByteArray().Length);
        int n = 1;
        while (n < 2 * size) n *= 2;

        Complex[] fa = new Complex[n];
        Complex[] fb = new Complex[n];

        FillComplexArray(fa, a);
        FillComplexArray(fb, b);

        FFT(fa, false);
        FFT(fb, false);

        for (int i = 0; i < n; i++)
            fa[i] *= fb[i];

        FFT(fa, true);

        return ConvertComplexArrayToBigInteger(fa);
    }

    private static void FillComplexArray(Complex[] arr, BigInteger num)
    {
        byte[] bytes = num.ToByteArray();
        for (int i = 0; i < bytes.Length; i++)
            arr[i] = new Complex(bytes[i], 0);
    }

    private static void FFT(Complex[] a, bool invert)
    {
        int n = a.Length;
        for (int i = 1, j = 0; i < n; i++)
        {
            int bit = n >> 1;
            for (; j >= bit; bit >>= 1)
                j -= bit;
            j += bit;
            if (i < j)
                (a[i], a[j]) = (a[j], a[i]);
        }

        for (int len = 2; len <= n; len <<= 1)
        {
            double angle = 2 * Math.PI / len * (invert ? -1 : 1);
            Complex wlen = new Complex(Math.Cos(angle), Math.Sin(angle));

            for (int i = 0; i < n; i += len)
            {
                Complex w = new Complex(1, 0);
                for (int j = 0; j < len / 2; j++)
                {
                    Complex u = a[i + j], v = a[i + j + len / 2] * w;
                    a[i + j] = u + v;
                    a[i + j + len / 2] = u - v;
                    w *= wlen;
                }
            }
        }

        if (invert)
        {
            for (int i = 0; i < n; i++)
                a[i] /= n;
        }
    }

    private static BigInteger ConvertComplexArrayToBigInteger(Complex[] arr)
    {
        BigInteger result = 0;
        BigInteger factor = 1;
        foreach (var c in arr)
        {
            result += (BigInteger)Math.Round(c.Real) * factor;
            factor <<= 8;
        }
        return result;
    }
}