using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001075 RID: 4213
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x06006415 RID: 25621 RVA: 0x0022ACE8 File Offset: 0x00228EE8
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x06006416 RID: 25622 RVA: 0x0022AD05 File Offset: 0x00228F05
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x06006417 RID: 25623 RVA: 0x0022AD23 File Offset: 0x00228F23
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor)))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x06006418 RID: 25624 RVA: 0x0022AD64 File Offset: 0x00228F64
		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, 0);
			float num = 0.65f * quality;
			if (Rand.Value < num)
			{
				if (batchPosition == 0 && this.pawn.Spawned)
				{
					MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatSuccess".Translate(num.ToStringPercent()), 6.5f);
				}
				this.Severity -= 0.3f;
				return;
			}
			if (batchPosition == 0 && this.pawn.Spawned)
			{
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatFailed".Translate(num.ToStringPercent()), 6.5f);
			}
		}

		// Token: 0x04003CE1 RID: 15585
		private float intervalFactor;

		// Token: 0x04003CE2 RID: 15586
		private const int SeverityChangeInterval = 5000;

		// Token: 0x04003CE3 RID: 15587
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x04003CE4 RID: 15588
		private const float TendSeverityReduction = 0.3f;
	}
}
