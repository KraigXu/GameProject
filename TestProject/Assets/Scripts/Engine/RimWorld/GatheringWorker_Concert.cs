using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class GatheringWorker_Concert : GatheringWorker
	{
		
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Concert(spot, organizer, this.def);
		}

		
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			bool enjoyableOutside = JoyUtility.EnjoyableOutsideNow(organizer, null);
			Map map = organizer.Map;
			IEnumerable<Building_MusicalInstrument> enumerable = organizer.Map.listerBuildings.AllBuildingsColonistOfClass<Building_MusicalInstrument>();
			bool result;
			try
			{
				int num = -1;
				foreach (Building_MusicalInstrument building_MusicalInstrument in enumerable)
				{
					if (GatheringsUtility.ValidateGatheringSpot(building_MusicalInstrument.InteractionCell, this.def, organizer, enjoyableOutside) && GatheringWorker_Concert.InstrumentAccessible(building_MusicalInstrument, organizer))
					{
						float instrumentRange = building_MusicalInstrument.def.building.instrumentRange;
						if ((float)num < instrumentRange)
						{
							GatheringWorker_Concert.tmpInstruments.Clear();
						}
						else if ((float)num > instrumentRange)
						{
							continue;
						}
						GatheringWorker_Concert.tmpInstruments.Add(building_MusicalInstrument);
					}
				}
				Building_MusicalInstrument building_MusicalInstrument2;
				if (!GatheringWorker_Concert.tmpInstruments.TryRandomElement(out building_MusicalInstrument2))
				{
					spot = IntVec3.Invalid;
					result = false;
				}
				else
				{
					spot = building_MusicalInstrument2.InteractionCell;
					result = true;
				}
			}
			finally
			{
				GatheringWorker_Concert.tmpInstruments.Clear();
			}
			return result;
		}

		
		public static bool InstrumentAccessible(Building_MusicalInstrument i, Pawn p)
		{
			return !i.IsBeingPlayed && p.CanReserveAndReach(i.InteractionCell, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false);
		}

		
		private static List<Building_MusicalInstrument> tmpInstruments = new List<Building_MusicalInstrument>();
	}
}
