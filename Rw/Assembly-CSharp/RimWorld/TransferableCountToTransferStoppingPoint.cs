using System;

namespace RimWorld
{
	// Token: 0x02000E7E RID: 3710
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x06005A43 RID: 23107 RVA: 0x001E8375 File Offset: 0x001E6575
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		// Token: 0x04003112 RID: 12562
		public int threshold;

		// Token: 0x04003113 RID: 12563
		public string leftLabel;

		// Token: 0x04003114 RID: 12564
		public string rightLabel;
	}
}
