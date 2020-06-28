using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074E RID: 1870
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x001127C0 File Offset: 0x001109C0
		public static void ResetStaticData()
		{
			WorkGiver_Miner.NoPathTrans = "NoPath".Translate();
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x001127D6 File Offset: 0x001109D6
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine))
			{
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = designation.target.Cell + GenAdj.AdjacentCells[i];
					if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Mineable firstMineable = designation.target.Cell.GetFirstMineable(pawn.Map);
					if (firstMineable != null)
					{
						yield return firstMineable;
					}
				}
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x001127E6 File Offset: 0x001109E6
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Mine);
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x00112800 File Offset: 0x00110A00
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.def.mineable)
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
					if (intVec2.InBounds(t.Map) && ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) && intVec2.Walkable(t.Map) && !intVec2.Standable(t.Map))
					{
						Thing thing = null;
						List<Thing> thingList = intVec2.GetThingList(t.Map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
							{
								thing = thingList[k];
								break;
							}
						}
						if (thing != null)
						{
							Job job = HaulAIUtility.HaulAsideJobFor(pawn, thing);
							if (job != null)
							{
								return job;
							}
							JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
							return null;
						}
					}
				}
				JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Mine, t, 20000, true);
		}

		// Token: 0x04001AFF RID: 6911
		private static string NoPathTrans;

		// Token: 0x04001B00 RID: 6912
		private const int MiningJobTicks = 20000;
	}
}
