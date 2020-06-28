using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000669 RID: 1641
	public class JobDriver_ManTurret : JobDriver
	{
		// Token: 0x06002CC3 RID: 11459 RVA: 0x000FE6E0 File Offset: 0x000FC8E0
		private static bool GunNeedsLoading(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			CompChangeableProjectile compChangeableProjectile = building_TurretGun.gun.TryGetComp<CompChangeableProjectile>();
			return compChangeableProjectile != null && !compChangeableProjectile.Loaded;
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000FE714 File Offset: 0x000FC914
		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			StorageSettings allowedShellsSettings = pawn.IsColonist ? gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings : null;
			Predicate<Thing> validator = (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 10, 1, null, false) && (allowedShellsSettings == null || allowedShellsSettings.AllowedToAccept(t));
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000FE794 File Offset: 0x000FC994
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil loadIfNeeded = new Toil();
			loadIfNeeded.initAction = delegate
			{
				Pawn actor = loadIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(gotoTurret);
					return;
				}
				Thing thing = JobDriver_ManTurret.FindAmmoForTurret(this.pawn, building_TurretGun);
				if (thing == null)
				{
					if (actor.Faction == Faction.OfPlayer)
					{
						Messages.Message("MessageOutOfNearbyShellsFor".Translate(actor.LabelShort, building_TurretGun.Label, actor.Named("PAWN"), building_TurretGun.Named("GUN")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
					}
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				actor.CurJob.targetB = thing;
				actor.CurJob.count = 1;
			};
			yield return loadIfNeeded;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate
				{
					Pawn actor = loadIfNeeded.actor;
					Building_TurretGun building_TurretGun = ((Building)actor.CurJob.targetA.Thing) as Building_TurretGun;
					SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
					building_TurretGun.gun.TryGetComp<CompChangeableProjectile>().LoadShell(actor.CurJob.targetB.Thing.def, 1);
					actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
			};
			yield return gotoTurret;
			Toil man = new Toil();
			man.tickAction = delegate
			{
				Pawn actor = man.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				if (JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(loadIfNeeded);
					return;
				}
				building.GetComp<CompMannable>().ManForATick(actor);
				man.actor.rotationTracker.FaceCell(building.Position);
			};
			man.handlingFacing = true;
			man.defaultCompleteMode = ToilCompleteMode.Never;
			man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return man;
			yield break;
		}

		// Token: 0x040019F5 RID: 6645
		private const float ShellSearchRadius = 40f;

		// Token: 0x040019F6 RID: 6646
		private const int MaxPawnAmmoReservations = 10;
	}
}
