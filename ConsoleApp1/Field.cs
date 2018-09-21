using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	class Field
	{
		Cell[] Play = new Cell[81];
		//internal static Cell[] Area = new Cell[10];
		List<HCell> History = new List<HCell>();
		public List<HCell> Updates = new List<HCell>();

		public Field()
		{
			for (int i = 0; i < 100; i++)
			{
				Play[i] = new Cell();
			}
		}

		public void Clear()
		{
			for (int i = 0; i < 100; i++)
			{
				Play[i].ClearCell();
			}
		}

		public int Index(int col, int row)
		{
			return col + row * 10;
		}

		public Cell GetCell(int col, int row)
		{
			return Play[Index(col, row)];
		}

		public Cell SetCell(int col, int row, char content, Cell.CellSource source)
		{
			Cell cell = GetCell(col, row);

			cell.Content = content;
			cell.Source = source;

			Update(col, row);

			return cell;
		}

		public void Update(int col, int row)
		{
			UpdateColumn(col, row);
			UpdateRow(col, row);
			UpdateArea(col, row);
		}

		internal void UpdateColumn(int col, int row)
		{
			Cell cell = GetCell(col, row);
			char UpdateContent = cell.Content;

			for (int i = 0; i < 9; i++)
			{
				if (i != col)
				{
					cell = GetCell(i, row);
					cell.RemoveFromOpen(UpdateContent);
					if (cell.Reset())
					{
						HCell hCell = new HCell() { Col = i, Row = row };
						Updates.Add(hCell);
					}
				}
			}
		}

		internal void UpdateRow(int col, int row)
		{
			Cell cell = GetCell(col, row);
			char UpdateContent = cell.Content;

			for (int i = 0; i < 9; i++)
			{
				if (i != row)
				{
					cell = GetCell(col, i);
					cell.RemoveFromOpen(UpdateContent);
					if (cell.Reset())
					{
						HCell hCell = new HCell() { Col = col, Row = i };
						Updates.Add(hCell);
					}
				}
			}
		}

		internal void UpdateArea(int col, int row)
		{
			Cell cell = GetCell(col, row);
			char UpdateContent = cell.Content;

			int AreaCol = (col / 3) * 3;
			int AreaRow = (row / 3) * 3;

			for (int j = 0; j < 3; j++)
			{
				for (int i = 0; i < 3; i++)
				{
					if (AreaCol + i != col && AreaRow + j != row)
					{
						cell = GetCell(AreaCol + i, AreaRow + j);
						cell.RemoveFromOpen(UpdateContent);
						if (cell.Reset())
						{
							HCell hCell = new HCell() { Col = AreaCol + i, Row = AreaRow + i };
							Updates.Add(hCell);
						}
					}
				}
			}
		}//UpdateArea
	}
}
