using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A7 RID: 1447
	public class JobGiver_MoveToStandable : ThinkNode_JobGiver
	{
		// Token: 0x060028A3 RID: 10403 RVA: 0x000EF560 File Offset: 0x000ED760
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Drafted)
			{
				return null;
			}
			if (pawn.pather.Moving)
			{
				return null;
			}
			if (!pawn.Position.Standable(pawn.Map))
			{
				return this.FindBetterPositionJob(pawn);
			}
			List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn2 = thingList[i] as Pawn;
				if (pawn2 != null && pawn2 != pawn && pawn2.Faction == pawn.Faction && pawn2.Drafted && !pawn2.pather.MovingNow)
				{
					return this.FindBetterPositionJob(pawn);
				}
			}
			return null;
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000EF604 File Offset: 0x000ED804
		private Job FindBetterPositionJob(Pawn pawn)
		{
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(pawn.Position, pawn);
			if (intVec.IsValid && intVec != pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Goto, intVec);
			}
			return null;
		}
	}
}
