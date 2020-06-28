using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200001B RID: 27
	public static class GenCollection
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x00008198 File Offset: 0x00006398
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				hashSet.Add(item);
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000081E4 File Offset: 0x000063E4
		public static void SetOrAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = value;
				return;
			}
			dict.Add(key, value);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008200 File Offset: 0x00006400
		public static void Increment<K>(this Dictionary<K, int> dict, K key)
		{
			if (dict.ContainsKey(key))
			{
				int num = dict[key];
				dict[key] = num + 1;
				return;
			}
			dict[key] = 1;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008234 File Offset: 0x00006434
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			return source.Any((T item) => other.Contains(item));
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008260 File Offset: 0x00006460
		public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source, IList<T> workingList = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (workingList == null)
			{
				workingList = source.ToList<T>();
			}
			else
			{
				workingList.Clear();
				foreach (T item in source)
				{
					workingList.Add(item);
				}
			}
			int countUnChosen = workingList.Count;
			int rand = 0;
			while (countUnChosen > 0)
			{
				rand = Rand.Range(0, countUnChosen);
				yield return workingList[rand];
				T value = workingList[rand];
				workingList[rand] = workingList[countUnChosen - 1];
				workingList[countUnChosen - 1] = value;
				int num = countUnChosen;
				countUnChosen = num - 1;
			}
			yield break;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00008278 File Offset: 0x00006478
		public static T RandomElement<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list == null)
			{
				list = source.ToList<T>();
			}
			if (list.Count == 0)
			{
				Log.Warning("Getting random element from empty collection.", false);
				return default(T);
			}
			return list[Rand.Range(0, list.Count)];
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000082D4 File Offset: 0x000064D4
		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000082F0 File Offset: 0x000064F0
		public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				if (list.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				list = source.ToList<T>();
				if (!list.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			result = list.RandomElement<T>();
			return true;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000834C File Offset: 0x0000654C
		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}), false);
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					return list[0];
				}
			}
			else
			{
				int num3 = 0;
				foreach (T t in source)
				{
					num3++;
					float num4 = weightSelector(t);
					if (num4 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num4,
							" from ",
							t
						}), false);
						num4 = 0f;
					}
					num += num4;
				}
				if (num3 == 1 && num > 0f)
				{
					return source.First<T>();
				}
			}
			if (num <= 0f)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num + " - use TryRandomElementByWeight.", false);
				return default(T);
			}
			float num5 = Rand.Value * num;
			float num6 = 0f;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					float num7 = weightSelector(list[j]);
					if (num7 > 0f)
					{
						num6 += num7;
						if (num6 >= num5)
						{
							return list[j];
						}
					}
				}
			}
			else
			{
				foreach (T t2 in source)
				{
					float num8 = weightSelector(t2);
					if (num8 > 0f)
					{
						num6 += num8;
						if (num6 >= num5)
						{
							return t2;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008594 File Offset: 0x00006794
		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElementByWeight(weightSelector, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000085B0 File Offset: 0x000067B0
		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}), false);
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					result = list[0];
					return true;
				}
				if (num == 0f)
				{
					result = default(T);
					return false;
				}
				num *= Rand.Value;
				for (int j = 0; j < list.Count; j++)
				{
					float num3 = weightSelector(list[j]);
					if (num3 > 0f)
					{
						num -= num3;
						if (num <= 0f)
						{
							result = list[j];
							return true;
						}
					}
				}
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			result = default(T);
			float num4 = 0f;
			while (num4 == 0f && enumerator.MoveNext())
			{
				result = enumerator.Current;
				num4 = weightSelector(result);
				if (num4 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num4,
						" from ",
						result
					}), false);
					num4 = 0f;
				}
			}
			if (num4 == 0f)
			{
				result = default(T);
				return false;
			}
			while (enumerator.MoveNext())
			{
				T t = enumerator.Current;
				float num5 = weightSelector(t);
				if (num5 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num5,
						" from ",
						t
					}), false);
					num5 = 0f;
				}
				if (Rand.Range(0f, num4 + num5) >= num4)
				{
					result = t;
				}
				num4 += num5;
			}
			return true;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000087E8 File Offset: 0x000069E8
		public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
		{
			if (defaultValueWeight < 0f)
			{
				Log.Error("Negative default value weight.", false);
				defaultValueWeight = 0f;
			}
			float num = 0f;
			foreach (T t in source)
			{
				float num2 = weightSelector(t);
				if (num2 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num2,
						" from ",
						t
					}), false);
					num2 = 0f;
				}
				num += num2;
			}
			float num3 = defaultValueWeight + num;
			if (num3 <= 0f)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3, false);
				return default(T);
			}
			if (Rand.Value < defaultValueWeight / num3 || num == 0f)
			{
				return default(T);
			}
			return source.RandomElementByWeight(weightSelector);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000088EC File Offset: 0x00006AEC
		public static T FirstOrFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return fallback;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008934 File Offset: 0x00006B34
		public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
		{
			return source.Where(predicate).FirstOrFallback(fallback);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008943 File Offset: 0x00006B43
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008954 File Offset: 0x00006B54
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) > 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008A00 File Offset: 0x00006C00
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008A10 File Offset: 0x00006C10
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = fallback;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					result = tsource;
				}
			}
			return result;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008AB8 File Offset: 0x00006CB8
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008AC8 File Offset: 0x00006CC8
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008B7C File Offset: 0x00006D7C
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008B8C File Offset: 0x00006D8C
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) < 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008C38 File Offset: 0x00006E38
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008C48 File Offset: 0x00006E48
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) < 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008CFC File Offset: 0x00006EFC
		public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				return tsortBy.CompareTo(selector(b));
			});
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008D34 File Offset: 0x00006F34
		public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				return tthenBy.CompareTo(thenBySelector(b));
			});
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008D74 File Offset: 0x00006F74
		public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending tsortByDescending = selector(b);
				return tsortByDescending.CompareTo(selector(a));
			});
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008DAC File Offset: 0x00006FAC
		public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending other = selector(a);
				TSortByDescending other2 = selector(b);
				if (!other.Equals(other2))
				{
					return other2.CompareTo(other);
				}
				TThenByDescending tthenByDescending = thenByDescendingSelector(b);
				return tthenByDescending.CompareTo(thenByDescendingSelector(a));
			});
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008DEC File Offset: 0x00006FEC
		public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
		{
			if (list.Count <= 1)
			{
				return;
			}
			List<Pair<T, int>> list2;
			bool flag;
			if (GenCollection.SortStableTempList<T>.working)
			{
				list2 = new List<Pair<T, int>>();
				flag = false;
			}
			else
			{
				list2 = GenCollection.SortStableTempList<T>.list;
				GenCollection.SortStableTempList<T>.working = true;
				flag = true;
			}
			try
			{
				list2.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(new Pair<T, int>(list[i], i));
				}
				list2.Sort(delegate(Pair<T, int> lhs, Pair<T, int> rhs)
				{
					int num = comparator(lhs.First, rhs.First);
					if (num != 0)
					{
						return num;
					}
					return lhs.Second.CompareTo(rhs.Second);
				});
				list.Clear();
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j].First);
				}
				list2.Clear();
			}
			finally
			{
				if (flag)
				{
					GenCollection.SortStableTempList<T>.working = false;
				}
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008EC0 File Offset: 0x000070C0
		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			int result;
			try
			{
				foreach (KeyValuePair<TKey, TValue> obj in dictionary)
				{
					if (predicate(obj))
					{
						if (list == null)
						{
							list = SimplePool<List<TKey>>.Get();
						}
						list.Add(obj.Key);
					}
				}
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						dictionary.Remove(list[i]);
						i++;
					}
					result = list.Count;
				}
				else
				{
					result = 0;
				}
			}
			finally
			{
				if (list != null)
				{
					list.Clear();
					SimplePool<List<TKey>>.Return(list);
				}
			}
			return result;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00008F7C File Offset: 0x0000717C
		public static void RemoveAll<T>(this List<T> list, Func<T, int, bool> predicate)
		{
			int num = 0;
			int count = list.Count;
			while (num < count && !predicate(list[num], num))
			{
				num++;
			}
			if (num >= count)
			{
				return;
			}
			int i = num + 1;
			while (i < count)
			{
				while (i < count && predicate(list[i], i))
				{
					i++;
				}
				if (i < count)
				{
					list[num++] = list[i++];
				}
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008FED File Offset: 0x000071ED
		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00008FFD File Offset: 0x000071FD
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000901B File Offset: 0x0000721B
		public static bool Any<T>(this List<T> list, Predicate<T> predicate)
		{
			return list.FindIndex(predicate) != -1;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000902A File Offset: 0x0000722A
		public static bool Any<T>(this List<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009035 File Offset: 0x00007235
		public static bool Any<T>(this HashSet<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009040 File Offset: 0x00007240
		public static bool Any<T>(this Stack<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000904C File Offset: 0x0000724C
		public static void AddRange<T>(this HashSet<T> set, List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				set.Add(list[i]);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009078 File Offset: 0x00007278
		public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
		{
			foreach (T item in other)
			{
				set.Add(item);
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000090C8 File Offset: 0x000072C8
		public static int Count_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return 0;
			}
			ICollection collection = e as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in e)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009130 File Offset: 0x00007330
		public static object FirstOrDefault_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return null;
			}
			IList list = e as IList;
			if (list == null)
			{
				using (IEnumerator enumerator = e.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
				return null;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000091A0 File Offset: 0x000073A0
		public static float AverageWeighted<T>(this IEnumerable<T> list, Func<T, float> weight, Func<T, float> value)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (T arg in list)
			{
				float num3 = weight(arg);
				num += num3;
				num2 += value(arg) * num3;
			}
			return num2 / num;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000920C File Offset: 0x0000740C
		public static void ExecuteEnumerable(this IEnumerable enumerable)
		{
			foreach (object obj in enumerable)
			{
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009254 File Offset: 0x00007454
		public static bool EnumerableNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return !enumerable.Any<T>();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009284 File Offset: 0x00007484
		public static int EnumerableCount(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in enumerable)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000092EC File Offset: 0x000074EC
		public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T arg in enumerable)
			{
				if (predicate(arg))
				{
					break;
				}
				num++;
			}
			return num;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009340 File Offset: 0x00007540
		public static V TryGetValue<T, V>(this IDictionary<T, V> dict, T key, V fallback = default(V))
		{
			V result;
			if (!dict.TryGetValue(key, out result))
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000935B File Offset: 0x0000755B
		public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
		{
			T[] lhsv = lhs.ToArray<T>();
			V[] rhsv = rhs.ToArray<V>();
			int num;
			for (int i = 0; i < lhsv.Length; i = num)
			{
				for (int j = 0; j < rhsv.Length; j = num)
				{
					yield return new Pair<T, V>(lhsv[i], rhsv[j]);
					num = j + 1;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009372 File Offset: 0x00007572
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			foreach (T t in lhs)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield return rhs;
			yield break;
			yield break;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000938C File Offset: 0x0000758C
		public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
		{
			if (source == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].IsValid)
				{
					return source[i];
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000093D1 File Offset: 0x000075D1
		public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
		{
			foreach (T t in lhs)
			{
				if (t != rhs)
				{
					yield return t;
				}
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000093E8 File Offset: 0x000075E8
		public static bool ListsEqual<T>(List<T> a, List<T> b) where T : class
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!@default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009460 File Offset: 0x00007660
		public static bool ListsEqualIgnoreOrder<T>(this List<T> a, List<T> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!b.Contains(a[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < b.Count; j++)
			{
				if (!a.Contains(b[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000094DC File Offset: 0x000076DC
		public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
		{
			if (list.NullOrEmpty<T>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < count; i = num)
			{
				yield return list[Rand.Range(0, list.Count)];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000094F4 File Offset: 0x000076F4
		public static void AddDistinct<T>(this List<T> list, T element) where T : class
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == element)
				{
					return;
				}
			}
			list.Add(element);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009530 File Offset: 0x00007730
		public static int Replace<T>(this IList<T> list, T replace, T with) where T : class
		{
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == replace)
				{
					list[i] = with;
					num++;
				}
			}
			return num;
		}

		// Token: 0x020012E4 RID: 4836
		private static class SortStableTempList<T>
		{
			// Token: 0x04004768 RID: 18280
			public static List<Pair<T, int>> list = new List<Pair<T, int>>();

			// Token: 0x04004769 RID: 18281
			public static bool working;
		}
	}
}
