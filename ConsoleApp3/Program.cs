using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
	class Program
	{
		public static Field Puzzle = new Field();

		static void Main(string[] args)
		{
				if (Puzzle.ImportPreFilledCells(@"C:\Temp\Soducko 2018-02-18 1701.json"))
				{
					//ShowField();
				}

			var Area = Puzzle.GetRow(0);

			Console.WriteLine(Area[0].Content);

			Console.Write("Press any key... ");
			Console.ReadKey();
		}
	}
}
