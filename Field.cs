using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
	class Field
	{
		public Cell[] Play = new Cell[81];
		public List<Cell> History = new List<Cell>();
		public List<Cell> Updates = new List<Cell>();
		Area area = new Area();

		const byte Count = 0;
		const byte Position = 1;
		private byte[,] CountOpen = new byte[2, 9];

		public Field()
		{
			for (byte row = 0; row < 9; row++)
			{
				for (byte col = 0; col < 9; col++)
				{
					Play[col + row * 9] = new Cell
					{
						Col = col,
						Row = row,
						Content = ' ',
						Source = Cell.CellSource.Empty,
						Updated = false,
						Open = "123456789"
					};
				}
			}
		}//Field intilizer

		public Cell GetCell(byte col, byte row)
		{
			return Play[col + row * 9];
		}

		public Cell SetCell(byte col, byte row, char content, Cell.CellSource source)
		{
			Play[col + row * 9].Content = content;
			Play[col + row * 9].Source = source;

			History.Add(Play[col + row * 9]);

			UpdateField(col, row);

			return GetCell(col, row);
		}

		public Cell SetCell(Cell cell)
		{
			return SetCell(cell.Col, cell.Row, cell.Content, cell.Source);
		}

		internal void UpdateField(byte col, byte row)
		{
			UpdateColumn(col, row);
			UpdateRow(col, row);
			UpdateArea(col, row);
		}

		internal void UpdateColumn(byte col, byte row)
		{
			Cell cell = GetCell(col, row);
			char UpdateContent = cell.Content;

			for (byte i = 0; i < 9; i++)
			{
				if (i != col)
				{
					cell = GetCell(i, row);
					cell.RemoveOpen(UpdateContent);
					if (cell.Reset())
					{
						Updates.Add(cell);
					}
				}
			}
		}

		internal void UpdateRow(byte col, byte row)
		{
			Cell cell = GetCell(col, row);
			char UpdateContent = cell.Content;

			for (byte i = 0; i < 9; i++)
			{
				if (i != row)
				{
					cell = GetCell(col, i);
					cell.RemoveOpen(UpdateContent);
					if (cell.Reset())
					{
						Updates.Add(cell);
					}
				}
			}
		}

		internal void UpdateArea(byte col, byte row)
		{
			Cell cell = GetCell(col, row);
			char UpdateContent = cell.Content;

			int AreaCol = (col / 3) * 3;
			int AreaRow = (row / 3) * 3;

			for (byte j = 0; j < 3; j++)
			{
				for (byte i = 0; i < 3; i++)
				{
					if (AreaCol + i != col && AreaRow + j != row)
					{
						cell = GetCell((byte)(AreaCol + i), (byte)(AreaRow + j));
						cell.RemoveOpen(UpdateContent);
						if (cell.Reset())
						{
							Updates.Add(cell);
						}
					}
				}
			}
		}

		private void GetRow(byte row)
		{
			for (byte col = 0; col < 9; col++)
			{
				area.Result[col] = Play[col + row * 9];
			}
		}

		private void GetColumn(byte col)
		{
			for (byte row = 0; row < 9; row++)
			{
				area.Result[row] = Play[col + row * 9];
			}
		}

		private void GetArea(byte _area)
		{
			byte left = (byte)(_area % 3 * 3);
			byte top = (byte)(_area / 3 * 3);
			byte count = 0;

			for (byte row = 0; row < 3; row++)
			{
				for (byte col = 0; col < 3; col++)
				{
					area.Result[count] = Play[left + col + (top + row) * 3];
					count++;
				}
			}
		}

		public bool CheckOpenField()
		{
			bool Result = false;

			Result = Result || CheckOpenColumn();
			Result = Result || CheckOpenRow();
			Result = Result || CheckOpenArea();

			return Result;
		}

		internal bool CheckOpenColumn()
		{
			bool Result = false;
			List<Cell> updates;

			for (byte col = 0; col < 9; col++)
			{
				GetColumn(col);
				updates = area.CountOpen();
				Result = Result || updates.Count > 0;
				foreach (var item in updates)
				{
					Updates.Add(item);
				}
			}

			return Result;
		}

		internal bool CheckOpenRow()
		{
			bool Result = false;
			List<Cell> updates;

			for (byte row = 0; row < 9; row++)
			{
				GetRow(row);
				updates = area.CountOpen();
				Result = Result || updates.Count > 0;
				foreach (var item in updates)
				{
					Updates.Add(item);
				}
			}

			return Result;
		}

		internal bool CheckOpenArea()
		{
			bool Result = false;
			List<Cell> updates;

			for (byte i = 0; i < 9; i++)
			{
				GetArea(i);
				updates = area.CountOpen();
				Result = Result || updates.Count > 0;
				foreach (var item in updates)
				{
					Updates.Add(item);
				}
			}

			return Result;
		}

		public void ExportPreFilledCells(string path)
		{
			using (FileStream Json = File.Open(path, FileMode.OpenOrCreate))
			{
				DataContractJsonSerializer Export = new DataContractJsonSerializer(typeof(List<Cell>));
				Export.WriteObject(Json, History);
			}
		}

		public bool ImportPreFilledCells(string path)
		{
			bool Result = false;
			try
			{
				using (FileStream JSon = File.Open(path, FileMode.Open))
				{
					DataContractJsonSerializer Import = new DataContractJsonSerializer(typeof(List<Cell>));
					List<Cell> TempHistory = new List<Cell>();
					TempHistory = (List<Cell>)Import.ReadObject(JSon);

					History.Clear();
					var PreFilled = from x in TempHistory
													where x.Source == Cell.CellSource.Pre
													select x;

					//ClearField();
					foreach (Cell item in PreFilled)
					{
						SetCell(item.Col, item.Row, item.Content, item.Source);
					}

					Import = null;
					TempHistory = null;
					Result = true;
				}
			}
			catch (Exception)
			{
			}
			return Result;
		}
	}
}
