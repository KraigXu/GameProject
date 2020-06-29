using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_ViewArt : JobDriver_VisitJoyThing
	{
		
		
		private Thing ArtThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		protected override void WaitTickAction()
		{
			float num = this.ArtThing.GetStatValue(StatDefOf.Beauty, true) / this.ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty, null);
			float extraJoyGainFactor = (num > 0f) ? num : 0f;
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)this.ArtThing);
		}
	}
}
