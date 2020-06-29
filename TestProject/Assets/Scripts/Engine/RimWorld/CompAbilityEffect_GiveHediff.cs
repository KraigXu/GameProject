using System;
using Verse;

namespace RimWorld
{
	
	public class CompAbilityEffect_GiveHediff : CompAbilityEffect_WithDuration
	{
		
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x0015EAE6 File Offset: 0x0015CCE6
		public new CompProperties_AbilityGiveHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediff)this.props;
			}
		}

		
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			this.ApplyInner(target.Pawn, this.parent.pawn);
			if (this.Props.applyToSelf)
			{
				this.ApplyInner(this.parent.pawn, target.Pawn);
			}
		}

		
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
