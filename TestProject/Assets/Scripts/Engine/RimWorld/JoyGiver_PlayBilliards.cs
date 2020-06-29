using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		
		
		protected override bool CanDoDuringGathering
		{
			get
			{
				return true;
			}
		}

		
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}

		
		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && !c.Standable(t.Map))
				{
					return false;
				}
			}
			return true;
		}
	}
}
