using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001032 RID: 4146
	public class Verb_MeleeApplyHediff : Verb_MeleeAttack
	{
		// Token: 0x06006326 RID: 25382 RVA: 0x00227384 File Offset: 0x00225584
		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (this.tool == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without a tool", 38381735, false);
				return damageResult;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without pawn target", 78330053, false);
				return damageResult;
			}
			foreach (BodyPartRecord part in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, this.verbProps.bodypartTagTarget, null))
			{
				damageResult.AddHediff(pawn.health.AddHediff(this.tool.hediff, part, null, null));
				damageResult.AddPart(pawn, part);
				damageResult.wounded = true;
			}
			return damageResult;
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x0022745C File Offset: 0x0022565C
		public override bool IsUsableOn(Thing target)
		{
			return target is Pawn;
		}
	}
}
