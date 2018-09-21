using ConsoleApp3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
	class Field
	{
		public Cell[] Play = new Cell[81];
		public List<Cell> History = new List<Cell>();
		public List<Cell> Updates = new List<Cell>();
		public AreaCollection<Cell> Area = new AreaCollection<Cell>();

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

		public AreaCollection<Cell> GetRow(byte position)
		{
			for (int i = 0; i < 9; i++)
			{
				Area[i] = Play[i + position * 9];
			}
			return Area;
		}

		public bool CheckOpenField()
		{
			bool Result = false;

			Result = Result || CheckOpenColumn();
			Result = Result || CheckOpenRow();
			//			Result = Result || CheckOpenArea();

			return Result;
		}

		internal void ResetCountOpen()
		{
			for (byte col = 0; col < 2; col++)
			{
				for (byte reset = 0; reset < 9; reset++)
				{
					CountOpen[col, reset] = 0;
				}
			}
		}

		internal bool CheckOpenColumn()
		{
			bool Result = false;
			byte Index = 0;

			for (byte row = 0; row < 9; row++)
			{
				ResetCountOpen();

				//Open strings on this row
				for (byte col = 0; col < 9; col++)
				{
					if (Play[col + row * 9].Open.Length > 0)
					{
						for (byte check = 0; check < Play[col + row * 9].Open.Length; check++)
						{
							CountOpen[Count, Play[col + row * 9].Open[check] - '1']++;
							CountOpen[Position, Play[col + row * 9].Open[check] - '1'] = col;
						}
					}
				}

				for (byte check = 0; check < 9; check++)
				{
					if (CountOpen[Count, check] == 1)
					{
						Index = (byte)(CountOpen[Position, check] + row * 9);
						Play[Index].Content = (char)(check + '1');
						Play[Index].Source = Cell.CellSource.Rule;
						Play[Index].Open = string.Empty;
						Updates.Add(Play[Index]);
						Result = true;
						UpdateField(CountOpen[Position, check], row);
					}
				}
			}
			return Result;
		}

		internal bool CheckOpenRow()
		{
			bool Result = false;
			byte Index = 0;

			for (byte col = 0; col < 9; col++)
			{
				ResetCountOpen();

				//Open stings on this column
				for (byte row = 0; row < 9; row++)
				{
					if (Play[col + row * 9].Open.Length > 0)
					{
						for (byte check = 0; check < Play[col + row * 9].Open.Length; check++)
						{
							CountOpen[Count, Play[col + row * 9].Open[check] - '1']++;
							CountOpen[Position, Play[col + row * 9].Open[check] - '1'] = col;
						}
					}
				}

				for (byte check = 0; check < 9; check++)
				{
					if (CountOpen[Count, check] == 1)
					{
						Index = (byte)(col + CountOpen[Position, check] * 9);
						Play[Index].Content = (char)(check + '1');
						Play[Index].Source = Cell.CellSource.Rule;
						Play[Index].Open = string.Empty;
						Updates.Add(Play[Index]);
						Result = true;
						UpdateField(col, CountOpen[Position, check]);
					}
				}
			}
			return Result;
		}

		/*
				internal bool CheckOpenArea()
				{
					bool Result = false;

					byte Index;
					for (byte area = 0; area < 9; area++)
					{
						byte AreaCol = (byte)((area % 3) * 3);
						byte AreaRow = (byte)((area / 3) * 3);

						for (byte reset = 0; reset < 9; reset++) { CountOpen[reset] = 0; }

						for (byte row = 0; row < 3; row++)
						{
							for (byte col = 0; col < 3; col++)
							{
								Index = (byte)(AreaCol + col + AreaRow + row * 9);
								if (Play[Index].Open.Length > 0)
								{
									for (byte check = 0; check < Play[Index].Open.Length; check++)
									{
										CountOpen[Play[Index].Open[check] - '1']++;
									}
									for (byte check = 0; check < 9; check++)
									{
										if (CountOpen[check] == 1)
										{
											Play[Index].Content = (char)(check + '1');
											Updates.Add(Play[Index]);
											Result = true;
											UpdateField((byte)(AreaCol + col), (byte)(AreaRow + row));
										}
									}
								}
							}
						}
					}
					return Result;
				}
		*/

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
													//where x.Source == Cell.CellSource.Pre
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
