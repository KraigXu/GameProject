using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	
	public static class Toils_JobTransforms
	{
		
		public static Toil ExtractNextTargetFromQueue(TargetIndex ind, bool failIfCountFromQueueTooBig = true)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				if (failIfCountFromQueueTooBig && !curJob.countQueue.NullOrEmpty<int>() && targetQueue[0].HasThing && curJob.countQueue[0] > targetQueue[0].Thing.stackCount)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				curJob.SetTarget(ind, targetQueue[0]);
				targetQueue.RemoveAt(0);
				if (!curJob.countQueue.NullOrEmpty<int>())
				{
					curJob.count = curJob.countQueue[0];
					curJob.countQueue.RemoveAt(0);
				}
			};
			return toil;
		}

		
		public static Toil ClearQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				List<LocalTargetInfo> targetQueue = toil.actor.jobs.curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				targetQueue.Clear();
			};
			return toil;
		}

		
		public static Toil ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex ind, Func<Thing, bool> validator = null)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				actor.jobs.curJob.GetTargetQueue(ind).RemoveAll((LocalTargetInfo ta) => !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(actor) || (validator != null && !validator(ta.Thing)));
			};
			return toil;
		}

		
		private static IEnumerable<IntVec3> IngredientPlaceCellsInOrder(Thing destination)
		{
			//Toils_JobTransforms.yieldedIngPlaceCells.Clear();
			//try
			//{
			//	Toils_JobTransforms.c__DisplayClass4_0 c__DisplayClass4_ = new Toils_JobTransforms.c__DisplayClass4_0();
			//	c__DisplayClass4_.interactCell = destination.Position;
			//	IBillGiver billGiver = destination as IBillGiver;
			//	if (billGiver != null)
			//	{
			//		c__DisplayClass4_.interactCell = ((Thing)billGiver).InteractionCell;
			//		IEnumerable<IntVec3> ingredientStackCells = billGiver.IngredientStackCells;
			//		Func<IntVec3, int> keySelector;
			//		if ((keySelector = c__DisplayClass4_.9__0) == null)
			//		{
			//			keySelector = (c__DisplayClass4_.9__0 = ((IntVec3 c) => (c - c__DisplayClass4_.interactCell).LengthHorizontalSquared));
			//		}
			//		foreach (IntVec3 intVec in ingredientStackCells.OrderBy(keySelector))
			//		{
			//			Toils_JobTransforms.yieldedIngPlaceCells.Add(intVec);
			//			yield return intVec;
			//		}
			//		IEnumerator<IntVec3> enumerator = null;
			//	}
			//	int num;
			//	for (int i = 0; i < 200; i = num + 1)
			//	{
			//		IntVec3 intVec2 = c__DisplayClass4_.interactCell + GenRadial.RadialPattern[i];
			//		if (!Toils_JobTransforms.yieldedIngPlaceCells.Contains(intVec2))
			//		{
			//			Building edifice = intVec2.GetEdifice(destination.Map);
			//			if (edifice == null || edifice.def.passability != Traversability.Impassable || edifice.def.surfaceType != SurfaceType.None)
			//			{
			//				yield return intVec2;
			//			}
			//		}
			//		num = i;
			//	}
			//	c__DisplayClass4_ = null;
			//}
			//finally
			//{
			//	Toils_JobTransforms.yieldedIngPlaceCells.Clear();
			//}
			yield break;
			yield break;
		}

		
		public static Toil SetTargetToIngredientPlaceCell(TargetIndex facilityInd, TargetIndex carryItemInd, TargetIndex cellTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(carryItemInd).Thing;
				IntVec3 c = IntVec3.Invalid;
				foreach (IntVec3 intVec in Toils_JobTransforms.IngredientPlaceCellsInOrder(curJob.GetTarget(facilityInd).Thing))
				{
					if (!c.IsValid)
					{
						c = intVec;
					}
					bool flag = false;
					List<Thing> list = actor.Map.thingGrid.ThingsListAt(intVec);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].def.category == ThingCategory.Item && (list[i].def != thing.def || list[i].stackCount == list[i].def.stackLimit))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						curJob.SetTarget(cellTargetInd, intVec);
						return;
					}
				}
				curJob.SetTarget(cellTargetInd, c);
			};
			return toil;
		}

		
		public static Toil MoveCurrentTargetIntoQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Job curJob = toil.actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (target.IsValid)
				{
					List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
					if (targetQueue == null)
					{
						curJob.AddQueuedTarget(ind, target);
					}
					else
					{
						targetQueue.Insert(0, target);
					}
					curJob.SetTarget(ind, null);
				}
			};
			return toil;
		}

		
		public static Toil SucceedOnNoTargetInQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.EndOnNoTargetInQueue(ind, JobCondition.Succeeded);
			return toil;
		}

		
		private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();
	}
}
