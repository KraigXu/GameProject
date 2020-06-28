using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000429 RID: 1065
	public static class Dijkstra<T>
	{
		// Token: 0x06001FD4 RID: 8148 RVA: 0x000C2BEF File Offset: 0x000C0DEF
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x000C2C18 File Offset: 0x000C0E18
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			outDistances.Clear();
			Dijkstra<T>.distances.Clear();
			Dijkstra<T>.queue.Clear();
			if (outParents != null)
			{
				outParents.Clear();
			}
			IList<T> list = startingNodes as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					T key = list[i];
					if (!Dijkstra<T>.distances.ContainsKey(key))
					{
						Dijkstra<T>.distances.Add(key, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key, 0f));
					}
				}
				goto IL_183;
			}
			using (IEnumerator<T> enumerator = startingNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T key2 = enumerator.Current;
					if (!Dijkstra<T>.distances.ContainsKey(key2))
					{
						Dijkstra<T>.distances.Add(key2, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key2, 0f));
					}
				}
				goto IL_183;
			}
			IL_DC:
			KeyValuePair<T, float> node = Dijkstra<T>.queue.Pop();
			float num = Dijkstra<T>.distances[node.Key];
			if (node.Value == num)
			{
				IEnumerable<T> enumerable = neighborsGetter(node.Key);
				if (enumerable != null)
				{
					IList<T> list2 = enumerable as IList<T>;
					if (list2 != null)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							Dijkstra<T>.HandleNeighbor(list2[j], num, node, distanceGetter, outParents);
						}
					}
					else
					{
						foreach (T n in enumerable)
						{
							Dijkstra<T>.HandleNeighbor(n, num, node, distanceGetter, outParents);
						}
					}
				}
			}
			IL_183:
			if (Dijkstra<T>.queue.Count == 0)
			{
				foreach (KeyValuePair<T, float> item in Dijkstra<T>.distances)
				{
					outDistances.Add(item);
				}
				Dijkstra<T>.distances.Clear();
				return;
			}
			goto IL_DC;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000C2E24 File Offset: 0x000C1024
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000C2E4C File Offset: 0x000C104C
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.Run(startingNodes, neighborsGetter, distanceGetter, Dijkstra<T>.tmpResult, outParents);
			outDistances.Clear();
			for (int i = 0; i < Dijkstra<T>.tmpResult.Count; i++)
			{
				outDistances.Add(Dijkstra<T>.tmpResult[i].Key, Dijkstra<T>.tmpResult[i].Value);
			}
			Dijkstra<T>.tmpResult.Clear();
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x000C2EBC File Offset: 0x000C10BC
		private static void HandleNeighbor(T n, float nodeDist, KeyValuePair<T, float> node, Func<T, T, float> distanceGetter, Dictionary<T, T> outParents)
		{
			float num = nodeDist + Mathf.Max(distanceGetter(node.Key, n), 0f);
			bool flag = false;
			float num2;
			if (Dijkstra<T>.distances.TryGetValue(n, out num2))
			{
				if (num < num2)
				{
					Dijkstra<T>.distances[n] = num;
					flag = true;
				}
			}
			else
			{
				Dijkstra<T>.distances.Add(n, num);
				flag = true;
			}
			if (flag)
			{
				Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(n, num));
				if (outParents != null)
				{
					if (outParents.ContainsKey(n))
					{
						outParents[n] = node.Key;
						return;
					}
					outParents.Add(n, node.Key);
				}
			}
		}

		// Token: 0x040013A4 RID: 5028
		private static Dictionary<T, float> distances = new Dictionary<T, float>();

		// Token: 0x040013A5 RID: 5029
		private static FastPriorityQueue<KeyValuePair<T, float>> queue = new FastPriorityQueue<KeyValuePair<T, float>>(new Dijkstra<T>.DistanceComparer());

		// Token: 0x040013A6 RID: 5030
		private static List<T> singleNodeList = new List<T>();

		// Token: 0x040013A7 RID: 5031
		private static List<KeyValuePair<T, float>> tmpResult = new List<KeyValuePair<T, float>>();

		// Token: 0x02001676 RID: 5750
		private class DistanceComparer : IComparer<KeyValuePair<T, float>>
		{
			// Token: 0x060084C6 RID: 33990 RVA: 0x002B0AC4 File Offset: 0x002AECC4
			public int Compare(KeyValuePair<T, float> a, KeyValuePair<T, float> b)
			{
				return a.Value.CompareTo(b.Value);
			}
		}
	}
}
