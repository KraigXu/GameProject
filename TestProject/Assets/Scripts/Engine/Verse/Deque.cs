using System;
using UnityEngine;

namespace Verse
{
	
	internal class Deque<T>
	{
		
		// (get) Token: 0x06001EAF RID: 7855 RVA: 0x000BE9BC File Offset: 0x000BCBBC
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		
		public void PushFront(T item)
		{
			this.PushPrep();
			this.first--;
			if (this.first < 0)
			{
				this.first += this.data.Length;
			}
			this.count++;
			this.data[this.first] = item;
		}

		
		public void PushBack(T item)
		{
			this.PushPrep();
			T[] array = this.data;
			int num = this.first;
			int num2 = this.count;
			this.count = num2 + 1;
			array[(num + num2) % this.data.Length] = item;
		}

		
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		
		private void PushPrep()
		{
			if (this.count < this.data.Length)
			{
				return;
			}
			T[] destinationArray = new T[this.data.Length * 2];
			Array.Copy(this.data, this.first, destinationArray, 0, Mathf.Min(this.count, this.data.Length - this.first));
			if (this.first + this.count > this.data.Length)
			{
				Array.Copy(this.data, 0, destinationArray, this.data.Length - this.first, this.count - this.data.Length + this.first);
			}
			this.data = destinationArray;
			this.first = 0;
		}

		
		private T[] data;

		
		private int first;

		
		private int count;
	}
}
