using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Fibonacci
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare the variables
            Console.WriteLine("Escribe tu primer número");
            string num1string = Console.ReadLine();
            int num1 = Convert.ToInt32(num1string);
            Console.WriteLine("Escribe tu segundo número");
            string num2string = Console.ReadLine();
            int num2 = Convert.ToInt32(num2string);
            int result;
            //Write numbers and results
            //Logic
            Console.WriteLine(num1);
            Console.WriteLine(num2);
            for(int i = 0; i < 25; i++)
            {
                result = num1 + num2;
                Console.WriteLine(result);
                num1 = num2;
                num2 = result;
            }
        Console.WriteLine("Presiona ENTER para salir");
        Console.ReadLine();
        }
    }
}