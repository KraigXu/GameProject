using System;
using Verse;

namespace RimWorld
{
	
	public class WorkGiver_Deconstruct : WorkGiver_RemoveBuilding
	{
		
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x000FB242 File Offset: 0x000F9442
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		
		// (get) Token: 0x06003047 RID: 12359 RVA: 0x0010F425 File Offset: 0x0010D625
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Deconstruct;
			}
		}

		
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t.GetInnerIfMinified() as Building;
			return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
