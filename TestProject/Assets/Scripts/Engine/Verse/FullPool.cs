using System;
using System.Collections.Generic;

namespace Verse
{
	
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		
		public static T Get()
		{
			if (FullPool<T>.freeItems.Count == 0)
			{
				return Activator.CreateInstance<T>();
			}
			T result = FullPool<T>.freeItems[FullPool<T>.freeItems.Count - 1];
			FullPool<T>.freeItems.RemoveAt(FullPool<T>.freeItems.Count - 1);
			return result;
		}

		
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		
		private static List<T> freeItems = new List<T>();
	}
}
