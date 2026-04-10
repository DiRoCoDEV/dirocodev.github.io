using System;

namespace WordRepeter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pedir palabra
            Console.WriteLine("Entre la palabra:");
            string word = Console.ReadLine();

            int times;

            // Pedir número hasta que sea válido
            while (true)
            {
                Console.WriteLine("Entre el numero de veces que se va a repetir:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out times) && times > 0)
                {
                    break;
                }

                Console.WriteLine("Error: introduce un número entero positivo.");
            }

            // Repetir palabra
            for (int i = 0; i < times; i++)
            {
                Console.WriteLine((i + 1) + ". " + word);
            }

            // Evita que la consola se cierre
            Console.WriteLine("Presiona ENTER para salir...");
            Console.ReadLine();
        }
    }
}