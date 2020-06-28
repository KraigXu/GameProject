using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000437 RID: 1079
	public class FastPriorityQueue<T>
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x000C3902 File Offset: 0x000C1B02
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000C390F File Offset: 0x000C1B0F
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x000C392D File Offset: 0x000C1B2D
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x000C3948 File Offset: 0x000C1B48
		public void Push(T item)
		{
			int num = this.innerList.Count;
			this.innerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.CompareElements(num, num2) >= 0)
				{
					break;
				}
				this.SwapElements(num, num2);
				num = num2;
			}
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x000C3990 File Offset: 0x000C1B90
		public T Pop()
		{
			T result = this.innerList[0];
			int num = 0;
			int count = this.innerList.Count;
			this.innerList[0] = this.innerList[count - 1];
			this.innerList.RemoveAt(count - 1);
			count = this.innerList.Count;
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = num3 + 1;
				if (num3 < count && this.CompareElements(num, num3) > 0)
				{
					num = num3;
				}
				if (num4 < count && this.CompareElements(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwapElements(num, num2);
			}
			return result;
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x000C3A32 File Offset: 0x000C1C32
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000C3A40 File Offset: 0x000C1C40
		protected void SwapElements(int i, int j)
		{
			T value = this.innerList[i];
			this.innerList[i] = this.innerList[j];
			this.innerList[j] = value;
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000C3A7F File Offset: 0x000C1C7F
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}

		// Token: 0x040013B5 RID: 5045
		protected List<T> innerList = new List<T>();

		// Token: 0x040013B6 RID: 5046
		protected IComparer<T> comparer;
	}
}
