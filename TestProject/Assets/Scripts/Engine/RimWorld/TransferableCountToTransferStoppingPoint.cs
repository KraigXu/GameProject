using System;

namespace RimWorld
{
	
	public struct TransferableCountToTransferStoppingPoint
	{
		
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		
		public int threshold;

		
		public string leftLabel;

		
		public string rightLabel;
	}
}
