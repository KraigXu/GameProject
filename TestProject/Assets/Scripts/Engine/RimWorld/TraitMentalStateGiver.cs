using System;
using Verse;

namespace RimWorld
{
	
	public class TraitMentalStateGiver
	{
		
		public virtual bool CheckGive(Pawn pawn, int checkInterval)
		{
			if (this.traitDegreeData.randomMentalState == null)
			{
				return false;
			}
			float curMood = pawn.mindState.mentalBreaker.CurMood;
			return Rand.MTBEventOccurs(this.traitDegreeData.randomMentalStateMtbDaysMoodCurve.Evaluate(curMood), 60000f, (float)checkInterval) && this.traitDegreeData.randomMentalState.Worker.StateCanOccur(pawn) && pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.randomMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.label), false, false, null, false);
		}

		
		public TraitDegreeData traitDegreeData;
	}
}
