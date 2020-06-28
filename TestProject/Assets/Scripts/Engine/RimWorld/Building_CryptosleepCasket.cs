using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C78 RID: 3192
	public class Building_CryptosleepCasket : Building_Casket
	{
		// Token: 0x06004C8E RID: 19598 RVA: 0x0019B0E9 File Offset: 0x001992E9
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x0019B11C File Offset: 0x0019931C
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			if (myPawn.IsQuestLodger())
			{
				FloatMenuOption floatMenuOption = new FloatMenuOption("CannotUseReason".Translate("CryptosleepCasketGuestsNotAllowed".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return floatMenuOption;
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption2 in this.<>n__0(myPawn))
			{
				yield return floatMenuOption2;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.innerContainer.Count == 0)
			{
				if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					FloatMenuOption floatMenuOption3 = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					yield return floatMenuOption3;
				}
				else
				{
					JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
					string label = "EnterCryptosleepCasket".Translate();
					Action action = delegate
					{
						Job job = JobMaker.MakeJob(jobDef, this);
						myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, "ReservedBy");
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x0019B133 File Offset: 0x00199333
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__1())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.Faction == Faction.OfPlayer && this.innerContainer.Count > 0 && this.def.building.isPlayerEjectable)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.action = new Action(this.EjectContents);
				command_Action.defaultLabel = "CommandPodEject".Translate();
				command_Action.defaultDesc = "CommandPodEjectDesc".Translate();
				if (this.innerContainer.Count == 0)
				{
					command_Action.Disable("CommandPodEjectFailEmpty".Translate());
				}
				command_Action.hotKey = KeyBindingDefOf.Misc8;
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x0019B144 File Offset: 0x00199344
		public override void EjectContents()
		{
			ThingDef filth_Slime = ThingDefOf.Filth_Slime;
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filth_Slime);
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null, null);
					}
				}
			}
			if (!base.Destroyed)
			{
				SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x0019B204 File Offset: 0x00199404
		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
		{
			Predicate<Thing> <>9__1;
			foreach (ThingDef singleDef in from def in DefDatabase<ThingDef>.AllDefs
			where typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass)
			select def)
			{
				IntVec3 position = p.Position;
				Map map = p.Map;
				ThingRequest thingReq = ThingRequest.ForDef(singleDef);
				PathEndMode peMode = PathEndMode.InteractionCell;
				TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false);
				float maxDistance = 9999f;
				Predicate<Thing> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((Thing x) => !((Building_CryptosleepCasket)x).HasAnyContents && traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations)));
				}
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}
	}
}
