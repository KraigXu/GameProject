using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C6 RID: 1734
	public class JobGiver_MarryAdjacentPawn : ThinkNode_JobGiver
	{
		// Token: 0x06002E90 RID: 11920 RVA: 0x00105BE8 File Offset: 0x00103DE8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			Predicate<Thing> <>9__0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = pawn.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(pawn.Map))
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					Predicate<Thing> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Thing x) => x is Pawn && this.CanMarry(pawn, (Pawn)x)));
					}
					Thing thing = thingList.Find(match);
					if (thing != null)
					{
						return JobMaker.MakeJob(JobDefOf.MarryAdjacentPawn, thing);
					}
				}
			}
			return null;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x00105CA8 File Offset: 0x00103EA8
		private bool CanMarry(Pawn pawn, Pawn toMarry)
		{
			return !toMarry.Drafted && pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, toMarry);
		}
	}
}
