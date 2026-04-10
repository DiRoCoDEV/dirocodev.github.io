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
                // Using string.Format instead of $
                Console.WriteLine(string.Format("{0} is prime", i));
            }
            else
            {
                // Also can use direct concatenation
                Console.WriteLine(i + " isn't prime");
            }
        }

        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();
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