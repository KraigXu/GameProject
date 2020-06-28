using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7D RID: 2941
	public class Pawn_FoodRestrictionTracker : IExposable
	{
		// Token: 0x17000C06 RID: 3078
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

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x060044E5 RID: 17637 RVA: 0x001740C0 File Offset: 0x001722C0
		public bool Configurable
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.Destroyed && (this.pawn.Faction == Faction.OfPlayer || this.pawn.HostFaction == Faction.OfPlayer);
			}
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x00174114 File Offset: 0x00172314
		public Pawn_FoodRestrictionTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public Pawn_FoodRestrictionTracker()
		{
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x00174124 File Offset: 0x00172324
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

		// Token: 0x060044E9 RID: 17641 RVA: 0x00174174 File Offset: 0x00172374
		public void ExposeData()
		{
			Scribe_References.Look<FoodRestriction>(ref this.curRestriction, "curRestriction", false);
		}

		// Token: 0x04002757 RID: 10071
		public Pawn pawn;

		// Token: 0x04002758 RID: 10072
		private FoodRestriction curRestriction;
	}
}
