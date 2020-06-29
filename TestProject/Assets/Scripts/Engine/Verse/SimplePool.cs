using System;
using System.Collections.Generic;

namespace Verse
{
	
	public static class SimplePool<T> where T : new()
	{
		
		
		public static int FreeItemsCount
		{
			get
			{
				return SimplePool<T>.freeItems.Count;
			}
		}

		
		public static T Get()
		{
			if (SimplePool<T>.freeItems.Count == 0)
			{
				return Activator.CreateInstance<T>();
			}
			int index = SimplePool<T>.freeItems.Count - 1;
			T result = SimplePool<T>.freeItems[index];
			SimplePool<T>.freeItems.RemoveAt(index);
			return result;
		}

		
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		
		private static List<T> freeItems = new List<T>();
	}
}
