using System;

namespace WordRepeter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ask for word
            Console.WriteLine("Enter the word:");
            string word = Console.ReadLine();

            int times;

            // Ask for number until valid
            while (true)
            {
                Console.WriteLine("Enter the number of times to repeat:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out times) && times > 0)
                {
                    break;
                }

                Console.WriteLine("Error: enter a positive integer.");
            }

            // Repeat word
            for (int i = 0; i < times; i++)
            {
                Console.WriteLine((i + 1) + ". " + word);
            }

            // Prevent console from closing
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}