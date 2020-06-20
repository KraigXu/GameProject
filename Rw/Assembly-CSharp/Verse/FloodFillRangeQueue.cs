using System;

namespace Verse
{
	// Token: 0x020001B1 RID: 433
	public class FloodFillRangeQueue
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00044267 File Offset: 0x00042467
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0004426F File Offset: 0x0004246F
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00044284 File Offset: 0x00042484
		public string PerfDebugString
		{
			get
			{
				return string.Concat(new object[]
				{
					"NumTimesExpanded: ",
					this.debugNumTimesExpanded,
					", MaxUsedSize= ",
					this.debugMaxUsedSpace,
					", ClaimedSize=",
					this.array.Length,
					", UnusedSpace=",
					this.array.Length - this.debugMaxUsedSpace
				});
			}
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x000442FF File Offset: 0x000424FF
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00044324 File Offset: 0x00042524
		public void Enqueue(FloodFillRange r)
		{
			if (this.count + this.head == this.array.Length)
			{
				FloodFillRange[] destinationArray = new FloodFillRange[2 * this.array.Length];
				Array.Copy(this.array, this.head, destinationArray, 0, this.count);
				this.array = destinationArray;
				this.head = 0;
				this.debugNumTimesExpanded++;
			}
			FloodFillRange[] array = this.array;
			int num = this.head;
			int num2 = this.count;
			this.count = num2 + 1;
			array[num + num2] = r;
			this.debugMaxUsedSpace = this.count + this.head;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000443C4 File Offset: 0x000425C4
		public FloodFillRange Dequeue()
		{
			FloodFillRange result = default(FloodFillRange);
			if (this.count > 0)
			{
				result = this.array[this.head];
				this.array[this.head] = default(FloodFillRange);
				this.head++;
				this.count--;
			}
			return result;
		}

		// Token: 0x04000995 RID: 2453
		private FloodFillRange[] array;

		// Token: 0x04000996 RID: 2454
		private int count;

		// Token: 0x04000997 RID: 2455
		private int head;

		// Token: 0x04000998 RID: 2456
		private int debugNumTimesExpanded;

		// Token: 0x04000999 RID: 2457
		private int debugMaxUsedSpace;
	}
}
