using System;
using Verse;

namespace RimWorld
{
	
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x001D198E File Offset: 0x001CFB8E
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
		}
	}
}
