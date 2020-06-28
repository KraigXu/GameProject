using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200054D RID: 1357
	public class MentalState_Slaughterer : MentalState
	{
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000E43F0 File Offset: 0x000E25F0
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000E4415 File Offset: 0x000E2615
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x000E4423 File Offset: 0x000E2623
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x000E4450 File Offset: 0x000E2650
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x000E44AC File Offset: 0x000E26AC
		public override void Notify_SlaughteredAnimal()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.animalsSlaughtered++;
			if (this.animalsSlaughtered >= 4)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04001746 RID: 5958
		private int lastSlaughterTicks = -1;

		// Token: 0x04001747 RID: 5959
		private int animalsSlaughtered;

		// Token: 0x04001748 RID: 5960
		private const int NoAnimalToSlaughterCheckInterval = 600;

		// Token: 0x04001749 RID: 5961
		private const int MinTicksBetweenSlaughter = 3750;

		// Token: 0x0400174A RID: 5962
		private const int MaxAnimalsSlaughtered = 4;
	}
}
