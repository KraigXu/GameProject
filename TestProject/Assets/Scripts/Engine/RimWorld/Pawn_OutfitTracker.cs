using System;
using Verse;

namespace RimWorld
{
	
	public class Pawn_OutfitTracker : IExposable
	{
		
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

		
		public Pawn_OutfitTracker()
		{
		}

		
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", Array.Empty<object>());
		}

		
		public Pawn pawn;

		
		private Outfit curOutfit;

		
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();
	}
}
