using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B87 RID: 2951
	public class Need_Beauty : Need_Seeker
	{
		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x0600453C RID: 17724 RVA: 0x001764EC File Offset: 0x001746EC
		public override float CurInstantLevel
		{
			get
			{
				if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
				{
					return 0.5f;
				}
				if (!this.pawn.Spawned)
				{
					return 0.5f;
				}
				return this.LevelFromBeauty(this.CurrentInstantBeauty());
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x0600453D RID: 17725 RVA: 0x0017653C File Offset: 0x0017473C
		public BeautyCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.99f)
				{
					return BeautyCategory.Beautiful;
				}
				if (this.CurLevel > 0.85f)
				{
					return BeautyCategory.VeryPretty;
				}
				if (this.CurLevel > 0.65f)
				{
					return BeautyCategory.Pretty;
				}
				if (this.CurLevel > 0.35f)
				{
					return BeautyCategory.Neutral;
				}
				if (this.CurLevel > 0.15f)
				{
					return BeautyCategory.Ugly;
				}
				if (this.CurLevel > 0.01f)
				{
					return BeautyCategory.VeryUgly;
				}
				return BeautyCategory.Hideous;
			}
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x001765A4 File Offset: 0x001747A4
		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.35f);
			this.threshPercents.Add(0.65f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x00176603 File Offset: 0x00174803
		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01(this.def.baseLevel + beauty * 0.1f);
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x0017661D File Offset: 0x0017481D
		public float CurrentInstantBeauty()
		{
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return 0.5f;
			}
			return BeautyUtility.AverageBeautyPerceptible(this.pawn.PositionHeld, this.pawn.MapHeld);
		}

		// Token: 0x04002797 RID: 10135
		private const float BeautyImpactFactor = 0.1f;

		// Token: 0x04002798 RID: 10136
		private const float ThreshVeryUgly = 0.01f;

		// Token: 0x04002799 RID: 10137
		private const float ThreshUgly = 0.15f;

		// Token: 0x0400279A RID: 10138
		private const float ThreshNeutral = 0.35f;

		// Token: 0x0400279B RID: 10139
		private const float ThreshPretty = 0.65f;

		// Token: 0x0400279C RID: 10140
		private const float ThreshVeryPretty = 0.85f;

		// Token: 0x0400279D RID: 10141
		private const float ThreshBeautiful = 0.99f;
	}
}
