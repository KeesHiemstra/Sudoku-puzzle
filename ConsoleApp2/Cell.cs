using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SudokuSolver
{
	[DataContract]
	class Cell
	{
		public enum CellSource { Empty, Pre, Rule }

		private char _Content;
		[DataMember]
		public char Content
		{
			get => _Content;
			set
			{
				_Content = value;
				//Updated = true;
				if (_Content != ' ')
				{
					Open = "";
				}
			}
		}

		[DataMember]
		public CellSource Source { get; set; }

		private string _Open;
		public string Open
		{
			get => _Open;
			set
			{
				_Open = value;
				if (_Open.Length == 1)
				{
					Content = _Open[0];
					Updated = true;
					Source = CellSource.Rule;
				}
			}
		}

		private bool _Updated;
		public bool Updated
		{
			get => _Updated;
			set => _Updated = value;
		}

		[DataMember]
		public byte Col { get; set; }

		[DataMember]
		public byte Row { get; set; }

		public void RemoveOpen(char remove)
		{
			if (Open.Length == 0) { return; }

			int Index = Open.IndexOf(remove);
			if (Index >= 0) { Open = Open.Remove(Index, 1); }
		}

		public bool Reset()
		{
			bool Result = Updated;
			Updated = false;
			return Result;
		}
	}
}
