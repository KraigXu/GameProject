using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_MarryAdjacentPawn : ThinkNode_JobGiver
	{
		
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}

			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = pawn.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(pawn.Map))
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					Predicate<Thing> match = (((Thing x) => x is Pawn && this.CanMarry(pawn, (Pawn)x)));
					if ((match ) == null)
					{
						
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

		
		private bool CanMarry(Pawn pawn, Pawn toMarry)
		{
			return !toMarry.Drafted && pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, toMarry);
		}
	}
}
