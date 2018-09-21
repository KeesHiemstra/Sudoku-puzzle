using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	class Program
	{
		public static int Left = 0;
		public static int Top = 0;
		public static Field Puzzle = new Field();

		static void Main(string[] args)
		{
			Console.Title = "Soduko puzzle";
			ConsoleKeyInfo Key = new ConsoleKeyInfo();
			int Col = 0;
			int Row = 0;
			Cell cell;

			ShowPuzzle();

			do
			{
				Key = Console.ReadKey(true);

				switch (Key.Key)
				{
					case ConsoleKey.D0:
					case ConsoleKey.D1:
					case ConsoleKey.D2:
					case ConsoleKey.D3:
					case ConsoleKey.D4:
					case ConsoleKey.D5:
					case ConsoleKey.D6:
					case ConsoleKey.D7:
					case ConsoleKey.D8:
					case ConsoleKey.D9:
						cell = Puzzle.SetCell(Col, Row, Key.KeyChar, Cell.CellSource.Pre);
						ShowCell(Col, Row, cell);
						break;
					case ConsoleKey.LeftArrow:
						if (Col > 0)
						{
							Col--;
						}
						break;
					case ConsoleKey.UpArrow:
						if (Row > 0)
						{
							Row--;
						}
						break;
					case ConsoleKey.RightArrow:
						if (Col < 8)
						{
							Col++;
						}
						break;
					case ConsoleKey.DownArrow:
						if (Row < 8)
						{
							Row++;
						}
						break;
					default:
						Console.SetCursorPosition(0, Console.WindowHeight - 1);
						Console.Write("Key: {0}                    ", Key.Key);
						break;
				}
				Console.SetCursorPosition(Left + Col * 2, Top + Row);
			} while (Key.Key != ConsoleKey.Q);
		}//Main

		static void ShowCell(int col, int row, Cell cell)
		{
			Console.SetCursorPosition(Left + col * 2, Top + row);
			switch (cell.Source)
			{
				case Cell.CellSource.Empty:
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write(".");
					break;
				case Cell.CellSource.Pre:
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write(cell.Content);
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case Cell.CellSource.Rule:
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write(cell.Content);
					Console.ForegroundColor = ConsoleColor.White;
					break;
				default:
					break;
			}
		}

		static void ShowField()
		{
			Cell cell;

			for (int row = 0; row < 10; row++)
			{
				for (int col = 0; col < 10; col++)
				{
					cell = Puzzle.GetCell(col, row);
					ShowCell(col, row, cell);
				}
			}
			Console.SetCursorPosition(Left, Top);
		}

		static void ShowPuzzle()
		{
			Console.Clear();
			Console.WriteLine("   | 0 1 2 3 4 5 6 7 8");
			Console.WriteLine(" ======================");
			for (int i = 0; i < 9; i++)
			{
				Console.Write(" {0} | ", i);
				if (Left == 0 && Top == 0)
				{
					Left = Console.CursorLeft;
					Top = Console.CursorTop;
				}
				Console.WriteLine(". . . . . . . . .", i);
			}

			ShowField();
		}

		//static Field NewPuzzle(Field puzzle)
		//{
		//	if (puzzle != null)
		//	{
		//		puzzle = null;
		//	}
		//	puzzle = new Field();
		//	return puzzle;
		//}
	}//Program
}//ConsoleApp1
