using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D3A RID: 3386
	public class CompProperties_ProjectileInterceptor : CompProperties
	{
		// Token: 0x06005237 RID: 21047 RVA: 0x001B7748 File Offset: 0x001B5948
		public CompProperties_ProjectileInterceptor()
		{
			this.compClass = typeof(CompProjectileInterceptor);
		}

		// Token: 0x04002D5A RID: 11610
		public float radius;

		// Token: 0x04002D5B RID: 11611
		public int cooldownTicks;

		// Token: 0x04002D5C RID: 11612
		public int disarmedByEmpForTicks;

		// Token: 0x04002D5D RID: 11613
		public bool interceptGroundProjectiles;

		// Token: 0x04002D5E RID: 11614
		public bool interceptAirProjectiles;

		// Token: 0x04002D5F RID: 11615
		public bool interceptNonHostileProjectiles;

		// Token: 0x04002D60 RID: 11616
		public bool interceptOutgoingProjectiles;

		// Token: 0x04002D61 RID: 11617
		public int chargeIntervalTicks;

		// Token: 0x04002D62 RID: 11618
		public int chargeDurationTicks;

		// Token: 0x04002D63 RID: 11619
		public float minAlpha;

		// Token: 0x04002D64 RID: 11620
		public Color color = Color.white;

		// Token: 0x04002D65 RID: 11621
		public EffecterDef reactivateEffect;

		// Token: 0x04002D66 RID: 11622
		public EffecterDef interceptEffect;
	}
}
