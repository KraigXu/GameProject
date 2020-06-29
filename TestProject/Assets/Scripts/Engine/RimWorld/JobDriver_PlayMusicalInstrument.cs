using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_PlayMusicalInstrument : JobDriver_SitFacingBuilding
	{
		
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000FC66E File Offset: 0x000FA86E
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
