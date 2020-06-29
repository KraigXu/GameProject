using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public abstract class WorkGiver_Grower : WorkGiver_Scanner
	{
		
		
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		
		protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			return true;
		}

		
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			Danger maxDanger = pawn.NormalMaxDanger();
			List<Building> bList = pawn.Map.listerBuildings.allBuildingsColonist;
			int num;
			for (int i = 0; i < bList.Count; i = num + 1)
			{
				Building_PlantGrower building_PlantGrower = bList[i] as Building_PlantGrower;
				if (building_PlantGrower != null && this.ExtraRequirements(building_PlantGrower, pawn) && !building_PlantGrower.IsForbidden(pawn) && pawn.CanReach(building_PlantGrower, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn) && !building_PlantGrower.IsBurning())
				{
					foreach (IntVec3 intVec in building_PlantGrower.OccupiedRect())
					{
						yield return intVec;
					}
					WorkGiver_Grower.wantedPlantDef = null;
				}
				num = i;
			}
			WorkGiver_Grower.wantedPlantDef = null;
			List<Zone> zonesList = pawn.Map.zoneManager.AllZones;
			for (int i = 0; i < zonesList.Count; i = num + 1)
			{
				Zone_Growing growZone = zonesList[i] as Zone_Growing;
				if (growZone != null)
				{
					if (growZone.cells.Count == 0)
					{
						Log.ErrorOnce("Grow zone has 0 cells: " + growZone, -563487, false);
					}
					else if (this.ExtraRequirements(growZone, pawn) && !growZone.ContainsStaticFire && pawn.CanReach(growZone.Cells[0], PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
					{
						for (int j = 0; j < growZone.cells.Count; j = num + 1)
						{
							yield return growZone.cells[j];
							num = j;
						}
						WorkGiver_Grower.wantedPlantDef = null;
						growZone = null;
					}
				}
				num = i;
			}
			WorkGiver_Grower.wantedPlantDef = null;
			yield break;
			yield break;
		}

		
		public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetPlantToGrowSettable(map);
			if (plantToGrowSettable == null)
			{
				return null;
			}
			return plantToGrowSettable.GetPlantDefToGrow();
		}

		
		protected static ThingDef wantedPlantDef;
	}
}
