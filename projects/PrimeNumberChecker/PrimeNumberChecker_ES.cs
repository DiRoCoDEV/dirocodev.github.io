using System;

class Program
{
    static void Main()
    {
        for (int i = 1; i <= 100; i++)
        {
            if (IsPrimeNumber(i))
            {
                Console.WriteLine(i);
            }
        }
        
        Console.WriteLine();
        Console.WriteLine("List:");
        Console.WriteLine();
        
        for (int i = 1; i <= 100; i++)
        {
            if (IsPrimeNumber(i))
            {
                // Usando string.Format en lugar de $
                Console.WriteLine(string.Format("{0} is prime", i));
            }
            else
            {
                // También puedes usar concatenación directa
                Console.WriteLine(i + " isn't prime");
            }
        }
        Console.WriteLine("Presiona ENTER para salir");
        Console.ReadLine();
    }

    static bool IsPrimeNumber(int n)
    {
        if (n <= 1)
        {
            return false;
        }
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}