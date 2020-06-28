using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200069E RID: 1694
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		// Token: 0x06002DF8 RID: 11768 RVA: 0x00102944 File Offset: 0x00100B44
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.nuzzleMtbHours <= 0f)
			{
				return null;
			}
			Pawn t;
			if (!(from p in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, 40f) && pawn.GetRoom(RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable) && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow(false)
			select p).TryRandomElement(out t))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Nuzzle, t);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			job.expiryInterval = 3000;
			return job;
		}

		// Token: 0x04001A49 RID: 6729
		private const float MaxNuzzleDistance = 40f;
	}
}
