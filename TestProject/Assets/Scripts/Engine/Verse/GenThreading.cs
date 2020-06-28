using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200044B RID: 1099
	public static class GenThreading
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000C99CC File Offset: 0x000C7BCC
		public static int ProcessorCount
		{
			get
			{
				return Environment.ProcessorCount;
			}
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000C99D3 File Offset: 0x000C7BD3
		private static void GetMaxDegreeOfParallelism(ref int maxDegreeOfParallelism)
		{
			if (maxDegreeOfParallelism == -1)
			{
				maxDegreeOfParallelism = GenThreading.ProcessorCount;
			}
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000C99E4 File Offset: 0x000C7BE4
		public static List<GenThreading.Slice> SliceWork(int fromInclusive, int toExclusive, int maxBatches)
		{
			List<GenThreading.Slice> list = new List<GenThreading.Slice>(maxBatches);
			int num = toExclusive - fromInclusive;
			if (num <= 0)
			{
				return list;
			}
			int num2 = num % maxBatches;
			int num3 = Mathf.FloorToInt((float)num / (float)maxBatches);
			if (num3 > 0)
			{
				int num4 = 0;
				for (int i = 0; i < maxBatches; i++)
				{
					int num5 = num3;
					if (num2 > 0)
					{
						num5++;
						num2--;
					}
					list.Add(new GenThreading.Slice(num4, num4 + num5));
					num4 += num5;
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					list.Add(new GenThreading.Slice(j, j + 1));
				}
			}
			return list;
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000C9A78 File Offset: 0x000C7C78
		public static List<List<T>> SliceWork<T>(List<T> list, int maxBatches)
		{
			List<List<T>> list2 = new List<List<T>>(maxBatches);
			foreach (GenThreading.Slice slice in GenThreading.SliceWork(0, list.Count, maxBatches))
			{
				List<T> list3 = new List<T>(slice.toExclusive - slice.fromInclusive);
				for (int i = slice.fromInclusive; i < slice.toExclusive; i++)
				{
					list3.Add(list[i]);
				}
				list2.Add(list3);
			}
			return list2;
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000C9B18 File Offset: 0x000C7D18
		public static void ParallelForEach<T>(List<T> list, Action<T> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int count = list.Count;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<List<T>>.Enumerator enumerator = GenThreading.SliceWork<T>(list, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<T> localBatch2 = enumerator.Current;
					List<T> localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						foreach (T obj in localBatch)
						{
							try
							{
								callback(obj);
							}
							catch (Exception exception)
							{
								Debug.LogException(exception);
							}
						}
						Interlocked.Add(ref tasksDone, (long)localBatch.Count);
						taskDoneEvent.Set();
					});
				}
				goto IL_8F;
			}
			IL_83:
			taskDoneEvent.WaitOne();
			IL_8F:
			if (Interlocked.Read(ref tasksDone) >= (long)count)
			{
				return;
			}
			goto IL_83;
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000C9BD4 File Offset: 0x000C7DD4
		public static void ParallelFor(int fromInclusive, int toExclusive, Action<int> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int num = toExclusive - fromInclusive;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<GenThreading.Slice>.Enumerator enumerator = GenThreading.SliceWork(fromInclusive, toExclusive, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenThreading.Slice localBatch2 = enumerator.Current;
					GenThreading.Slice localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						for (int i = localBatch.fromInclusive; i < localBatch.toExclusive; i++)
						{
							try
							{
								callback(i);
							}
							catch (Exception exception)
							{
								Debug.LogException(exception);
							}
						}
						Interlocked.Add(ref tasksDone, (long)(localBatch.toExclusive - localBatch.fromInclusive));
						taskDoneEvent.Set();
					});
				}
				goto IL_8D;
			}
			IL_81:
			taskDoneEvent.WaitOne();
			IL_8D:
			if (Interlocked.Read(ref tasksDone) >= (long)num)
			{
				return;
			}
			goto IL_81;
		}

		// Token: 0x0200169C RID: 5788
		public struct Slice
		{
			// Token: 0x06008538 RID: 34104 RVA: 0x002B20A3 File Offset: 0x002B02A3
			public Slice(int fromInclusive, int toExclusive)
			{
				this.fromInclusive = fromInclusive;
				this.toExclusive = toExclusive;
			}

			// Token: 0x040056C8 RID: 22216
			public readonly int fromInclusive;

			// Token: 0x040056C9 RID: 22217
			public readonly int toExclusive;
		}
	}
}
