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
	
	public class Building_Bed : Building
	{
		
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x00199D03 File Offset: 0x00197F03
		public List<Pawn> OwnersForReading
		{
			get
			{
				return this.CompAssignableToPawn.AssignedPawnsForReading;
			}
		}

		
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x00199D10 File Offset: 0x00197F10
		public CompAssignableToPawn CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn>();
			}
		}

		
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

		
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x00199EE5 File Offset: 0x001980E5
		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(this.def.size);
			}
		}

		
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x00199EF7 File Offset: 0x001980F7
		private bool PlayerCanSeeOwners
		{
			get
			{
				return this.CompAssignableToPawn.PlayerCanSeeAssignments;
			}
		}

		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forPrisonersInt, "forPrisoners", false, false);
			Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
			Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
		}

		
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null && Building_Bed.RoomCanBePrisonCell(room))
			{
				room.DrawFieldEdges();
			}
		}

		
		public static bool RoomCanBePrisonCell(Room r)
		{
			return !r.TouchesMapEdge && !r.IsHuge && r.RegionType == RegionType.Normal;
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

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

		
		public IntVec3 GetSleepingSlotPos(int index)
		{
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		
		private void RemoveAllOwners()
		{
			for (int i = this.OwnersForReading.Count - 1; i >= 0; i--)
			{
				this.OwnersForReading[i].ownership.UnclaimBed();
			}
		}

		
		private void NotifyRoomBedTypeChanged()
		{
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		
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

		
		private bool forPrisonersInt;

		
		private bool medicalInt;

		
		private bool alreadySetDefaultMed;

		
		private static int lastPrisonerSetChangeFrame = -1;

		
		private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

		
		private static readonly Color SheetColorRoyal = new Color(0.670588255f, 0.9137255f, 0.745098054f);

		
		public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.129411772f);

		
		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.623529434f, 0.8862745f);

		
		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.152941182f);
	}
}
