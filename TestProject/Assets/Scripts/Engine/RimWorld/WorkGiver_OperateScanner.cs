using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000750 RID: 1872
	public class WorkGiver_OperateScanner : WorkGiver_Scanner
	{
		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x00112A3C File Offset: 0x00110C3C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(this.ScannerDef);
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x0600310A RID: 12554 RVA: 0x00112A49 File Offset: 0x00110C49
		public ThingDef ScannerDef
		{
			get
			{
				return this.def.scannerDef;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x0010FDBF File Offset: 0x0010DFBF
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x00112A58 File Offset: 0x00110C58
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

		// Token: 0x0600310E RID: 12558 RVA: 0x00112ABC File Offset: 0x00110CBC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && !building.IsForbidden(pawn) && pawn.CanReserve(building, 1, -1, null, forced) && building.TryGetComp<CompScanner>().CanUseNow && !building.IsBurning();
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x00112B1E File Offset: 0x00110D1E
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateScanner, t, 1500, true);
		}
	}
}
