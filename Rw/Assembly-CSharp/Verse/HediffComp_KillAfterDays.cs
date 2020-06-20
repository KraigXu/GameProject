using System;

namespace Verse
{
	// Token: 0x02000264 RID: 612
	public class HediffComp_KillAfterDays : HediffComp
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x0005EB40 File Offset: 0x0005CD40
		public HediffCompProperties_KillAfterDays Props
		{
			get
			{
				return (HediffCompProperties_KillAfterDays)this.props;
			}
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0005EB4D File Offset: 0x0005CD4D
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.addedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0005EB60 File Offset: 0x0005CD60
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame - this.addedTick >= 60000 * this.Props.days)
			{
				base.Pawn.Kill(null, this.parent);
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0005EBAB File Offset: 0x0005CDAB
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.addedTick, "addedTick", 0, false);
		}

		// Token: 0x04000C1D RID: 3101
		private int addedTick;
	}
}
