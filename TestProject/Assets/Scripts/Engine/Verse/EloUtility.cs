using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000436 RID: 1078
	public static class EloUtility
	{
		// Token: 0x06001FFC RID: 8188 RVA: 0x000C3875 File Offset: 0x000C1A75
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000C3890 File Offset: 0x000C1A90
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000C38D4 File Offset: 0x000C1AD4
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000C38EB File Offset: 0x000C1AEB
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x040013B4 RID: 5044
		private const float TenFactorRating = 400f;
	}
}
