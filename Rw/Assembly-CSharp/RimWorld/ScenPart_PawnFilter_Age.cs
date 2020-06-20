using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1D RID: 3101
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		// Token: 0x060049F2 RID: 18930 RVA: 0x001904E5 File Offset: 0x0018E6E5
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.IntRange(listing.GetScenPartRect(this, 31f), (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, null, 4);
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x0019050C File Offset: 0x0018E70C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x0019053C File Offset: 0x0018E73C
		public override string Summary(Scenario scen)
		{
			if (this.allowedAgeRange.min > 15)
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeRange".Translate(this.allowedAgeRange.min, this.allowedAgeRange.max);
				}
				return "ScenPart_StartingPawnAgeMin".Translate(this.allowedAgeRange.min);
			}
			else
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeMax".Translate(this.allowedAgeRange.max);
				}
				throw new Exception();
			}
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x001905F1 File Offset: 0x0018E7F1
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0019060C File Offset: 0x0018E80C
		public override void Randomize()
		{
			this.allowedAgeRange = new IntRange(15, 120);
			switch (Rand.RangeInclusive(0, 2))
			{
			case 0:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				break;
			case 1:
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			case 2:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			}
			this.MakeAllowedAgeRangeValid();
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x001906A0 File Offset: 0x0018E8A0
		private void MakeAllowedAgeRangeValid()
		{
			if (this.allowedAgeRange.max < 19)
			{
				this.allowedAgeRange.max = 19;
			}
			if (this.allowedAgeRange.max - this.allowedAgeRange.min < 4)
			{
				this.allowedAgeRange.min = this.allowedAgeRange.max - 4;
			}
		}

		// Token: 0x04002A08 RID: 10760
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		// Token: 0x04002A09 RID: 10761
		private const int RangeMin = 15;

		// Token: 0x04002A0A RID: 10762
		private const int RangeMax = 120;

		// Token: 0x04002A0B RID: 10763
		private const int RangeMinMax = 19;

		// Token: 0x04002A0C RID: 10764
		private const int RangeMinWidth = 4;
	}
}
