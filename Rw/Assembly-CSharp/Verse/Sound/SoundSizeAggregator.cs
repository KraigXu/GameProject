using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000501 RID: 1281
	public class SoundSizeAggregator
	{
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x000DAA24 File Offset: 0x000D8C24
		public float AggregateSize
		{
			get
			{
				if (this.reporters.Count == 0)
				{
					return this.testSize;
				}
				float num = 0f;
				foreach (ISizeReporter sizeReporter in this.reporters)
				{
					num += sizeReporter.CurrentSize();
				}
				return num;
			}
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000DAA94 File Offset: 0x000D8C94
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000DAACB File Offset: 0x000D8CCB
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000DAAD9 File Offset: 0x000D8CD9
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x0400165C RID: 5724
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x0400165D RID: 5725
		private float testSize;
	}
}
