using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105B RID: 4187
	public class PlaceWorker_ShowInstrumentAoE : PlaceWorker
	{
		// Token: 0x060063D2 RID: 25554 RVA: 0x00229B20 File Offset: 0x00227D20
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = (ThingDef)checkingDef;
			PlaceWorker_ShowInstrumentAoE.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(thingDef.building.instrumentRange);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = loc + GenRadial.RadialPattern[i];
				if (Building_MusicalInstrument.IsAffectedByInstrument(thingDef, loc, intVec, map))
				{
					PlaceWorker_ShowInstrumentAoE.tmpCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(PlaceWorker_ShowInstrumentAoE.tmpCells);
			return true;
		}

		// Token: 0x04003CDC RID: 15580
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
