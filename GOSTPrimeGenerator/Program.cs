using GOSTPrimeGenerator;
using System.Numerics;
using System.Security.Cryptography;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Генерация простого числа по ГОСТ Р 34.10-1994");
        Console.Write("Введите битовую длину простого числа: ");
        int bitLength = int.Parse(Console.ReadLine());

        var prime = GOSTPrimeNumGenerator.GeneratePrime(bitLength);
        Console.WriteLine($"Сгенерированное простое число ({bitLength} бит):");
        Console.WriteLine(prime);
        Console.WriteLine($"Проверка битовой длины: {prime.GetBitLength()} бит");
    }
}