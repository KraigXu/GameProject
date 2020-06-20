using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040C RID: 1036
	internal class Deque<T>
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001EAF RID: 7855 RVA: 0x000BE9BC File Offset: 0x000BCBBC
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000BE9C7 File Offset: 0x000BCBC7
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000BE9EC File Offset: 0x000BCBEC
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

		// Token: 0x06001EB2 RID: 7858 RVA: 0x000BEA4C File Offset: 0x000BCC4C
		public void PushBack(T item)
		{
			this.PushPrep();
			T[] array = this.data;
			int num = this.first;
			int num2 = this.count;
			this.count = num2 + 1;
			array[(num + num2) % this.data.Length] = item;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x000BEA8C File Offset: 0x000BCC8C
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000BEAE9 File Offset: 0x000BCCE9
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000BEAFC File Offset: 0x000BCCFC
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

		// Token: 0x040012E7 RID: 4839
		private T[] data;

		// Token: 0x040012E8 RID: 4840
		private int first;

		// Token: 0x040012E9 RID: 4841
		private int count;
	}
}
