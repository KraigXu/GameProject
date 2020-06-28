using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000040 RID: 64
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000376 RID: 886 RVA: 0x000126A6 File Offset: 0x000108A6
		public static int FreeItemsCount
		{
			get
			{
				return SimplePool<T>.freeItems.Count;
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000126B4 File Offset: 0x000108B4
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

		// Token: 0x06000378 RID: 888 RVA: 0x000126F6 File Offset: 0x000108F6
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		// Token: 0x040000F4 RID: 244
		private static List<T> freeItems = new List<T>();
	}
}
