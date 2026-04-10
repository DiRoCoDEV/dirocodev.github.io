using System;

namespace SoloLearn
{
	class Program
	{
		static void Main(string[] args)
		{
			for(int i = 0; i < 101; i++)
			{
				if(i % 3 == 0 && i % 5 == 0 && i != 0)
				{
					Console.WriteLine("3 & 5 multiple");

				} else if(i % 3 == 0 && i != 0)
				{
					Console.WriteLine("3 multiple");
				} else if(i % 5 == 0 && i != 0)
				{
					Console.WriteLine("5 multiple");
				}
				else
				{
					Console.WriteLine(i);
				}
				
				Console.WriteLine("");
			}
			Console.WriteLine("Presiona ENTER para salir");
			Console.ReadLine();
		}
	}
}
