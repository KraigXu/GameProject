using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class SimpleLinearPool<T> where T : new()
	{
		
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			List<T> list = this.items;
			int num = this.readIndex;
			this.readIndex = num + 1;
			return list[num];
		}

		
		public void Clear()
		{
			this.readIndex = 0;
		}

		
		private List<T> items = new List<T>();

		
		private int readIndex;
	}
}
