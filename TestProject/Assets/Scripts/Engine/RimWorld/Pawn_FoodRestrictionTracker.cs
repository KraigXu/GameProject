using System;
using Verse;

namespace RimWorld
{
	
	public class Pawn_FoodRestrictionTracker : IExposable
	{
		
		// (get) Token: 0x060044E3 RID: 17635 RVA: 0x0017408F File Offset: 0x0017228F
		// (set) Token: 0x060044E4 RID: 17636 RVA: 0x001740B4 File Offset: 0x001722B4
		public FoodRestriction CurrentFoodRestriction
		{
			get
			{
				if (this.curRestriction == null)
				{
					this.curRestriction = Current.Game.foodRestrictionDatabase.DefaultFoodRestriction();
				}
				return this.curRestriction;
			}
			set
			{
				this.curRestriction = value;
			}
		}

		
		// (get) Token: 0x060044E5 RID: 17637 RVA: 0x001740C0 File Offset: 0x001722C0
		public bool Configurable
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.Destroyed && (this.pawn.Faction == Faction.OfPlayer || this.pawn.HostFaction == Faction.OfPlayer);
			}
		}

		
		public Pawn_FoodRestrictionTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public Pawn_FoodRestrictionTracker()
		{
		}

		
		public FoodRestriction GetCurrentRespectedRestriction(Pawn getter = null)
		{
			if (!this.Configurable)
			{
				return null;
			}
			if (this.pawn.Faction != Faction.OfPlayer && (getter == null || getter.Faction != Faction.OfPlayer))
			{
				return null;
			}
			if (this.pawn.InMentalState)
			{
				return null;
			}
			return this.CurrentFoodRestriction;
		}

		
		public void ExposeData()
		{
			Scribe_References.Look<FoodRestriction>(ref this.curRestriction, "curRestriction", false);
		}

		
		public Pawn pawn;

		
		private FoodRestriction curRestriction;
	}
}
