using System;

namespace Verse
{
	
	public class FloodFillRangeQueue
	{
		
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00044267 File Offset: 0x00042467
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0004426F File Offset: 0x0004246F
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		
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

		
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		
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

		
		private FloodFillRange[] array;

		
		private int count;

		
		private int head;

		
		private int debugNumTimesExpanded;

		
		private int debugMaxUsedSpace;
	}
}
