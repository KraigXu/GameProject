using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class CompAbilityEffect_ForceJob : CompAbilityEffect_WithDest
	{
		
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x0015E9E3 File Offset: 0x0015CBE3
		public new CompProperties_AbilityForceJob Props
		{
			get
			{
				return (CompProperties_AbilityForceJob)this.props;
			}
		}

		
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				Job job = JobMaker.MakeJob(this.Props.jobDef, new LocalTargetInfo(base.GetDestination(target).Cell));
				float num = 1f;
				if (this.Props.durationMultiplier != null)
				{
					num = pawn.GetStatValue(this.Props.durationMultiplier, true);
				}
				job.expiryInterval = (this.parent.def.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 10f) * num).SecondsToTicks();
				job.mote = MoteMaker.MakeThoughtBubble(pawn, this.parent.def.iconPath, true);
				pawn.jobs.StopAll(false, true);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
			}
		}
	}
}
