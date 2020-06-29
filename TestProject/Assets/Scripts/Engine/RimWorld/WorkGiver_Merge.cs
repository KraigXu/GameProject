using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_Merge : WorkGiver_Scanner
	{
		
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging();
		}

		
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging().Count == 0;
		}

		
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.stackCount == t.def.stackLimit)
			{
				return null;
			}
			if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				return null;
			}
			SlotGroup slotGroup = t.GetSlotGroup();
			if (slotGroup == null)
			{
				return null;
			}
			if (!pawn.CanReserve(t.Position, 1, -1, null, forced))
			{
				return null;
			}
			foreach (Thing thing in slotGroup.HeldThings)
			{
				if (thing != t && thing.CanStackWith(t) && (forced || thing.stackCount >= t.stackCount) && thing.stackCount < thing.def.stackLimit && pawn.CanReserve(thing.Position, 1, -1, null, forced) && pawn.CanReserve(thing, 1, -1, null, false) && thing.Position.IsValidStorageFor(thing.Map, t))
				{
					Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, thing.Position);
					job.count = Mathf.Min(thing.def.stackLimit - thing.stackCount, t.stackCount);
					job.haulMode = HaulMode.ToCellStorage;
					return job;
				}
			}
			JobFailReason.Is("NoMergeTarget".Translate(), null);
			return null;
		}
	}
}
