using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200042B RID: 1067
	public class DirectedAcyclicGraph
	{
		// Token: 0x06001FDC RID: 8156 RVA: 0x000C2FE8 File Offset: 0x000C11E8
		public DirectedAcyclicGraph(int numVertices)
		{
			this.numVertices = numVertices;
			for (int i = 0; i < numVertices; i++)
			{
				this.adjacencyList.Add(new List<int>());
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000C3029 File Offset: 0x000C1229
		public void AddEdge(int from, int to)
		{
			this.adjacencyList[from].Add(to);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000C3040 File Offset: 0x000C1240
		public List<int> TopologicalSort()
		{
			bool[] array = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
			}
			List<int> result = new List<int>();
			for (int j = 0; j < this.numVertices; j++)
			{
				if (!array[j])
				{
					this.TopologicalSortInner(j, array, result);
				}
			}
			return result;
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000C3094 File Offset: 0x000C1294
		private void TopologicalSortInner(int v, bool[] visited, List<int> result)
		{
			visited[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num])
				{
					this.TopologicalSortInner(num, visited, result);
				}
			}
			result.Add(v);
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000C3100 File Offset: 0x000C1300
		public bool IsCyclic()
		{
			return this.FindCycle() != -1;
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000C3110 File Offset: 0x000C1310
		public int FindCycle()
		{
			bool[] array = new bool[this.numVertices];
			bool[] array2 = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
				array2[i] = false;
			}
			for (int j = 0; j < this.numVertices; j++)
			{
				if (this.IsCyclicInner(j, array, array2))
				{
					return j;
				}
			}
			return -1;
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000C3170 File Offset: 0x000C1370
		private bool IsCyclicInner(int v, bool[] visited, bool[] history)
		{
			visited[v] = true;
			history[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num] && this.IsCyclicInner(num, visited, history))
				{
					return true;
				}
				if (history[num])
				{
					return true;
				}
			}
			history[v] = false;
			return false;
		}

		// Token: 0x040013A9 RID: 5033
		private int numVertices;

		// Token: 0x040013AA RID: 5034
		private List<List<int>> adjacencyList = new List<List<int>>();
	}
}
