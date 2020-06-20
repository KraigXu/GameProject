using System;

namespace Verse.AI
{
	// Token: 0x020005BD RID: 1469
	public struct CastPositionRequest
	{
		// Token: 0x0400188E RID: 6286
		public Pawn caster;

		// Token: 0x0400188F RID: 6287
		public Thing target;

		// Token: 0x04001890 RID: 6288
		public Verb verb;

		// Token: 0x04001891 RID: 6289
		public float maxRangeFromCaster;

		// Token: 0x04001892 RID: 6290
		public float maxRangeFromTarget;

		// Token: 0x04001893 RID: 6291
		public IntVec3 locus;

		// Token: 0x04001894 RID: 6292
		public float maxRangeFromLocus;

		// Token: 0x04001895 RID: 6293
		public bool wantCoverFromTarget;

		// Token: 0x04001896 RID: 6294
		public int maxRegions;
	}
}
