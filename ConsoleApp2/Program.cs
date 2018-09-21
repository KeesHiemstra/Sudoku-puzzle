/* Sudoku puzzle solver
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver

{
	class Program
	{
		public static int Left = 0;
		public static int Top = 0;
		public static Field Puzzle;

		[STAThread]
		static void Main(string[] args)
		{
			Console.Title = "Sudoku puzzle solver";
			ConsoleKeyInfo Key = new ConsoleKeyInfo();
			byte Col = 0;
			byte Row = 0;
			Cell cell;
			string Path;
			int Steps = 0;
			Puzzle = new Field();

			ShowPuzzle();

			do
			{
				if (Puzzle.History.Count > 0)
				{
					Console.SetCursorPosition(0, Console.WindowHeight - 3);
					Console.Write("{0}: Col {1}, Row {2}, Content {3}",
						Puzzle.History.Count,
						Puzzle.History.Last().Col,
						Puzzle.History.Last().Row,
						Puzzle.History.Last().Content);
				}

				if (Puzzle.Updates.Count > 0)
				{
					Console.SetCursorPosition(40, Console.WindowHeight - 3);
					Console.Write("{0}: Col {1}, Row {2}, Content {3}                             ",
						Puzzle.Updates.Count,
						Puzzle.Updates.Last().Col,
						Puzzle.Updates.Last().Row,
						Puzzle.Updates.Last().Content);
				}

				while (Puzzle.Updates.Count > 0)
				{
					cell = Puzzle.Updates[0];
					Puzzle.SetCell(cell.Col, cell.Row, cell.Content, cell.Source);
					ShowCell(cell);
					Puzzle.Updates.Remove(cell);
				}
				Console.SetCursorPosition(0, Console.WindowHeight - 5);
				Console.Write("Steps: {0}", Steps);

				Console.SetCursorPosition(Left + Col * 2, Top + Row);

				Key = Console.ReadKey(true);

				switch (Key.Key)
				{
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
						ShowCell(cell);
						break;
					case ConsoleKey.Enter:
						Steps++;
						Console.SetCursorPosition(0, Console.WindowHeight - 4);
						Console.Write("Check run = {0} ", Puzzle.CheckOpenField());
						break;
					case ConsoleKey.P:
						Console.SetCursorPosition(40, 2);
						Console.Write("Content: {0} ", Puzzle.Play[Col + Row * 9].Content);
						Console.SetCursorPosition(40, 3);
						Console.Write("Peek: {0:9}         ", Puzzle.Play[Col + Row * 9].Open);
						break;
					case ConsoleKey.E:
						#region Export history
						Path = string.Format("C:\\Temp\\Sudoku {0}.json", DateTime.Now.ToString("yyyy-MM-dd HHmm"));

						SaveFileDialog saveFile = new SaveFileDialog
						{
							FileName = Path,
							Filter = "Json files (*.json)|*.json",
							FilterIndex = 0,
						};

						if (saveFile.ShowDialog() == DialogResult.OK)
						{
							Path = saveFile.FileName;
						}

						Puzzle.ExportPreFilledCells(Path);
						break;
					#endregion
					case ConsoleKey.I:
						#region Import history
						OpenFileDialog openFile = new OpenFileDialog
						{

						};

						if (openFile.ShowDialog() == DialogResult.OK)
						{
							if (Puzzle.ImportPreFilledCells(openFile.FileName))
							{
								ShowField();
							}
						}

						break;
					#endregion
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
		}

		static void ShowCell(Cell cell)
		{
			Console.SetCursorPosition(Left + cell.Col * 2, Top + cell.Row);
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

			for (byte row = 0; row < 9; row++)
			{
				for (byte col = 0; col < 9; col++)
				{
					cell = Puzzle.GetCell(col, row);
					ShowCell(cell);
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
	}//Program
}
