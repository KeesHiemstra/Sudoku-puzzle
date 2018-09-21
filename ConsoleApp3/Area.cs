using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
	class AreaCollection<T>
	{
		static private T[] arr = new T[10];

		public T this[int i]
		{
			get { return arr[i]; }
			set { arr[i] = value; }
		}

	}
}
