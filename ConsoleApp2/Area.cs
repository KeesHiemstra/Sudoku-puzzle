using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
	class Area
	{
		private static Cell[] result = new Cell[10];
		public Cell[] Result
		{
			get => result;
			set => result = value;
		}

		public List<Cell> CountOpen()
		{
			List<Cell> Updates = new List<Cell>();
			CountOpen[] OpenCount = new CountOpen[10];
			string Open;

			//Initialize
			for (int i = 0; i < 10; i++)
			{
				OpenCount[i] = new CountOpen();
			}

			//Count open chars in Area(.Result)
			for (byte i = 0; i < 9; i++)
			{
				Open = Result[i].Open;
				for (int o = 0; o < Open.Length; o++)
				{
					OpenCount[Open[o] - '1'].Count++;
					OpenCount[Open[o] - '1'].Position = i;
				}
			}

			for (int r = 0; r < 10; r++)
			{
				if (OpenCount[r].Count == 1)
				{
					Result[OpenCount[r].Position].Content = (char)('1' + r);
					Result[OpenCount[r].Position].Source = Cell.CellSource.Rule;
					Updates.Add(Result[OpenCount[r].Position]);
				}
			}

			return Updates;
		}
	}

	class CountOpen
	{
		public byte Position { get; set; }
		public byte Count { get; set; }
	}
}
