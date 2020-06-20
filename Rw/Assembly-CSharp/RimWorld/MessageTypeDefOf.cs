using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9B RID: 3995
	[DefOf]
	public static class MessageTypeDefOf
	{
		// Token: 0x060060A2 RID: 24738 RVA: 0x00217222 File Offset: 0x00215422
		static MessageTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MessageTypeDefOf));
		}

		// Token: 0x04003A64 RID: 14948
		public static MessageTypeDef ThreatBig;

		// Token: 0x04003A65 RID: 14949
		public static MessageTypeDef ThreatSmall;

		// Token: 0x04003A66 RID: 14950
		public static MessageTypeDef PawnDeath;

		// Token: 0x04003A67 RID: 14951
		public static MessageTypeDef NegativeHealthEvent;

		// Token: 0x04003A68 RID: 14952
		public static MessageTypeDef NegativeEvent;

		// Token: 0x04003A69 RID: 14953
		public static MessageTypeDef NeutralEvent;

		// Token: 0x04003A6A RID: 14954
		public static MessageTypeDef TaskCompletion;

		// Token: 0x04003A6B RID: 14955
		public static MessageTypeDef PositiveEvent;

		// Token: 0x04003A6C RID: 14956
		public static MessageTypeDef SituationResolved;

		// Token: 0x04003A6D RID: 14957
		public static MessageTypeDef RejectInput;

		// Token: 0x04003A6E RID: 14958
		public static MessageTypeDef CautionInput;

		// Token: 0x04003A6F RID: 14959
		public static MessageTypeDef SilentInput;
	}
}
