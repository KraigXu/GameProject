using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000268 RID: 616
	public class HediffComp_ReactOnDamage : HediffComp
	{
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0005EDE8 File Offset: 0x0005CFE8
		public HediffCompProperties_ReactOnDamage Props
		{
			get
			{
				return (HediffCompProperties_ReactOnDamage)this.props;
			}
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0005EDF5 File Offset: 0x0005CFF5
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.damageDefIncoming == dinfo.Def)
			{
				this.React();
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0005EE14 File Offset: 0x0005D014
		private void React()
		{
			if (this.Props.createHediff != null)
			{
				BodyPartRecord part = this.parent.Part;
				if (this.Props.createHediffOn != null)
				{
					part = this.parent.pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == this.Props.createHediffOn, null);
				}
				this.parent.pawn.health.AddHediff(this.Props.createHediff, part, null, null);
			}
			if (this.Props.vomit)
			{
				this.parent.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
			}
		}
	}
}
