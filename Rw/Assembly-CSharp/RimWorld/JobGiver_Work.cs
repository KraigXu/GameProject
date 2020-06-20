using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000718 RID: 1816
	public class JobGiver_Work : ThinkNode
	{
		// Token: 0x06002FC0 RID: 12224 RVA: 0x0010CD69 File Offset: 0x0010AF69
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Work jobGiver_Work = (JobGiver_Work)base.DeepCopy(resolve);
			jobGiver_Work.emergency = this.emergency;
			return jobGiver_Work;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x0010CD84 File Offset: 0x0010AF84
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return 0f;
			}
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				return 5.5f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Work)
			{
				return 9f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				return 3f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				return 2f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
			{
				return 2f;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x0010CE14 File Offset: 0x0010B014
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobGiver_Work.<>c__DisplayClass3_0 <>c__DisplayClass3_ = new JobGiver_Work.<>c__DisplayClass3_0();
			<>c__DisplayClass3_.pawn = pawn;
			if (this.emergency && <>c__DisplayClass3_.pawn.mindState.priorityWork.IsPrioritized)
			{
				List<WorkGiverDef> workGiversByPriority = <>c__DisplayClass3_.pawn.mindState.priorityWork.WorkGiver.workType.workGiversByPriority;
				for (int i = 0; i < workGiversByPriority.Count; i++)
				{
					WorkGiver worker = workGiversByPriority[i].Worker;
					if (this.WorkGiversRelated(<>c__DisplayClass3_.pawn.mindState.priorityWork.WorkGiver, worker.def))
					{
						Job job = this.GiverTryGiveJobPrioritized(<>c__DisplayClass3_.pawn, worker, <>c__DisplayClass3_.pawn.mindState.priorityWork.Cell);
						if (job != null)
						{
							job.playerForced = true;
							return new ThinkResult(job, this, new JobTag?(workGiversByPriority[i].tagToGive), false);
						}
					}
				}
				<>c__DisplayClass3_.pawn.mindState.priorityWork.Clear();
			}
			List<WorkGiver> list = (!this.emergency) ? <>c__DisplayClass3_.pawn.workSettings.WorkGiversInOrderNormal : <>c__DisplayClass3_.pawn.workSettings.WorkGiversInOrderEmergency;
			int num = -999;
			<>c__DisplayClass3_.bestTargetOfLastPriority = TargetInfo.Invalid;
			<>c__DisplayClass3_.scannerWhoProvidedTarget = null;
			for (int j = 0; j < list.Count; j++)
			{
				WorkGiver workGiver = list[j];
				if (workGiver.def.priorityInType != num && <>c__DisplayClass3_.bestTargetOfLastPriority.IsValid)
				{
					break;
				}
				if (this.PawnCanUseWorkGiver(<>c__DisplayClass3_.pawn, workGiver))
				{
					try
					{
						JobGiver_Work.<>c__DisplayClass3_1 <>c__DisplayClass3_2 = new JobGiver_Work.<>c__DisplayClass3_1();
						<>c__DisplayClass3_2.CS$<>8__locals1 = <>c__DisplayClass3_;
						Job job2 = workGiver.NonScanJob(<>c__DisplayClass3_2.CS$<>8__locals1.pawn);
						if (job2 != null)
						{
							return new ThinkResult(job2, this, new JobTag?(list[j].def.tagToGive), false);
						}
						<>c__DisplayClass3_2.scanner = (workGiver as WorkGiver_Scanner);
						if (<>c__DisplayClass3_2.scanner != null)
						{
							if (<>c__DisplayClass3_2.scanner.def.scanThings)
							{
								Predicate<Thing> validator = (Thing t) => !t.IsForbidden(<>c__DisplayClass3_2.CS$<>8__locals1.pawn) && <>c__DisplayClass3_2.scanner.HasJobOnThing(<>c__DisplayClass3_2.CS$<>8__locals1.pawn, t, false);
								IEnumerable<Thing> enumerable = <>c__DisplayClass3_2.scanner.PotentialWorkThingsGlobal(<>c__DisplayClass3_2.CS$<>8__locals1.pawn);
								Thing thing;
								if (<>c__DisplayClass3_2.scanner.Prioritized)
								{
									IEnumerable<Thing> enumerable2 = enumerable;
									if (enumerable2 == null)
									{
										enumerable2 = <>c__DisplayClass3_2.CS$<>8__locals1.pawn.Map.listerThings.ThingsMatching(<>c__DisplayClass3_2.scanner.PotentialWorkThingRequest);
									}
									if (<>c__DisplayClass3_2.scanner.AllowUnreachable)
									{
										thing = GenClosest.ClosestThing_Global(<>c__DisplayClass3_2.CS$<>8__locals1.pawn.Position, enumerable2, 99999f, validator, (Thing x) => <>c__DisplayClass3_2.scanner.GetPriority(<>c__DisplayClass3_2.CS$<>8__locals1.pawn, x));
									}
									else
									{
										thing = GenClosest.ClosestThing_Global_Reachable(<>c__DisplayClass3_2.CS$<>8__locals1.pawn.Position, <>c__DisplayClass3_2.CS$<>8__locals1.pawn.Map, enumerable2, <>c__DisplayClass3_2.scanner.PathEndMode, TraverseParms.For(<>c__DisplayClass3_2.CS$<>8__locals1.pawn, <>c__DisplayClass3_2.scanner.MaxPathDanger(<>c__DisplayClass3_2.CS$<>8__locals1.pawn), TraverseMode.ByPawn, false), 9999f, validator, (Thing x) => <>c__DisplayClass3_2.scanner.GetPriority(<>c__DisplayClass3_2.CS$<>8__locals1.pawn, x));
									}
								}
								else if (<>c__DisplayClass3_2.scanner.AllowUnreachable)
								{
									IEnumerable<Thing> enumerable3 = enumerable;
									if (enumerable3 == null)
									{
										enumerable3 = <>c__DisplayClass3_2.CS$<>8__locals1.pawn.Map.listerThings.ThingsMatching(<>c__DisplayClass3_2.scanner.PotentialWorkThingRequest);
									}
									thing = GenClosest.ClosestThing_Global(<>c__DisplayClass3_2.CS$<>8__locals1.pawn.Position, enumerable3, 99999f, validator, null);
								}
								else
								{
									thing = GenClosest.ClosestThingReachable(<>c__DisplayClass3_2.CS$<>8__locals1.pawn.Position, <>c__DisplayClass3_2.CS$<>8__locals1.pawn.Map, <>c__DisplayClass3_2.scanner.PotentialWorkThingRequest, <>c__DisplayClass3_2.scanner.PathEndMode, TraverseParms.For(<>c__DisplayClass3_2.CS$<>8__locals1.pawn, <>c__DisplayClass3_2.scanner.MaxPathDanger(<>c__DisplayClass3_2.CS$<>8__locals1.pawn), TraverseMode.ByPawn, false), 9999f, validator, enumerable, 0, <>c__DisplayClass3_2.scanner.MaxRegionsToScanBeforeGlobalSearch, enumerable != null, RegionType.Set_Passable, false);
								}
								if (thing != null)
								{
									<>c__DisplayClass3_2.CS$<>8__locals1.bestTargetOfLastPriority = thing;
									<>c__DisplayClass3_2.CS$<>8__locals1.scannerWhoProvidedTarget = <>c__DisplayClass3_2.scanner;
								}
							}
							if (<>c__DisplayClass3_2.scanner.def.scanCells)
							{
								JobGiver_Work.<>c__DisplayClass3_2 <>c__DisplayClass3_3;
								<>c__DisplayClass3_3.pawnPosition = <>c__DisplayClass3_2.CS$<>8__locals1.pawn.Position;
								<>c__DisplayClass3_3.closestDistSquared = 99999f;
								<>c__DisplayClass3_3.bestPriority = float.MinValue;
								<>c__DisplayClass3_3.prioritized = <>c__DisplayClass3_2.scanner.Prioritized;
								<>c__DisplayClass3_3.allowUnreachable = <>c__DisplayClass3_2.scanner.AllowUnreachable;
								<>c__DisplayClass3_3.maxPathDanger = <>c__DisplayClass3_2.scanner.MaxPathDanger(<>c__DisplayClass3_2.CS$<>8__locals1.pawn);
								IEnumerable<IntVec3> enumerable4 = <>c__DisplayClass3_2.scanner.PotentialWorkCellsGlobal(<>c__DisplayClass3_2.CS$<>8__locals1.pawn);
								IList<IntVec3> list2;
								if ((list2 = (enumerable4 as IList<IntVec3>)) != null)
								{
									for (int k = 0; k < list2.Count; k++)
									{
										<>c__DisplayClass3_2.<TryIssueJobPackage>g__ProcessCell|3(list2[k], ref <>c__DisplayClass3_3);
									}
								}
								else
								{
									foreach (IntVec3 c in enumerable4)
									{
										<>c__DisplayClass3_2.<TryIssueJobPackage>g__ProcessCell|3(c, ref <>c__DisplayClass3_3);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							<>c__DisplayClass3_.pawn,
							" threw exception in WorkGiver ",
							workGiver.def.defName,
							": ",
							ex.ToString()
						}), false);
					}
					finally
					{
					}
					if (<>c__DisplayClass3_.bestTargetOfLastPriority.IsValid)
					{
						Job job3;
						if (<>c__DisplayClass3_.bestTargetOfLastPriority.HasThing)
						{
							job3 = <>c__DisplayClass3_.scannerWhoProvidedTarget.JobOnThing(<>c__DisplayClass3_.pawn, <>c__DisplayClass3_.bestTargetOfLastPriority.Thing, false);
						}
						else
						{
							job3 = <>c__DisplayClass3_.scannerWhoProvidedTarget.JobOnCell(<>c__DisplayClass3_.pawn, <>c__DisplayClass3_.bestTargetOfLastPriority.Cell, false);
						}
						if (job3 != null)
						{
							job3.workGiverDef = <>c__DisplayClass3_.scannerWhoProvidedTarget.def;
							return new ThinkResult(job3, this, new JobTag?(list[j].def.tagToGive), false);
						}
						Log.ErrorOnce(string.Concat(new object[]
						{
							<>c__DisplayClass3_.scannerWhoProvidedTarget,
							" provided target ",
							<>c__DisplayClass3_.bestTargetOfLastPriority,
							" but yielded no actual job for pawn ",
							<>c__DisplayClass3_.pawn,
							". The CanGiveJob and JobOnX methods may not be synchronized."
						}), 6112651, false);
					}
					num = workGiver.def.priorityInType;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x0010D520 File Offset: 0x0010B720
		private bool PawnCanUseWorkGiver(Pawn pawn, WorkGiver giver)
		{
			return (giver.def.nonColonistsCanDo || pawn.IsColonist) && !pawn.WorkTagIsDisabled(giver.def.workTags) && !giver.ShouldSkip(pawn, false) && giver.MissingRequiredCapacity(pawn) == null;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x0010D571 File Offset: 0x0010B771
		private bool WorkGiversRelated(WorkGiverDef current, WorkGiverDef next)
		{
			return next != WorkGiverDefOf.Repair || current == WorkGiverDefOf.Repair;
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x0010D588 File Offset: 0x0010B788
		private Job GiverTryGiveJobPrioritized(Pawn pawn, WorkGiver giver, IntVec3 cell)
		{
			if (!this.PawnCanUseWorkGiver(pawn, giver))
			{
				return null;
			}
			try
			{
				Job job = giver.NonScanJob(pawn);
				if (job != null)
				{
					return job;
				}
				WorkGiver_Scanner scanner = giver as WorkGiver_Scanner;
				if (scanner != null)
				{
					if (giver.def.scanThings)
					{
						Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false);
						List<Thing> thingList = cell.GetThingList(pawn.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (scanner.PotentialWorkThingRequest.Accepts(thing) && predicate(thing))
							{
								Job job2 = scanner.JobOnThing(pawn, thing, false);
								if (job2 != null)
								{
									job2.workGiverDef = giver.def;
								}
								return job2;
							}
						}
					}
					if (giver.def.scanCells && !cell.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, cell, false))
					{
						Job job3 = scanner.JobOnCell(pawn, cell, false);
						if (job3 != null)
						{
							job3.workGiverDef = giver.def;
						}
						return job3;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					pawn,
					" threw exception in GiverTryGiveJobTargeted on WorkGiver ",
					giver.def.defName,
					": ",
					ex.ToString()
				}), false);
			}
			return null;
		}

		// Token: 0x04001AD7 RID: 6871
		public bool emergency;
	}
}
