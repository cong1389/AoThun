using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Aplication
{
	public class ModelHelper<T>
	{
		public IEnumerable<T> Result
		{
			get;
			set;
		}

		public int TotalRecords
		{
			get;
			set;
		}

		public ModelHelper()
		{
		}
	}
}