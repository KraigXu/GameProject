using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D97 RID: 3479
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		// Token: 0x0600549F RID: 21663 RVA: 0x001C3794 File Offset: 0x001C1994
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PsychicShock, pawn, null);
			BodyPartRecord part = null;
			pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out part);
			pawn.health.AddHediff(hediff, part, null, null);
		}
	}
}
