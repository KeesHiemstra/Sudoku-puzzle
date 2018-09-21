using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	[DataContract]
	class Cell
	{
		public enum CellSource { Empty, Pre, Rule }

		[DataMember]
		public char Content { get; set; }

		[DataMember]
		public CellSource Source { get; set; }

		public string Open { get; set; }

		public bool Updated { get; private set; }

		public Cell()
		{
			ClearCell();
		}

		public void ClearCell()
		{
			Content = ' ';
			Source = CellSource.Empty;
			Open = "123456789";
			Updated = false;
		}

		public bool Reset()
		{
			bool Result = Updated;
			if (Updated)
			{
				Updated = false;
			}
			return Result;
		}

		public void RemoveFromOpen(char update)
		{
			if (Open.Length == 0) { return; }

			int Index = Open.IndexOf(update);
			if (Index >= 0)
			{
				Open.Remove(Index);
			}
			//if (Open.Length == 1)
			//{
			//	Content = Open[0];
			//}
		}
	}

	[DataContract]
	class HCell : Cell
	{
		[DataMember]
		public int Col { get; set; }
		[DataMember]
		public int Row { get; set; }
	}
}
