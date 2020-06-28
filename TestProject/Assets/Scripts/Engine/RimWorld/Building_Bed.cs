using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C72 RID: 3186
	public class Building_Bed : Building
	{
		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x00199D03 File Offset: 0x00197F03
		public List<Pawn> OwnersForReading
		{
			get
			{
				return this.CompAssignableToPawn.AssignedPawnsForReading;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x00199D10 File Offset: 0x00197F10
		public CompAssignableToPawn CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn>();
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x00199D18 File Offset: 0x00197F18
		// (set) Token: 0x06004C48 RID: 19528 RVA: 0x00199D20 File Offset: 0x00197F20
		public bool ForPrisoners
		{
			get
			{
				return this.forPrisonersInt;
			}
			set
			{
				if (value == this.forPrisonersInt || !this.def.building.bed_humanlike)
				{
					return;
				}
				if (Current.ProgramState != ProgramState.Playing && Scribe.mode != LoadSaveMode.Inactive)
				{
					Log.Error("Tried to set ForPrisoners while game mode was " + Current.ProgramState, false);
					return;
				}
				this.RemoveAllOwners();
				this.forPrisonersInt = value;
				this.Notify_ColorChanged();
				this.NotifyRoomBedTypeChanged();
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06004C49 RID: 19529 RVA: 0x00199D8C File Offset: 0x00197F8C
		// (set) Token: 0x06004C4A RID: 19530 RVA: 0x00199D94 File Offset: 0x00197F94
		public bool Medical
		{
			get
			{
				return this.medicalInt;
			}
			set
			{
				if (value == this.medicalInt || !this.def.building.bed_humanlike)
				{
					return;
				}
				this.RemoveAllOwners();
				this.medicalInt = value;
				this.Notify_ColorChanged();
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
					this.NotifyRoomBedTypeChanged();
				}
				this.FacilityChanged();
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06004C4B RID: 19531 RVA: 0x00199DFB File Offset: 0x00197FFB
		public bool AnyUnownedSleepingSlot
		{
			get
			{
				if (this.Medical)
				{
					Log.Warning("Tried to check for unowned sleeping slot on medical bed " + this, false);
					return false;
				}
				return this.CompAssignableToPawn.HasFreeSlot;
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x00199E24 File Offset: 0x00198024
		public bool AnyUnoccupiedSleepingSlot
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					if (this.GetCurOccupant(i) == null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x00199E4E File Offset: 0x0019804E
		public IEnumerable<Pawn> CurOccupants
		{
			get
			{
				int num;
				for (int i = 0; i < this.SleepingSlotsCount; i = num + 1)
				{
					Pawn curOccupant = this.GetCurOccupant(i);
					if (curOccupant != null)
					{
						yield return curOccupant;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x00199E5E File Offset: 0x0019805E
		public override Color DrawColor
		{
			get
			{
				if (this.def.MadeFromStuff)
				{
					return base.DrawColor;
				}
				return this.DrawColorTwo;
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06004C4F RID: 19535 RVA: 0x00199E7C File Offset: 0x0019807C
		public override Color DrawColorTwo
		{
			get
			{
				if (!this.def.building.bed_humanlike)
				{
					return base.DrawColorTwo;
				}
				bool forPrisoners = this.ForPrisoners;
				bool medical = this.Medical;
				if (forPrisoners && medical)
				{
					return Building_Bed.SheetColorMedicalForPrisoner;
				}
				if (forPrisoners)
				{
					return Building_Bed.SheetColorForPrisoner;
				}
				if (medical)
				{
					return Building_Bed.SheetColorMedical;
				}
				if (this.def == ThingDefOf.RoyalBed)
				{
					return Building_Bed.SheetColorRoyal;
				}
				return Building_Bed.SheetColorNormal;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x00199EE5 File Offset: 0x001980E5
		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(this.def.size);
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x00199EF7 File Offset: 0x001980F7
		private bool PlayerCanSeeOwners
		{
			get
			{
				return this.CompAssignableToPawn.PlayerCanSeeAssignments;
			}
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x00199F04 File Offset: 0x00198104
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(base.Position);
			if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.Room.isPrisonCell)
			{
				this.ForPrisoners = true;
			}
			if (!this.alreadySetDefaultMed)
			{
				this.alreadySetDefaultMed = true;
				if (this.def.building.bed_defaultMedical)
				{
					this.Medical = true;
				}
			}
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x00199F6C File Offset: 0x0019816C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			this.RemoveAllOwners();
			this.ForPrisoners = false;
			this.Medical = false;
			this.alreadySetDefaultMed = false;
			Room room = this.GetRoom(RegionType.Set_Passable);
			base.DeSpawn(mode);
			if (room != null)
			{
				room.Notify_RoomShapeOrContainedBedsChanged();
			}
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x00199FAC File Offset: 0x001981AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forPrisonersInt, "forPrisoners", false, false);
			Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
			Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x00199FEC File Offset: 0x001981EC
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null && Building_Bed.RoomCanBePrisonCell(room))
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x0019A018 File Offset: 0x00198218
		public static bool RoomCanBePrisonCell(Room r)
		{
			return !r.TouchesMapEdge && !r.IsHuge && r.RegionType == RegionType.Normal;
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x0019A035 File Offset: 0x00198235
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.def.building.bed_humanlike && base.Faction == Faction.OfPlayer)
			{
				Command_Toggle command_Toggle = new Command_Toggle();
				command_Toggle.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
				command_Toggle.defaultDesc = "CommandBedSetForPrisonersDesc".Translate();
				command_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
				command_Toggle.isActive = (() => this.ForPrisoners);
				command_Toggle.toggleAction = delegate
				{
					this.ToggleForPrisonersByInterface();
				};
				if (!Building_Bed.RoomCanBePrisonCell(this.GetRoom(RegionType.Set_Passable)) && !this.ForPrisoners)
				{
					command_Toggle.Disable("CommandBedSetForPrisonersFailOutdoors".Translate());
				}
				command_Toggle.hotKey = KeyBindingDefOf.Misc3;
				command_Toggle.turnOffSound = null;
				command_Toggle.turnOnSound = null;
				yield return command_Toggle;
				yield return new Command_Toggle
				{
					defaultLabel = "CommandBedSetAsMedicalLabel".Translate(),
					defaultDesc = "CommandBedSetAsMedicalDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/AsMedical", true),
					isActive = (() => this.Medical),
					toggleAction = delegate
					{
						this.Medical = !this.Medical;
					},
					hotKey = KeyBindingDefOf.Misc2
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x0019A048 File Offset: 0x00198248
		private void ToggleForPrisonersByInterface()
		{
			if (Building_Bed.lastPrisonerSetChangeFrame == Time.frameCount)
			{
				return;
			}
			Building_Bed.lastPrisonerSetChangeFrame = Time.frameCount;
			bool newForPrisoners = !this.ForPrisoners;
			(newForPrisoners ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff).PlayOneShotOnCamera(null);
			List<Building_Bed> bedsToAffect = new List<Building_Bed>();
			foreach (Building_Bed building_Bed in Find.Selector.SelectedObjects.OfType<Building_Bed>())
			{
				if (building_Bed.ForPrisoners != newForPrisoners)
				{
					Room room = building_Bed.GetRoom(RegionType.Set_Passable);
					if (room == null || !Building_Bed.RoomCanBePrisonCell(room))
					{
						if (!bedsToAffect.Contains(building_Bed))
						{
							bedsToAffect.Add(building_Bed);
						}
					}
					else
					{
						foreach (Building_Bed item in room.ContainedBeds)
						{
							if (!bedsToAffect.Contains(item))
							{
								bedsToAffect.Add(item);
							}
						}
					}
				}
			}
			Action action = delegate
			{
				List<Room> list = new List<Room>();
				foreach (Building_Bed building_Bed3 in bedsToAffect)
				{
					Room room2 = building_Bed3.GetRoom(RegionType.Set_Passable);
					building_Bed3.ForPrisoners = (newForPrisoners && !room2.TouchesMapEdge);
					for (int j = 0; j < this.SleepingSlotsCount; j++)
					{
						Pawn curOccupant = this.GetCurOccupant(j);
						if (curOccupant != null)
						{
							curOccupant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					}
					if (!list.Contains(room2) && !room2.TouchesMapEdge)
					{
						list.Add(room2);
					}
				}
				foreach (Room room3 in list)
				{
					room3.Notify_RoomShapeOrContainedBedsChanged();
				}
			};
			if ((from b in bedsToAffect
			where b.OwnersForReading.Any<Pawn>() && b != this
			select b).Count<Building_Bed>() == 0)
			{
				action();
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (newForPrisoners)
			{
				stringBuilder.Append("TurningOnPrisonerBedWarning".Translate());
			}
			else
			{
				stringBuilder.Append("TurningOffPrisonerBedWarning".Translate());
			}
			stringBuilder.AppendLine();
			foreach (Building_Bed building_Bed2 in bedsToAffect)
			{
				if ((newForPrisoners && !building_Bed2.ForPrisoners) || (!newForPrisoners && building_Bed2.ForPrisoners))
				{
					for (int i = 0; i < building_Bed2.OwnersForReading.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(building_Bed2.OwnersForReading[i].LabelShort);
					}
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("AreYouSure".Translate());
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), action, false, null));
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x0019A2E8 File Offset: 0x001984E8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.def.building.bed_humanlike)
			{
				if (this.ForPrisoners)
				{
					stringBuilder.AppendInNewLine("ForPrisonerUse".Translate());
				}
				else if (this.PlayerCanSeeOwners)
				{
					stringBuilder.AppendInNewLine("ForColonistUse".Translate());
				}
				if (this.Medical)
				{
					stringBuilder.AppendInNewLine("MedicalBed".Translate());
					if (base.Spawned)
					{
						stringBuilder.AppendInNewLine("RoomInfectionChanceFactor".Translate() + ": " + this.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.InfectionChanceFactor).ToStringPercent());
					}
				}
				else if (this.PlayerCanSeeOwners)
				{
					if (this.OwnersForReading.Count == 0)
					{
						stringBuilder.AppendInNewLine("Owner".Translate() + ": " + "Nobody".Translate());
					}
					else if (this.OwnersForReading.Count == 1)
					{
						stringBuilder.AppendInNewLine("Owner".Translate() + ": " + this.OwnersForReading[0].Label);
					}
					else
					{
						stringBuilder.AppendInNewLine("Owners".Translate() + ": ");
						bool flag = false;
						for (int i = 0; i < this.OwnersForReading.Count; i++)
						{
							if (flag)
							{
								stringBuilder.Append(", ");
							}
							flag = true;
							stringBuilder.Append(this.OwnersForReading[i].LabelShort);
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x0019A4B6 File Offset: 0x001986B6
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			if (myPawn.RaceProps.Humanlike && !this.ForPrisoners && this.Medical && !myPawn.Drafted && base.Faction == Faction.OfPlayer && RestUtility.CanUseBedEver(myPawn, this.def))
			{
				if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(myPawn))
				{
					yield return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					Action action = delegate
					{
						if (!this.ForPrisoners && this.Medical && myPawn.CanReserveAndReach(this, PathEndMode.ClosestTouch, Danger.Deadly, this.SleepingSlotsCount, -1, null, true))
						{
							if (myPawn.CurJobDef == JobDefOf.LayDown && myPawn.CurJob.GetTarget(TargetIndex.A).Thing == this)
							{
								myPawn.CurJob.restUntilHealed = true;
							}
							else
							{
								Job job = JobMaker.MakeJob(JobDefOf.LayDown, this);
								job.restUntilHealed = true;
								myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}
							myPawn.mindState.ResetLastDisturbanceTick();
						}
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), action, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, (this.AnyUnoccupiedSleepingSlot ? "ReservedBy" : "SomeoneElseSleeping").CapitalizeFirst());
				}
			}
			yield break;
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x0019A4D0 File Offset: 0x001986D0
		public override void DrawGUIOverlay()
		{
			if (this.Medical)
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeOwners)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.OwnersForReading.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this, "Unowned".Translate(), defaultThingLabelColor);
					return;
				}
				if (this.OwnersForReading.Count == 1)
				{
					if (this.OwnersForReading[0].InBed() && this.OwnersForReading[0].CurrentBed() == this)
					{
						return;
					}
					GenMapUI.DrawThingLabel(this, this.OwnersForReading[0].LabelShort, defaultThingLabelColor);
					return;
				}
				else
				{
					for (int i = 0; i < this.OwnersForReading.Count; i++)
					{
						if (!this.OwnersForReading[i].InBed() || this.OwnersForReading[i].CurrentBed() != this || !(this.OwnersForReading[i].Position == this.GetSleepingSlotPos(i)))
						{
							GenMapUI.DrawThingLabel(this.GetMultiOwnersLabelScreenPosFor(i), this.OwnersForReading[i].LabelShort, defaultThingLabelColor);
						}
					}
				}
			}
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x0019A5F8 File Offset: 0x001987F8
		public Pawn GetCurOccupant(int slotIndex)
		{
			if (!base.Spawned)
			{
				return null;
			}
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			List<Thing> list = base.Map.thingGrid.ThingsListAt(sleepingSlotPos);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.CurJob != null && pawn.GetPosture() == PawnPosture.LayingInBed)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x0019A660 File Offset: 0x00198860
		public int GetCurOccupantSlotIndex(Pawn curOccupant)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetCurOccupant(i) == curOccupant)
				{
					return i;
				}
			}
			Log.Error("Could not find pawn " + curOccupant + " on any of sleeping slots.", false);
			return 0;
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x0019A6A4 File Offset: 0x001988A4
		public Pawn GetCurOccupantAt(IntVec3 pos)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetSleepingSlotPos(i) == pos)
				{
					return this.GetCurOccupant(i);
				}
			}
			return null;
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x0019A6DA File Offset: 0x001988DA
		public IntVec3 GetSleepingSlotPos(int index)
		{
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x0019A6FC File Offset: 0x001988FC
		private void RemoveAllOwners()
		{
			for (int i = this.OwnersForReading.Count - 1; i >= 0; i--)
			{
				this.OwnersForReading[i].ownership.UnclaimBed();
			}
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x0019A738 File Offset: 0x00198938
		private void NotifyRoomBedTypeChanged()
		{
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x0019A758 File Offset: 0x00198958
		private void FacilityChanged()
		{
			CompFacility compFacility = this.TryGetComp<CompFacility>();
			CompAffectedByFacilities compAffectedByFacilities = this.TryGetComp<CompAffectedByFacilities>();
			if (compFacility != null)
			{
				compFacility.Notify_ThingChanged();
			}
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.Notify_ThingChanged();
			}
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x0019A788 File Offset: 0x00198988
		private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
		{
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			Vector3 drawPos = this.DrawPos;
			if (base.Rotation.IsHorizontal)
			{
				drawPos.z = (float)sleepingSlotPos.z + 0.6f;
			}
			else
			{
				drawPos.x = (float)sleepingSlotPos.x + 0.5f;
				drawPos.z += -0.4f;
			}
			Vector2 v = drawPos.MapToUIPosition();
			if (!base.Rotation.IsHorizontal && this.SleepingSlotsCount == 2)
			{
				v = this.AdjustOwnerLabelPosToAvoidOverlapping(v, slotIndex);
			}
			return v;
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x0019A828 File Offset: 0x00198A28
		private Vector3 AdjustOwnerLabelPosToAvoidOverlapping(Vector3 screenPos, int slotIndex)
		{
			Text.Font = GameFont.Tiny;
			float num = Text.CalcSize(this.OwnersForReading[slotIndex].LabelShort).x + 1f;
			Vector2 vector = this.DrawPos.MapToUIPosition();
			float num2 = Mathf.Abs(screenPos.x - vector.x);
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			if (num > num2 * 2f)
			{
				float num3;
				if (slotIndex == 0)
				{
					num3 = (float)this.GetSleepingSlotPos(1).x;
				}
				else
				{
					num3 = (float)this.GetSleepingSlotPos(0).x;
				}
				if ((float)sleepingSlotPos.x < num3)
				{
					screenPos.x -= (num - num2 * 2f) / 2f;
				}
				else
				{
					screenPos.x += (num - num2 * 2f) / 2f;
				}
			}
			return screenPos;
		}

		// Token: 0x04002AF8 RID: 11000
		private bool forPrisonersInt;

		// Token: 0x04002AF9 RID: 11001
		private bool medicalInt;

		// Token: 0x04002AFA RID: 11002
		private bool alreadySetDefaultMed;

		// Token: 0x04002AFB RID: 11003
		private static int lastPrisonerSetChangeFrame = -1;

		// Token: 0x04002AFC RID: 11004
		private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

		// Token: 0x04002AFD RID: 11005
		private static readonly Color SheetColorRoyal = new Color(0.670588255f, 0.9137255f, 0.745098054f);

		// Token: 0x04002AFE RID: 11006
		public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.129411772f);

		// Token: 0x04002AFF RID: 11007
		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.623529434f, 0.8862745f);

		// Token: 0x04002B00 RID: 11008
		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.152941182f);
	}
}
