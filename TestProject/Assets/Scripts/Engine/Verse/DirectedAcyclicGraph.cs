using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class DirectedAcyclicGraph
	{
		
		public DirectedAcyclicGraph(int numVertices)
		{
			this.numVertices = numVertices;
			for (int i = 0; i < numVertices; i++)
			{
				this.adjacencyList.Add(new List<int>());
			}
		}

		
		public void AddEdge(int from, int to)
		{
			this.adjacencyList[from].Add(to);
		}

		
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

		
		public bool IsCyclic()
		{
			return this.FindCycle() != -1;
		}

		
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

		
		private int numVertices;

		
		private List<List<int>> adjacencyList = new List<List<int>>();
	}
}
