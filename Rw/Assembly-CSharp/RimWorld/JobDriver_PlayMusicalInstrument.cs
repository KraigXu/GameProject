using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000649 RID: 1609
	public class JobDriver_PlayMusicalInstrument : JobDriver_SitFacingBuilding
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000FC66E File Offset: 0x000FA86E
		public Building_MusicalInstrument MusicalInstrument
		{
			get
			{
				return (Building_MusicalInstrument)((Thing)this.job.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000FC686 File Offset: 0x000FA886
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
