using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ITab_Pawn_Health : ITab
	{
		
		// (get) Token: 0x06005BBE RID: 23486 RVA: 0x001FAC58 File Offset: 0x001F8E58
		private Pawn PawnForHealth
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return null;
			}
		}

		
		public ITab_Pawn_Health()
		{
			this.size = new Vector2(630f, 430f);
			this.labelKey = "TabHealth";
			this.tutorTag = "Health";
		}

		
		protected override void FillTab()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth == null)
			{
				Log.Error("Health tab found no selected pawn to display.", false);
				return;
			}
			Corpse corpse = base.SelThing as Corpse;
			bool showBloodLoss = corpse == null || corpse.Age < 60000;
			HealthCardUtility.DrawPawnHealthCard(new Rect(0f, 20f, this.size.x, this.size.y - 20f), pawnForHealth, this.ShouldAllowOperations(), showBloodLoss, base.SelThing);
		}

		
		private bool ShouldAllowOperations()
		{
			Pawn pawn = this.PawnForHealth;
			return !pawn.Dead && base.SelThing.def.AllRecipes.Any((RecipeDef x) => x.AvailableNow && x.AvailableOnNow(pawn)) && (pawn.Faction == Faction.OfPlayer || (pawn.IsPrisonerOfColony || (pawn.HostFaction == Faction.OfPlayer && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))) || ((!pawn.RaceProps.IsFlesh || pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer)) && (!pawn.RaceProps.Humanlike && pawn.Downed)));
		}

		
		private const int HideBloodLossTicksThreshold = 60000;

		
		public const float Width = 630f;
	}
}
