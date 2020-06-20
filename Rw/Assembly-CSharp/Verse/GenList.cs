using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200001F RID: 31
	public static class GenList
	{
		// Token: 0x06000205 RID: 517 RVA: 0x00009B77 File Offset: 0x00007D77
		public static int CountAllowNull<T>(this IList<T> list)
		{
			if (list == null)
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00009B84 File Offset: 0x00007D84
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009B94 File Offset: 0x00007D94
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009BCC File Offset: 0x00007DCC
		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ListFullCopy<T>();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009BDC File Offset: 0x00007DDC
		public static void RemoveDuplicates<T>(this List<T> list) where T : class
		{
			if (list.Count <= 1)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if (list[i] == list[j])
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009C38 File Offset: 0x00007E38
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = Rand.RangeInclusive(0, i);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009C7C File Offset: 0x00007E7C
		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				T t = list[i];
				int num = i - 1;
				while (num >= 0 && comparison(list[num], t) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = t;
			}
		}
	}
}
