using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA3 RID: 2979
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x060045C2 RID: 17858 RVA: 0x00178EDB File Offset: 0x001770DB
		// (set) Token: 0x060045C3 RID: 17859 RVA: 0x00178F00 File Offset: 0x00177100
		public Outfit CurrentOutfit
		{
			get
			{
				if (this.curOutfit == null)
				{
					this.curOutfit = Current.Game.outfitDatabase.DefaultOutfit();
				}
				return this.curOutfit;
			}
			set
			{
				if (this.curOutfit == value)
				{
					return;
				}
				this.curOutfit = value;
				if (this.pawn.mindState != null)
				{
					this.pawn.mindState.Notify_OutfitChanged();
				}
			}
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x00178F30 File Offset: 0x00177130
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x00178F43 File Offset: 0x00177143
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x00178F5D File Offset: 0x0017715D
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", Array.Empty<object>());
		}

		// Token: 0x04002826 RID: 10278
		public Pawn pawn;

		// Token: 0x04002827 RID: 10279
		private Outfit curOutfit;

		// Token: 0x04002828 RID: 10280
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();
	}
}
