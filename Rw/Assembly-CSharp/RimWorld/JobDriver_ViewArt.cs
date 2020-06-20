using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200064F RID: 1615
	public class JobDriver_ViewArt : JobDriver_VisitJoyThing
	{
		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002C1E RID: 11294 RVA: 0x000FCCD8 File Offset: 0x000FAED8
		private Thing ArtThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000FCCFC File Offset: 0x000FAEFC
		protected override void WaitTickAction()
		{
			float num = this.ArtThing.GetStatValue(StatDefOf.Beauty, true) / this.ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty, null);
			float extraJoyGainFactor = (num > 0f) ? num : 0f;
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)this.ArtThing);
		}
	}
}
