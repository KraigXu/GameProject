using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC0 RID: 3264
	public static class SkyfallerDrawPosUtility
	{
		// Token: 0x06004F32 RID: 20274 RVA: 0x001AAED4 File Offset: 0x001A90D4
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x001AAF08 File Offset: 0x001A9108
		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001AAF2C File Offset: 0x001A912C
		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(ticksToImpact * ticksToImpact) * 0.00721f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001AAF57 File Offset: 0x001A9157
		private static Vector3 PosAtDist(Vector3 center, float dist, float angle)
		{
			return center + Vector3Utility.FromAngleFlat(angle - 90f) * dist;
		}
	}
}
