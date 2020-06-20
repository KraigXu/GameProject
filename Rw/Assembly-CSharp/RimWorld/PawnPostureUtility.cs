using System;

namespace RimWorld
{
	// Token: 0x02000B2E RID: 2862
	public static class PawnPostureUtility
	{
		// Token: 0x06004353 RID: 17235 RVA: 0x0016AE5C File Offset: 0x0016905C
		public static bool Laying(this PawnPosture posture)
		{
			return posture == PawnPosture.LayingOnGroundFaceUp || posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingInBed;
		}
	}
}
