using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000350 RID: 848
	public class DebugHistogram
	{
		// Token: 0x060019F4 RID: 6644 RVA: 0x0009F402 File Offset: 0x0009D602
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0009F434 File Offset: 0x0009D634
		public void Add(float val)
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				if (this.buckets[i] > val)
				{
					this.counts[i]++;
					return;
				}
			}
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0009F474 File Offset: 0x0009D674
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0009F49C File Offset: 0x0009D69C
		public void Display(StringBuilder sb)
		{
			int num = Mathf.Max(this.counts.Max(), 1);
			int num2 = this.counts.Aggregate((int a, int b) => a + b);
			for (int i = 0; i < this.buckets.Length; i++)
			{
				sb.AppendLine(string.Format("{0}    {1}: {2} ({3:F2}%)", new object[]
				{
					new string('#', this.counts[i] * 40 / num),
					this.buckets[i],
					this.counts[i],
					(double)this.counts[i] * 100.0 / (double)num2
				}));
			}
		}

		// Token: 0x04000F14 RID: 3860
		private float[] buckets;

		// Token: 0x04000F15 RID: 3861
		private int[] counts;
	}
}
