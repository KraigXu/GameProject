using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ACD RID: 2765
	public class CompAbilityEffect_GiveHediff : CompAbilityEffect_WithDuration
	{
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x0015EAE6 File Offset: 0x0015CCE6
		public new CompProperties_AbilityGiveHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediff)this.props;
			}
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x0015EAF4 File Offset: 0x0015CCF4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			this.ApplyInner(target.Pawn, this.parent.pawn);
			if (this.Props.applyToSelf)
			{
				this.ApplyInner(this.parent.pawn, target.Pawn);
			}
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x0015EB48 File Offset: 0x0015CD48
		protected void ApplyInner(Pawn target, Pawn other)
		{
			if (target != null)
			{
				if (this.Props.replaceExisting)
				{
					Hediff firstHediffOfDef = target.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
					if (firstHediffOfDef != null)
					{
						target.health.RemoveHediff(firstHediffOfDef);
					}
				}
				Hediff hediff = HediffMaker.MakeHediff(this.Props.hediffDef, target, this.Props.onlyBrain ? target.health.hediffSet.GetBrain() : null);
				HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.ticksToDisappear = base.GetDurationSeconds(target).SecondsToTicks();
				}
				HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
				if (hediffComp_Link != null)
				{
					hediffComp_Link.other = other;
					hediffComp_Link.drawConnection = (target == this.parent.pawn);
				}
				target.health.AddHediff(hediff, null, null, null);
			}
		}
	}
}
