using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_OperateScanner : WorkGiver_Scanner
	{
		
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x00112A3C File Offset: 0x00110C3C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(this.ScannerDef);
			}
		}

		
		// (get) Token: 0x0600310A RID: 12554 RVA: 0x00112A49 File Offset: 0x00110C49
		public ThingDef ScannerDef
		{
			get
			{
				return this.def.scannerDef;
			}
		}

		
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x0010FDBF File Offset: 0x0010DFBF
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(this.ScannerDef);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == pawn.Faction)
				{
					CompScanner compScanner = list[i].TryGetComp<CompScanner>();
					if (compScanner != null && compScanner.CanUseNow)
					{
						return false;
					}
				}
			}
			return true;
		}

		
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && !building.IsForbidden(pawn) && pawn.CanReserve(building, 1, -1, null, forced) && building.TryGetComp<CompScanner>().CanUseNow && !building.IsBurning();
		}

		
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateScanner, t, 1500, true);
		}
	}
}
