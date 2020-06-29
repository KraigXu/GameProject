﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_MoveDrugsToInventory : ThinkNode_JobGiver
	{
		
		public override float GetPriority(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.AllowedToTakeToInventory(currentPolicy[i].drug))
				{
					return 7.5f;
				}
			}
			return 0f;
		}

		
		protected override Job TryGiveJob(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.AllowedToTakeToInventory(currentPolicy[i].drug))
				{
					Thing thing = this.FindDrugFor(pawn, currentPolicy[i].drug);
					if (thing != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
						job.count = currentPolicy[i].takeToInventory - pawn.inventory.innerContainer.TotalStackCountOfDef(thing.def);
						return job;
					}
				}
			}
			return null;
		}

		
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => this.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		
		private bool DrugValidator(Pawn pawn, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned)
			{
				if (drug.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(drug, 10, 1, null, false))
				{
					return false;
				}
			}
			return true;
		}
	}
}
