﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200065F RID: 1631
	public class JobDriver_HaulCorpseToPublicPlace : JobDriver
	{
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x000FD7F0 File Offset: 0x000FB9F0
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x000FD818 File Offset: 0x000FBA18
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x000FD83E File Offset: 0x000FBA3E
		private bool InGrave
		{
			get
			{
				return this.Grave != null;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002C76 RID: 11382 RVA: 0x000FD849 File Offset: 0x000FBA49
		private Thing Target
		{
			get
			{
				return this.Grave ?? this.Corpse;
			}
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x000FD85B File Offset: 0x000FBA5B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000FD87D File Offset: 0x000FBA7D
		public override string GetReport()
		{
			if (this.InGrave && this.Grave.def == ThingDefOf.Grave)
			{
				return "ReportDiggingUpCorpse".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000FD8AF File Offset: 0x000FBAAF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, gotoCorpse);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_General.Wait(300, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			yield return Toils_General.Open(TargetIndex.B);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return gotoCorpse;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return this.FindCellToDropCorpseToil();
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.Touch);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return this.ForbidAndNotifyMentalStateToil();
			yield break;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000FD8BF File Offset: 0x000FBABF
		private Toil FindCellToDropCorpseToil()
		{
			return new Toil
			{
				initAction = delegate
				{
					IntVec3 c = IntVec3.Invalid;
					if (!Rand.Chance(0.8f) || !this.TryFindTableCell(out c))
					{
						bool flag = false;
						IntVec3 root;
						if (RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawn, out root) && CellFinder.TryRandomClosewalkCellNear(root, this.pawn.Map, 5, out c, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null))
						{
							flag = true;
						}
						if (!flag)
						{
							c = CellFinder.RandomClosewalkCellNear(this.pawn.Position, this.pawn.Map, 10, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null);
						}
					}
					this.job.SetTarget(TargetIndex.C, c);
				},
				atomicWithPrevious = true
			};
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000FD8DF File Offset: 0x000FBADF
		private Toil ForbidAndNotifyMentalStateToil()
		{
			return new Toil
			{
				initAction = delegate
				{
					Corpse corpse = this.Corpse;
					if (corpse != null)
					{
						corpse.SetForbidden(true, true);
					}
					MentalState_CorpseObsession mentalState_CorpseObsession = this.pawn.MentalState as MentalState_CorpseObsession;
					if (mentalState_CorpseObsession != null)
					{
						mentalState_CorpseObsession.Notify_CorpseHauled();
					}
				},
				atomicWithPrevious = true
			};
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x000FD900 File Offset: 0x000FBB00
		private bool TryFindTableCell(out IntVec3 cell)
		{
			JobDriver_HaulCorpseToPublicPlace.tmpCells.Clear();
			List<Building> allBuildingsColonist = this.pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.IsTable)
				{
					foreach (IntVec3 intVec in building.OccupiedRect())
					{
						if (this.pawn.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) && intVec.GetFirstItem(this.pawn.Map) == null)
						{
							JobDriver_HaulCorpseToPublicPlace.tmpCells.Add(intVec);
						}
					}
				}
			}
			if (!JobDriver_HaulCorpseToPublicPlace.tmpCells.Any<IntVec3>())
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = JobDriver_HaulCorpseToPublicPlace.tmpCells.RandomElement<IntVec3>();
			return true;
		}

		// Token: 0x040019E0 RID: 6624
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x040019E1 RID: 6625
		private const TargetIndex GraveInd = TargetIndex.B;

		// Token: 0x040019E2 RID: 6626
		private const TargetIndex CellInd = TargetIndex.C;

		// Token: 0x040019E3 RID: 6627
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
