using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091A RID: 2330
	public class TraitMentalStateGiver
	{
		// Token: 0x06003760 RID: 14176 RVA: 0x00129A44 File Offset: 0x00127C44
		public virtual bool CheckGive(Pawn pawn, int checkInterval)
		{
			if (this.traitDegreeData.randomMentalState == null)
			{
				return false;
			}
			float curMood = pawn.mindState.mentalBreaker.CurMood;
			return Rand.MTBEventOccurs(this.traitDegreeData.randomMentalStateMtbDaysMoodCurve.Evaluate(curMood), 60000f, (float)checkInterval) && this.traitDegreeData.randomMentalState.Worker.StateCanOccur(pawn) && pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.randomMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.label), false, false, null, false);
		}

		// Token: 0x040020B2 RID: 8370
		public TraitDegreeData traitDegreeData;
	}
}
