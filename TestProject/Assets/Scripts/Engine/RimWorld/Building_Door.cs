﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	
	public class Building_Door : Building
	{
		
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x0019B2F9 File Offset: 0x001994F9
		public bool Open
		{
			get
			{
				return this.openInt;
			}
		}

		
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x0019B301 File Offset: 0x00199501
		public bool HoldOpen
		{
			get
			{
				return this.holdOpenInt;
			}
		}

		
		// (get) Token: 0x06004C98 RID: 19608 RVA: 0x0019B309 File Offset: 0x00199509
		public bool FreePassage
		{
			get
			{
				return this.openInt && (this.holdOpenInt || !this.WillCloseSoon);
			}
		}

		
		// (get) Token: 0x06004C99 RID: 19609 RVA: 0x0019B328 File Offset: 0x00199528
		public int TicksTillFullyOpened
		{
			get
			{
				int num = this.TicksToOpenNow - this.ticksSinceOpen;
				if (num < 0)
				{
					num = 0;
				}
				return num;
			}
		}

		
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x0019B34C File Offset: 0x0019954C
		public bool WillCloseSoon
		{
			get
			{
				if (!base.Spawned)
				{
					return true;
				}
				if (!this.openInt)
				{
					return true;
				}
				if (this.holdOpenInt)
				{
					return false;
				}
				if (this.ticksUntilClose > 0 && this.ticksUntilClose <= 111 && !this.BlockedOpenMomentary)
				{
					return true;
				}
				if (this.CanTryCloseAutomatically && !this.BlockedOpenMomentary)
				{
					return true;
				}
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c = base.Position + GenAdj.CardinalDirectionsAndInside[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							Pawn pawn = thingList[j] as Pawn;
							if (pawn != null && !pawn.HostileTo(this) && !pawn.Downed && (pawn.Position == base.Position || (pawn.pather.Moving && pawn.pather.nextCell == base.Position)))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x06004C9B RID: 19611 RVA: 0x0019B464 File Offset: 0x00199664
		public bool BlockedOpenMomentary
		{
			get
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x0019B4BB File Offset: 0x001996BB
		public bool DoorPowerOn
		{
			get
			{
				return this.powerComp != null && this.powerComp.PowerOn;
			}
		}

		
		// (get) Token: 0x06004C9D RID: 19613 RVA: 0x0019B4D2 File Offset: 0x001996D2
		public bool SlowsPawns
		{
			get
			{
				return !this.DoorPowerOn || this.TicksToOpenNow > 20;
			}
		}

		
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x0019B4E8 File Offset: 0x001996E8
		public int TicksToOpenNow
		{
			get
			{
				float num = 45f / this.GetStatValue(StatDefOf.DoorOpenSpeed, true);
				if (this.DoorPowerOn)
				{
					num *= 0.25f;
				}
				return Mathf.RoundToInt(num);
			}
		}

		
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x0019B51E File Offset: 0x0019971E
		private bool CanTryCloseAutomatically
		{
			get
			{
				return this.FriendlyTouchedRecently && !this.HoldOpen;
			}
		}

		
		// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x0019B533 File Offset: 0x00199733
		private bool FriendlyTouchedRecently
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastFriendlyTouchTick + 120;
			}
		}

		
		// (get) Token: 0x06004CA1 RID: 19617 RVA: 0x0019B54A File Offset: 0x0019974A
		public override bool FireBulwark
		{
			get
			{
				return !this.Open && base.FireBulwark;
			}
		}

		
		public override void PostMake()
		{
			base.PostMake();
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.ClearReachabilityCache(map);
			if (this.BlockedOpenMomentary)
			{
				this.DoorOpen(110);
			}
		}

		
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			this.ClearReachabilityCache(map);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openInt, "open", false, false);
			Scribe_Values.Look<bool>(ref this.holdOpenInt, "holdOpen", false, false);
			Scribe_Values.Look<int>(ref this.lastFriendlyTouchTick, "lastFriendlyTouchTick", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.openInt)
			{
				this.ticksSinceOpen = this.TicksToOpenNow;
			}
		}

		
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				this.ClearReachabilityCache(base.Map);
			}
		}

		
		public override void Tick()
		{
			base.Tick();
			if (this.FreePassage != this.freePassageWhenClearedReachabilityCache)
			{
				this.ClearReachabilityCache(base.Map);
			}
			if (!this.openInt)
			{
				if (this.ticksSinceOpen > 0)
				{
					this.ticksSinceOpen--;
				}
				if ((Find.TickManager.TicksGame + this.thingIDNumber.HashOffset()) % 375 == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, 1f, false);
					return;
				}
			}
			else if (this.openInt)
			{
				if (this.ticksSinceOpen < this.TicksToOpenNow)
				{
					this.ticksSinceOpen++;
				}
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null)
					{
						this.CheckFriendlyTouched(pawn);
					}
				}
				if (this.ticksUntilClose > 0)
				{
					if (base.Map.thingGrid.CellContains(base.Position, ThingCategory.Pawn))
					{
						this.ticksUntilClose = 110;
					}
					this.ticksUntilClose--;
					if (this.ticksUntilClose <= 0 && !this.holdOpenInt && !this.DoorTryClose())
					{
						this.ticksUntilClose = 1;
					}
				}
				else if (this.CanTryCloseAutomatically)
				{
					this.ticksUntilClose = 110;
				}
				if ((Find.TickManager.TicksGame + this.thingIDNumber.HashOffset()) % 34 == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, 1f, false);
				}
			}
		}

		
		public void CheckFriendlyTouched(Pawn p)
		{
			if (!p.HostileTo(this) && this.PawnCanOpen(p))
			{
				this.lastFriendlyTouchTick = Find.TickManager.TicksGame;
			}
		}

		
		public void Notify_PawnApproaching(Pawn p, int moveCost)
		{
			this.CheckFriendlyTouched(p);
			bool flag = this.PawnCanOpen(p);
			if (flag || this.Open)
			{
				base.Map.fogGrid.Notify_PawnEnteringDoor(this, p);
			}
			if (flag && !this.SlowsPawns)
			{
				int ticksToClose = Mathf.Max(300, moveCost + 1);
				this.DoorOpen(ticksToClose);
			}
		}

		
		public bool CanPhysicallyPass(Pawn p)
		{
			return this.FreePassage || this.PawnCanOpen(p) || (this.Open && p.HostileTo(this));
		}

		
		public virtual bool PawnCanOpen(Pawn p)
		{
			Lord lord = p.GetLord();
			return (lord != null && lord.LordJob != null && lord.LordJob.CanOpenAnyDoor(p)) || WildManUtility.WildManShouldReachOutsideNow(p) || base.Faction == null || (p.guest != null && p.guest.Released) || GenAI.MachinesLike(base.Faction, p);
		}

		
		public override bool BlocksPawn(Pawn p)
		{
			return !this.openInt && !this.PawnCanOpen(p);
		}

		
		protected void DoorOpen(int ticksToClose = 110)
		{
			if (this.openInt)
			{
				this.ticksUntilClose = ticksToClose;
			}
			else
			{
				this.ticksUntilClose = this.TicksToOpenNow + ticksToClose;
			}
			if (!this.openInt)
			{
				this.openInt = true;
				this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
				if (this.DoorPowerOn)
				{
					this.def.building.soundDoorOpenPowered.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
					return;
				}
				this.def.building.soundDoorOpenManual.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
		}

		
		protected bool DoorTryClose()
		{
			if (this.holdOpenInt || this.BlockedOpenMomentary)
			{
				return false;
			}
			this.openInt = false;
			this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
			if (this.DoorPowerOn)
			{
				this.def.building.soundDoorClosePowered.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			else
			{
				this.def.building.soundDoorCloseManual.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			return true;
		}

		
		public void StartManualOpenBy(Pawn opener)
		{
			this.DoorOpen(110);
		}

		
		public void StartManualCloseBy(Pawn closer)
		{
			this.ticksUntilClose = 110;
		}

		
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			float num = Mathf.Clamp01((float)this.ticksSinceOpen / (float)this.TicksToOpenNow);
			float d = 0f + 0.45f * num;
			for (int i = 0; i < 2; i++)
			{
				Vector3 vector = default(Vector3);
				Mesh mesh;
				if (i == 0)
				{
					vector = new Vector3(0f, 0f, -1f);
					mesh = MeshPool.plane10;
				}
				else
				{
					vector = new Vector3(0f, 0f, 1f);
					mesh = MeshPool.plane10Flip;
				}
				Rot4 rotation = base.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				vector = rotation.AsQuat * vector;
				Vector3 vector2 = this.DrawPos;
				vector2.y = AltitudeLayer.DoorMoveable.AltitudeFor();
				vector2 += vector * d;
				Graphics.DrawMesh(mesh, vector2, base.Rotation.AsQuat, this.Graphic.MatAt(base.Rotation, null), 0);
			}
			base.Comps_PostDraw();
		}

		
		private static int AlignQualityAgainst(IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return 0;
			}
			if (!c.Walkable(map))
			{
				return 9;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
				{
					return 1;
				}
				Thing thing2 = thing as Blueprint;
				if (thing2 != null)
				{
					if (thing2.def.entityDefToBuild.passability == Traversability.Impassable)
					{
						return 9;
					}
					if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
					{
						return 1;
					}
				}
			}
			return 0;
		}

		
		public static Rot4 DoorRotationAt(IntVec3 loc, Map map)
		{
			int num = 0;
			int num2 = 0;
			int num3 = num + Building_Door.AlignQualityAgainst(loc + IntVec3.East, map) + Building_Door.AlignQualityAgainst(loc + IntVec3.West, map);
			num2 += Building_Door.AlignQualityAgainst(loc + IntVec3.North, map);
			num2 += Building_Door.AlignQualityAgainst(loc + IntVec3.South, map);
			if (num3 >= num2)
			{
				return Rot4.North;
			}
			return Rot4.East;
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			if (base.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandToggleDoorHoldOpen".Translate(),
					defaultDesc = "CommandToggleDoorHoldOpenDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = TexCommand.HoldOpen,
					isActive = (() => this.holdOpenInt),
					toggleAction = delegate
					{
						this.holdOpenInt = !this.holdOpenInt;
					}
				};
			}
			yield break;
			yield break;
		}

		
		private void ClearReachabilityCache(Map map)
		{
			map.reachability.ClearCache();
			this.freePassageWhenClearedReachabilityCache = this.FreePassage;
		}

		
		private void CheckClearReachabilityCacheBecauseOpenedOrClosed()
		{
			if (base.Spawned)
			{
				base.Map.reachability.ClearCacheForHostile(this);
			}
		}

		
		public CompPowerTrader powerComp;

		
		private bool openInt;

		
		private bool holdOpenInt;

		
		private int lastFriendlyTouchTick = -9999;

		
		protected int ticksUntilClose;

		
		protected int ticksSinceOpen;

		
		private bool freePassageWhenClearedReachabilityCache;

		
		private const float OpenTicks = 45f;

		
		private const int CloseDelayTicks = 110;

		
		private const int WillCloseSoonThreshold = 111;

		
		private const int ApproachCloseDelayTicks = 300;

		
		private const int MaxTicksSinceFriendlyTouchToAutoClose = 120;

		
		private const float PowerOffDoorOpenSpeedFactor = 0.25f;

		
		private const float VisualDoorOffsetStart = 0f;

		
		private const float VisualDoorOffsetEnd = 0.45f;
	}
}
