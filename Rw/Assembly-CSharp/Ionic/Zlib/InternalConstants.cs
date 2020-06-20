using System;

namespace Ionic.Zlib
{
	// Token: 0x020012BC RID: 4796
	internal static class InternalConstants
	{
		// Token: 0x04004638 RID: 17976
		internal static readonly int MAX_BITS = 15;

		// Token: 0x04004639 RID: 17977
		internal static readonly int BL_CODES = 19;

		// Token: 0x0400463A RID: 17978
		internal static readonly int D_CODES = 30;

		// Token: 0x0400463B RID: 17979
		internal static readonly int LITERALS = 256;

		// Token: 0x0400463C RID: 17980
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x0400463D RID: 17981
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x0400463E RID: 17982
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x0400463F RID: 17983
		internal static readonly int REP_3_6 = 16;

		// Token: 0x04004640 RID: 17984
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x04004641 RID: 17985
		internal static readonly int REPZ_11_138 = 18;
	}
}
