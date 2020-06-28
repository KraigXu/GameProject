using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BA RID: 2234
	public sealed class DifficultyDef : Def
	{
		// Token: 0x04001DA7 RID: 7591
		public Color drawColor = Color.white;

		// Token: 0x04001DA8 RID: 7592
		public bool isExtreme;

		// Token: 0x04001DA9 RID: 7593
		public int difficulty = -1;

		// Token: 0x04001DAA RID: 7594
		public float threatScale = 1f;

		// Token: 0x04001DAB RID: 7595
		public bool allowBigThreats = true;

		// Token: 0x04001DAC RID: 7596
		public bool allowIntroThreats = true;

		// Token: 0x04001DAD RID: 7597
		public bool allowCaveHives = true;

		// Token: 0x04001DAE RID: 7598
		public bool peacefulTemples;

		// Token: 0x04001DAF RID: 7599
		public bool allowViolentQuests = true;

		// Token: 0x04001DB0 RID: 7600
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x04001DB1 RID: 7601
		public float scariaRotChance;

		// Token: 0x04001DB2 RID: 7602
		public float colonistMoodOffset;

		// Token: 0x04001DB3 RID: 7603
		public float tradePriceFactorLoss;

		// Token: 0x04001DB4 RID: 7604
		public float cropYieldFactor = 1f;

		// Token: 0x04001DB5 RID: 7605
		public float mineYieldFactor = 1f;

		// Token: 0x04001DB6 RID: 7606
		public float butcherYieldFactor = 1f;

		// Token: 0x04001DB7 RID: 7607
		public float researchSpeedFactor = 1f;

		// Token: 0x04001DB8 RID: 7608
		public float diseaseIntervalFactor = 1f;

		// Token: 0x04001DB9 RID: 7609
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x04001DBA RID: 7610
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x04001DBB RID: 7611
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x04001DBC RID: 7612
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x04001DBD RID: 7613
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x04001DBE RID: 7614
		public float threatsGeneratorThreatCountFactor = 1f;

		// Token: 0x04001DBF RID: 7615
		public float maintenanceCostFactor = 1f;

		// Token: 0x04001DC0 RID: 7616
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x04001DC1 RID: 7617
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x04001DC2 RID: 7618
		public float adaptationEffectFactor = 1f;

		// Token: 0x04001DC3 RID: 7619
		public float questRewardValueFactor = 1f;
	}
}
