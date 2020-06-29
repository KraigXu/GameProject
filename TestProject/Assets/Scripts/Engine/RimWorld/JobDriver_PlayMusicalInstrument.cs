using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_PlayMusicalInstrument : JobDriver_SitFacingBuilding
	{
		
		
		public Building_MusicalInstrument MusicalInstrument
		{
			get
			{
				return (Building_MusicalInstrument)((Thing)this.job.GetTarget(TargetIndex.A));
			}
		}

		
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.AddPreInitAction(delegate
			{
				this.MusicalInstrument.StartPlaying(this.pawn);
			});
			toil.AddFinishAction(delegate
			{
				this.MusicalInstrument.StopPlaying();
			});
		}
	}
}
