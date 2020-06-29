using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (Rand.Value < 1f && RCellFinder.TryFindRandomCellNearWith(pawn.Position, (IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire, pawn.Map, out c, 5, this.MaxDistance.RandomInRange))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		
		private const float ActivateChance = 1f;

		
		private readonly IntRange MaxDistance = new IntRange(10, 16);
	}
}
