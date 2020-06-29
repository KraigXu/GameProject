using System;
using Verse;

namespace RimWorld
{
	
	public class WorkGiver_Uninstall : WorkGiver_RemoveBuilding
	{
		
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x000FB6CA File Offset: 0x000F98CA
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		
		// (get) Token: 0x0600305B RID: 12379 RVA: 0x0010F60E File Offset: 0x0010D80E
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Uninstall;
			}
		}

		
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.Claimable)
			{
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}
	}
}
