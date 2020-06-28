using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000042 RID: 66
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0001270F File Offset: 0x0001090F
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

		// Token: 0x0600037C RID: 892 RVA: 0x0001274F File Offset: 0x0001094F
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		// Token: 0x040000F5 RID: 245
		private static List<T> freeItems = new List<T>();
	}
}
