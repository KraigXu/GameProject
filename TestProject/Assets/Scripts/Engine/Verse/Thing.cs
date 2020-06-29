using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	
	public class Thing : Entity, IExposable, ISelectable, ILoadReferenceable, ISignalReceiver
	{
		
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x00080321 File Offset: 0x0007E521
		// (set) Token: 0x06001605 RID: 5637 RVA: 0x00080329 File Offset: 0x0007E529
		public virtual int HitPoints
		{
			get
			{
				return this.hitPointsInt;
			}
			set
			{
				this.hitPointsInt = value;
			}
		}

		
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x00080332 File Offset: 0x0007E532
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true));
			}
		}

		
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x00080345 File Offset: 0x0007E545
		public float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true);
			}
		}

		
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x00080353 File Offset: 0x0007E553
		public virtual float RoyalFavorValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.RoyalFavorValue, true);
			}
		}

		
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x00080364 File Offset: 0x0007E564
		public bool FlammableNow
		{
			get
			{
				if (this.GetStatValue(StatDefOf.Flammability, true) < 0.01f)
				{
					return false;
				}
				if (this.Spawned && !this.FireBulwark)
				{
					List<Thing> thingList = this.Position.GetThingList(this.Map);
					if (thingList != null)
					{
						for (int i = 0; i < thingList.Count; i++)
						{
							if (thingList[i].FireBulwark)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x000803CD File Offset: 0x0007E5CD
		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		
		// (get) Token: 0x0600160B RID: 5643 RVA: 0x000803DD File Offset: 0x0007E5DD
		public bool Destroyed
		{
			get
			{
				return this.mapIndexOrState == -2 || this.mapIndexOrState == -3;
			}
		}

		
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x000803F5 File Offset: 0x0007E5F5
		public bool Discarded
		{
			get
			{
				return this.mapIndexOrState == -3;
			}
		}

		
		// (get) Token: 0x0600160D RID: 5645 RVA: 0x00080401 File Offset: 0x0007E601
		public bool Spawned
		{
			get
			{
				if (this.mapIndexOrState < 0)
				{
					return false;
				}
				if ((int)this.mapIndexOrState < Find.Maps.Count)
				{
					return true;
				}
				Log.ErrorOnce("Thing is associated with invalid map index", 64664487, false);
				return false;
			}
		}

		
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x00080433 File Offset: 0x0007E633
		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.SpawnedParentOrMe != null;
			}
		}

		
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x0008043E File Offset: 0x0007E63E
		public Thing SpawnedParentOrMe
		{
			get
			{
				if (this.Spawned)
				{
					return this;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.SpawnedParentOrMe(this.ParentHolder);
				}
				return null;
			}
		}

		
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x0008045F File Offset: 0x0007E65F
		public Map Map
		{
			get
			{
				if (this.mapIndexOrState >= 0)
				{
					return Find.Maps[(int)this.mapIndexOrState];
				}
				return null;
			}
		}

		
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x0008047C File Offset: 0x0007E67C
		public Map MapHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.GetRootMap(this.ParentHolder);
				}
				return null;
			}
		}

		
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x000804A2 File Offset: 0x0007E6A2
		// (set) Token: 0x06001613 RID: 5651 RVA: 0x000804AC File Offset: 0x0007E6AC
		public IntVec3 Position
		{
			get
			{
				return this.positionInt;
			}
			set
			{
				if (value == this.positionInt)
				{
					return;
				}
				if (this.Spawned)
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed position of a spawned thing which affects regions. This is not supported.", false);
					}
					this.DirtyMapMesh(this.Map);
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
				}
				this.positionInt = value;
				if (this.Spawned)
				{
					this.Map.thingGrid.Register(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					this.DirtyMapMesh(this.Map);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x00080568 File Offset: 0x0007E768
		public IntVec3 PositionHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Position;
				}
				IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.ParentHolder);
				if (rootPosition.IsValid)
				{
					return rootPosition;
				}
				return this.Position;
			}
		}

		
		// (get) Token: 0x06001615 RID: 5653 RVA: 0x000805A1 File Offset: 0x0007E7A1
		// (set) Token: 0x06001616 RID: 5654 RVA: 0x000805AC File Offset: 0x0007E7AC
		public Rot4 Rotation
		{
			get
			{
				return this.rotationInt;
			}
			set
			{
				if (value == this.rotationInt)
				{
					return;
				}
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.", false);
					}
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
				}
				this.rotationInt = value;
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					this.Map.thingGrid.Register(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		
		// (get) Token: 0x06001617 RID: 5655 RVA: 0x0008069B File Offset: 0x0007E89B
		public bool Smeltable
		{
			get
			{
				return this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.smeltable);
			}
		}

		
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x000806C6 File Offset: 0x0007E8C6
		public bool BurnableByRecipe
		{
			get
			{
				return this.def.burnableByRecipe && (!this.def.MadeFromStuff || this.Stuff.burnableByRecipe);
			}
		}

		
		// (get) Token: 0x06001619 RID: 5657 RVA: 0x000806F1 File Offset: 0x0007E8F1
		public IThingHolder ParentHolder
		{
			get
			{
				if (this.holdingOwner == null)
				{
					return null;
				}
				return this.holdingOwner.Owner;
			}
		}

		
		// (get) Token: 0x0600161A RID: 5658 RVA: 0x00080708 File Offset: 0x0007E908
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		
		// (get) Token: 0x0600161B RID: 5659 RVA: 0x00080710 File Offset: 0x0007E910
		// (set) Token: 0x0600161C RID: 5660 RVA: 0x00080746 File Offset: 0x0007E946
		public string ThingID
		{
			get
			{
				if (this.def.HasThingIDNumber)
				{
					return this.def.defName + this.thingIDNumber.ToString();
				}
				return this.def.defName;
			}
			set
			{
				this.thingIDNumber = Thing.IDNumberFromThingID(value);
			}
		}

		
		public static int IDNumberFromThingID(string thingID)
		{
			string value = Regex.Match(thingID, "\\d+$").Value;
			int result = 0;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				result = Convert.ToInt32(value, invariantCulture);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new string[]
				{
					"Could not convert id number from thingID=",
					thingID,
					", numString=",
					value,
					" Exception=",
					ex.ToString()
				}), false);
			}
			return result;
		}

		
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x000807D4 File Offset: 0x0007E9D4
		public IntVec2 RotatedSize
		{
			get
			{
				if (!this.rotationInt.IsHorizontal)
				{
					return this.def.size;
				}
				return new IntVec2(this.def.size.z, this.def.size.x);
			}
		}

		
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x00080814 File Offset: 0x0007EA14
		public virtual CellRect? CustomRectForSelector
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x0008082A File Offset: 0x0007EA2A
		public override string Label
		{
			get
			{
				if (this.stackCount > 1)
				{
					return this.LabelNoCount + " x" + this.stackCount.ToStringCached();
				}
				return this.LabelNoCount;
			}
		}

		
		// (get) Token: 0x06001621 RID: 5665 RVA: 0x00080857 File Offset: 0x0007EA57
		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, true);
			}
		}

		
		// (get) Token: 0x06001622 RID: 5666 RVA: 0x00080861 File Offset: 0x0007EA61
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x00080874 File Offset: 0x0007EA74
		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst(this.def);
			}
		}

		
		// (get) Token: 0x06001624 RID: 5668 RVA: 0x00080887 File Offset: 0x0007EA87
		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		
		// (get) Token: 0x06001625 RID: 5669 RVA: 0x0008088F File Offset: 0x0007EA8F
		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		
		// (get) Token: 0x06001626 RID: 5670 RVA: 0x000808A6 File Offset: 0x0007EAA6
		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x000808AE File Offset: 0x0007EAAE
		public Graphic DefaultGraphic
		{
			get
			{
				if (this.graphicInt == null)
				{
					if (this.def.graphicData == null)
					{
						return BaseContent.BadGraphic;
					}
					this.graphicInt = this.def.graphicData.GraphicColoredFor(this);
				}
				return this.graphicInt;
			}
		}

		
		// (get) Token: 0x06001628 RID: 5672 RVA: 0x000808E8 File Offset: 0x0007EAE8
		public virtual Graphic Graphic
		{
			get
			{
				return this.DefaultGraphic;
			}
		}

		
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x000808F0 File Offset: 0x0007EAF0
		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		
		// (get) Token: 0x0600162A RID: 5674 RVA: 0x00080910 File Offset: 0x0007EB10
		public float AmbientTemperature
		{
			get
			{
				if (this.Spawned)
				{
					return GenTemperature.GetTemperatureForCell(this.Position, this.Map);
				}
				if (this.ParentHolder != null)
				{
					for (IThingHolder parentHolder = this.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
					{
						float result;
						if (ThingOwnerUtility.TryGetFixedTemperature(parentHolder, this, out result))
						{
							return result;
						}
					}
				}
				if (this.SpawnedOrAnyParentSpawned)
				{
					return GenTemperature.GetTemperatureForCell(this.PositionHeld, this.MapHeld);
				}
				if (this.Tile >= 0)
				{
					return GenTemperature.GetTemperatureAtTile(this.Tile);
				}
				return 21f;
			}
		}

		
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x00080993 File Offset: 0x0007EB93
		public int Tile
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map.Tile;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.GetRootTile(this.ParentHolder);
				}
				return -1;
			}
		}

		
		// (get) Token: 0x0600162C RID: 5676 RVA: 0x000809BE File Offset: 0x0007EBBE
		public bool Suspended
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
			}
		}

		
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x000809DF File Offset: 0x0007EBDF
		public virtual string DescriptionDetailed
		{
			get
			{
				return this.def.DescriptionDetailed;
			}
		}

		
		// (get) Token: 0x0600162E RID: 5678 RVA: 0x000809EC File Offset: 0x0007EBEC
		public virtual string DescriptionFlavor
		{
			get
			{
				return this.def.description;
			}
		}

		
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x000809F9 File Offset: 0x0007EBF9
		public TerrainAffordanceDef TerrainAffordanceNeeded
		{
			get
			{
				return this.def.GetTerrainAffordanceNeed(this.stuffInt);
			}
		}

		
		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		
		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Destroyed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Spawning destroyed thing ",
					this,
					" at ",
					this.Position,
					". Correcting."
				}), false);
				this.mapIndexOrState = -1;
				if (this.HitPoints <= 0 && this.def.useHitPoints)
				{
					this.HitPoints = 1;
				}
			}
			if (this.Spawned)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to spawn already-spawned thing ",
					this,
					" at ",
					this.Position
				}), false);
				return;
			}
			int num = Find.Maps.IndexOf(map);
			if (num < 0)
			{
				Log.Error("Tried to spawn thing " + this + ", but the map provided does not exist.", false);
				return;
			}
			if (this.stackCount > this.def.stackLimit)
			{
				Log.Error(string.Concat(new object[]
				{
					"Spawned ",
					this,
					" with stackCount ",
					this.stackCount,
					" but stackLimit is ",
					this.def.stackLimit,
					". Truncating."
				}), false);
				this.stackCount = this.def.stackLimit;
			}
			this.mapIndexOrState = (sbyte)num;
			RegionListersUpdater.RegisterInRegions(this, map);
			if (!map.spawnedThings.TryAdd(this, false))
			{
				Log.Error("Couldn't add thing " + this + " to spawned things.", false);
			}
			map.listerThings.Add(this);
			map.thingGrid.Register(this);
			if (Find.TickManager != null)
			{
				Find.TickManager.RegisterAllTickabilityFor(this);
			}
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.RegisterDrawable(this);
			}
			map.tooltipGiverList.Notify_ThingSpawned(this);
			if (this.def.graphicData != null && this.def.graphicData.Linked)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things, true, false);
			}
			if (!this.def.CanOverlapZones)
			{
				map.zoneManager.Notify_NoZoneOverlapThingSpawned(this);
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsSpawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathGrid.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			map.coverGrid.Register(this);
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_Spawned(this);
				map.listerMergeables.Notify_Spawned(this);
			}
			map.attackTargetsCache.Notify_ThingSpawned(this);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			Room room = (validRegionAt_NoRebuild == null) ? null : validRegionAt_NoRebuild.Room;
			if (room != null)
			{
				room.Notify_ContainedThingSpawnedOrDespawned(this);
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				if (slotGroup != null && slotGroup.parent != null)
				{
					slotGroup.parent.Notify_ReceivedThing(this);
				}
			}
			if (this.def.receivesSignals)
			{
				Find.SignalManager.RegisterReceiver(this);
			}
			if (!respawningAfterLoad)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
			}
		}

		
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is already destroyed.", false);
				return;
			}
			if (!this.Spawned)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is not spawned.", false);
				return;
			}
			Map map = this.Map;
			RegionListersUpdater.DeregisterInRegions(this, map);
			map.spawnedThings.Remove(this);
			map.listerThings.Remove(this);
			map.thingGrid.Deregister(this, false);
			map.coverGrid.DeRegister(this);
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			map.tooltipGiverList.Notify_ThingDespawned(this);
			if (this.def.graphicData != null && this.def.graphicData.Linked)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things, true, false);
			}
			Find.Selector.Deselect(this);
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.DeRegisterDrawable(this);
			}
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			Room room = (validRegionAt_NoRebuild == null) ? null : validRegionAt_NoRebuild.Room;
			if (room != null)
			{
				room.Notify_ContainedThingSpawnedOrDespawned(this);
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsDespawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathGrid.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			Find.TickManager.DeRegisterAllTickabilityFor(this);
			this.mapIndexOrState = -1;
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_DeSpawned(this);
				map.listerMergeables.Notify_DeSpawned(this);
			}
			map.attackTargetsCache.Notify_ThingDespawned(this);
			map.physicalInteractionReservationManager.ReleaseAllForTarget(this);
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				if (slotGroup != null && slotGroup.parent != null)
				{
					slotGroup.parent.Notify_LostThing(this);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
		}

		
		public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		
		public virtual void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!Thing.allowDestroyNonDestroyable && !this.def.destroyable)
			{
				Log.Error("Tried to destroy non-destroyable thing " + this, false);
				return;
			}
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed thing " + this, false);
				return;
			}
			bool spawned = this.Spawned;
			Map map = this.Map;
			if (this.Spawned)
			{
				this.DeSpawn(mode);
			}
			this.mapIndexOrState = -2;
			if (this.def.DiscardOnDestroyed)
			{
				this.Discard(false);
			}
			CompExplosive compExplosive = this.TryGetComp<CompExplosive>();
			if (spawned)
			{
				List<Thing> list = (compExplosive != null) ? new List<Thing>() : null;
				GenLeaving.DoLeavingsFor(this, map, mode, list);
				if (compExplosive != null)
				{
					compExplosive.AddThingsIgnoredByExplosion(list);
				}
			}
			if (this.holdingOwner != null)
			{
				this.holdingOwner.Notify_ContainedItemDestroyed(this);
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
			if (!(this is Pawn))
			{
				this.stackCount = 0;
			}
			if (mode != DestroyMode.QuestLogic)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
			}
			if (mode == DestroyMode.KillFinalize)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Killed", this.Named("SUBJECT"));
			}
		}

		
		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		
		public virtual void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		
		public virtual void Notify_MyMapRemoved()
		{
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			if (!ThingOwnerUtility.AnyParentIs<Pawn>(this))
			{
				this.mapIndexOrState = -3;
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
		}

		
		public virtual void Notify_LordDestroyed()
		{
		}

		
		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = -1;
		}

		
		public void DecrementMapIndex()
		{
			if (this.mapIndexOrState <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for ",
					this,
					", but mapIndexOrState=",
					this.mapIndexOrState
				}), false);
				return;
			}
			this.mapIndexOrState -= 1;
		}

		
		private void RemoveAllReservationsAndDesignationsOnThis()
		{
			if (this.def.category == ThingCategory.Mote)
			{
				return;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reservationManager.ReleaseAllForTarget(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllForTarget(this);
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					maps[i].attackTargetReservationManager.ReleaseAllForTarget(attackTarget);
				}
				maps[i].designationManager.RemoveAllDesignationsOn(this, false);
			}
		}

		
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			if (this.def.HasThingIDNumber)
			{
				string thingID = this.ThingID;
				Scribe_Values.Look<string>(ref thingID, "id", null, false);
				this.ThingID = thingID;
			}
			Scribe_Values.Look<sbyte>(ref this.mapIndexOrState, "map", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.mapIndexOrState >= 0)
			{
				this.mapIndexOrState = -1;
			}
			Scribe_Values.Look<IntVec3>(ref this.positionInt, "pos", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.rotationInt, "rot", Rot4.North, false);
			if (this.def.useHitPoints)
			{
				Scribe_Values.Look<int>(ref this.hitPointsInt, "health", -1, false);
			}
			bool flag = this.def.tradeability != Tradeability.None && this.def.category == ThingCategory.Item;
			if (this.def.stackLimit > 1 || flag)
			{
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, true);
			}
			Scribe_Defs.Look<ThingDef>(ref this.stuffInt, "stuff");
			string facID = (this.factionInt != null) ? this.factionInt.GetUniqueLoadID() : "null";
			Scribe_Values.Look<string>(ref facID, "faction", "null", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (facID == "null")
				{
					this.factionInt = null;
				}
				else if (Find.World != null && Find.FactionManager != null)
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
			}
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		
		public virtual void PostMapInit()
		{
		}

		
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x000814DC File Offset: 0x0007F6DC
		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x000814E4 File Offset: 0x0007F6E4
		// (set) Token: 0x06001642 RID: 5698 RVA: 0x00081524 File Offset: 0x0007F724
		public virtual Color DrawColor
		{
			get
			{
				if (this.Stuff != null)
				{
					return this.def.GetColorForStuff(this.Stuff);
				}
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.color;
				}
				return Color.white;
			}
			set
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot set instance color on non-ThingWithComps ",
					this.LabelCap,
					" at ",
					this.Position,
					"."
				}), false);
			}
		}

		
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x00081571 File Offset: 0x0007F771
		public virtual Color DrawColorTwo
		{
			get
			{
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.colorTwo;
				}
				return Color.white;
			}
		}

		
		public virtual void Draw()
		{
			this.DrawAt(this.DrawPos, false);
		}

		
		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Graphic.Draw(drawLoc, flip ? this.Rotation.Opposite : this.Rotation, this, 0f);
		}

		
		public virtual void Print(SectionLayer layer)
		{
			this.Graphic.Print(layer, this);
		}

		
		public void DirtyMapMesh(Map map)
		{
			if (this.def.drawerType != DrawerType.RealtimeOnly)
			{
				foreach (IntVec3 loc in this.OccupiedRect())
				{
					map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Things);
				}
			}
		}

		
		public virtual void DrawGUIOverlay()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				if (this.def.stackLimit > 1)
				{
					GenMapUI.DrawThingLabel(this, this.stackCount.ToStringCached());
					return;
				}
				QualityCategory cat;
				if (this.def.drawGUIOverlayQuality && this.TryGetQuality(out cat))
				{
					GenMapUI.DrawThingLabel(this, cat.GetLabelShort());
				}
			}
		}

		
		public virtual void DrawExtraSelectionOverlays()
		{
			if (this.def.specialDisplayRadius > 0.1f)
			{
				GenDraw.DrawRadiusRing(this.Position, this.def.specialDisplayRadius);
			}
			if (this.def.drawPlaceWorkersWhileSelected && this.def.PlaceWorkers != null)
			{
				for (int i = 0; i < this.def.PlaceWorkers.Count; i++)
				{
					this.def.PlaceWorkers[i].DrawGhost(this.def, this.Position, this.Rotation, Color.white, this);
				}
			}
			if (this.def.hasInteractionCell)
			{
				GenDraw.DrawInteractionCell(this.def, this.Position, this.rotationInt);
			}
		}

		
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		
		public virtual string GetInspectStringLowPriority()
		{
			string result = null;
			Thing.tmpDeteriorationReasons.Clear();
			SteadyEnvironmentEffects.FinalDeteriorationRate(this, Thing.tmpDeteriorationReasons);
			if (Thing.tmpDeteriorationReasons.Count != 0)
			{
				result = string.Format("{0}: {1}", "DeterioratingBecauseOf".Translate(), Thing.tmpDeteriorationReasons.ToCommaList(false).CapitalizeFirst());
			}
			return result;
		}

		
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		
		public virtual string GetCustomLabelNoCount(bool includeHp = true)
		{
			return GenLabel.ThingLabel(this, 1, includeHp);
		}

		
		public DamageWorker.DamageResult TakeDamage(DamageInfo dinfo)
		{
			if (this.Destroyed)
			{
				return new DamageWorker.DamageResult();
			}
			if (dinfo.Amount == 0f)
			{
				return new DamageWorker.DamageResult();
			}
			if (this.def.damageMultipliers != null)
			{
				for (int i = 0; i < this.def.damageMultipliers.Count; i++)
				{
					if (this.def.damageMultipliers[i].damageDef == dinfo.Def)
					{
						int num = Mathf.RoundToInt(dinfo.Amount * this.def.damageMultipliers[i].multiplier);
						dinfo.SetAmount((float)num);
					}
				}
			}
			bool flag;
			this.PreApplyDamage(ref dinfo, out flag);
			if (flag)
			{
				return new DamageWorker.DamageResult();
			}
			bool spawnedOrAnyParentSpawned = this.SpawnedOrAnyParentSpawned;
			Map mapHeld = this.MapHeld;
			DamageWorker.DamageResult damageResult = dinfo.Def.Worker.Apply(dinfo, this);
			if (dinfo.Def.harmsHealth && spawnedOrAnyParentSpawned)
			{
				mapHeld.damageWatcher.Notify_DamageTaken(this, damageResult.totalDamageDealt);
			}
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				GenLeaving.DropFilthDueToDamage(this, damageResult.totalDamageDealt);
				if (dinfo.Instigator != null)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null)
					{
						pawn.records.AddTo(RecordDefOf.DamageDealt, damageResult.totalDamageDealt);
						pawn.records.AccumulateStoryEvent(StoryEventDefOf.DamageDealt);
					}
				}
			}
			this.PostApplyDamage(dinfo, damageResult.totalDamageDealt);
			return damageResult;
		}

		
		public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		
		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		
		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && this.def == other.def && this.Stuff == other.Stuff;
		}

		
		public virtual bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int num = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.CeilToInt((float)(this.HitPoints * this.stackCount + other.HitPoints * num) / (float)(this.stackCount + num));
			}
			this.stackCount += num;
			other.stackCount -= num;
			StealAIDebugDrawer.Notify_ThingChanged(this);
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (other.stackCount <= 0)
			{
				other.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		
		public virtual Thing SplitOff(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentException("SplitOff with count <= 0", "count");
			}
			if (count >= this.stackCount)
			{
				if (count > this.stackCount)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to split off ",
						count,
						" of ",
						this,
						" but there are only ",
						this.stackCount
					}), false);
				}
				if (this.Spawned)
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				if (this.holdingOwner != null)
				{
					this.holdingOwner.Remove(this);
				}
				return this;
			}
			Thing thing = ThingMaker.MakeThing(this.def, this.Stuff);
			thing.stackCount = count;
			this.stackCount -= count;
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (this.def.useHitPoints)
			{
				thing.HitPoints = this.HitPoints;
			}
			return thing;
		}

		
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.Stuff != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Stuff_Name".Translate(), this.Stuff.LabelCap, "Stat_Stuff_Desc".Translate(), 1100, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(this.Stuff, -1)
				}, false);
			}
			yield break;
		}

		
		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
			if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
			{
				this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
			}
		}

		
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		
		public virtual void Notify_Explosion(Explosion explosion)
		{
		}

		
		public virtual TipSignal GetTooltip()
		{
			string text = this.LabelCap;
			if (this.def.useHitPoints)
			{
				text = string.Concat(new object[]
				{
					text,
					"\n",
					this.HitPoints,
					" / ",
					this.MaxHitPoints
				});
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		
		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable;
		}

		
		public void SetFactionDirect(Faction newFaction)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFactionDirect on " + this + " which cannot have a faction.", false);
				return;
			}
			this.factionInt = newFaction;
		}

		
		public virtual void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFaction on " + this + " which cannot have a faction.", false);
				return;
			}
			this.factionInt = newFaction;
			if (this.Spawned)
			{
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					this.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFaction", this.Named("SUBJECT"), newFaction.Named("FACTION"));
		}

		
		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		
		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		
		public override string ToString()
		{
			if (this.def != null)
			{
				return this.ThingID;
			}
			return base.GetType().ToString();
		}

		
		public override int GetHashCode()
		{
			return this.thingIDNumber;
		}

		
		public virtual void Discard(bool silentlyRemoveReferences = false)
		{
			if (this.mapIndexOrState != -2)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to discard ",
					this,
					" whose state is ",
					this.mapIndexOrState,
					"."
				}), false);
				return;
			}
			this.mapIndexOrState = -3;
		}

		
		public virtual IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.def.butcherProducts != null)
			{
				int num2;
				for (int i = 0; i < this.def.butcherProducts.Count; i = num2 + 1)
				{
					ThingDefCountClass thingDefCountClass = this.def.butcherProducts[i];
					int num = GenMath.RoundRandom((float)thingDefCountClass.count * efficiency);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
						thing.stackCount = num;
						yield return thing;
					}
					num2 = i;
				}
			}
			yield break;
		}

		
		public virtual IEnumerable<Thing> SmeltProducts(float efficiency)
		{
			List<ThingDefCountClass> costListAdj = this.def.CostListAdjusted(this.Stuff, true);
			int num2;
			for (int i = 0; i < costListAdj.Count; i = num2 + 1)
			{
				if (!costListAdj[i].thingDef.intricate)
				{
					int num = GenMath.RoundRandom((float)costListAdj[i].count * 0.25f);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(costListAdj[i].thingDef, null);
						thing.stackCount = num;
						yield return thing;
					}
				}
				num2 = i;
			}
			if (this.def.smeltProducts != null)
			{
				for (int i = 0; i < this.def.smeltProducts.Count; i = num2 + 1)
				{
					ThingDefCountClass thingDefCountClass = this.def.smeltProducts[i];
					Thing thing2 = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
					thing2.stackCount = thingDefCountClass.count;
					yield return thing2;
					num2 = i;
				}
			}
			yield break;
		}

		
		public float Ingested(Pawn ingester, float nutritionWanted)
		{
			if (this.Destroyed)
			{
				Log.Error(ingester + " ingested destroyed thing " + this, false);
				return 0f;
			}
			if (!this.IngestibleNow)
			{
				Log.Error(ingester + " ingested IngestibleNow=false thing " + this, false);
				return 0f;
			}
			ingester.mindState.lastIngestTick = Find.TickManager.TicksGame;
			if (ingester.needs.mood != null)
			{
				List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(ingester, this, this.def);
				for (int i = 0; i < list.Count; i++)
				{
					ingester.needs.mood.thoughts.memories.TryGainMemory(list[i], null);
				}
			}
			if (ingester.needs.drugsDesire != null)
			{
				ingester.needs.drugsDesire.Notify_IngestedDrug(this);
			}
			if (ingester.IsColonist && FoodUtility.IsHumanlikeMeatOrHumanlikeCorpse(this))
			{
				TaleRecorder.RecordTale(TaleDefOf.AteRawHumanlikeMeat, new object[]
				{
					ingester
				});
			}
			int num;
			float result;
			this.IngestedCalculateAmounts(ingester, nutritionWanted, out num, out result);
			if (!ingester.Dead && ingester.needs.joy != null && Mathf.Abs(this.def.ingestible.joy) > 0.0001f && num > 0)
			{
				JoyKindDef joyKind = (this.def.ingestible.joyKind != null) ? this.def.ingestible.joyKind : JoyKindDefOf.Gluttonous;
				ingester.needs.joy.GainJoy((float)num * this.def.ingestible.joy, joyKind);
			}
			if (ingester.RaceProps.Humanlike && Rand.Chance(this.GetStatValue(StatDefOf.FoodPoisonChanceFixedHuman, true) * FoodUtility.GetFoodPoisonChanceFactor(ingester)))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this, FoodPoisonCause.DangerousFoodType);
			}
			bool flag = false;
			if (num > 0)
			{
				if (this.stackCount == 0)
				{
					Log.Error(this + " stack count is 0.", false);
				}
				if (num == this.stackCount)
				{
					flag = true;
				}
				else
				{
					this.SplitOff(num);
				}
			}
			this.PrePostIngested(ingester);
			if (this.def.ingestible.outcomeDoers != null)
			{
				for (int j = 0; j < this.def.ingestible.outcomeDoers.Count; j++)
				{
					this.def.ingestible.outcomeDoers[j].DoIngestionOutcome(ingester, this);
				}
			}
			if (flag)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.PostIngested(ingester);
			return result;
		}

		
		protected virtual void PrePostIngested(Pawn ingester)
		{
		}

		
		protected virtual void PostIngested(Pawn ingester)
		{
		}

		
		protected virtual void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			numTaken = Mathf.CeilToInt(nutritionWanted / this.GetStatValue(StatDefOf.Nutrition, true));
			numTaken = Mathf.Min(new int[]
			{
				numTaken,
				this.def.ingestible.maxNumToIngestAtOnce,
				this.stackCount
			});
			numTaken = Mathf.Max(numTaken, 1);
			nutritionIngested = (float)numTaken * this.GetStatValue(StatDefOf.Nutrition, true);
		}

		
		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = null;
			return false;
		}

		
		public virtual ushort PathFindCostFor(Pawn p)
		{
			return 0;
		}

		
		public ThingDef def;

		
		public int thingIDNumber = -1;

		
		private sbyte mapIndexOrState = -1;

		
		private IntVec3 positionInt = IntVec3.Invalid;

		
		private Rot4 rotationInt = Rot4.North;

		
		public int stackCount = 1;

		
		protected Faction factionInt;

		
		private ThingDef stuffInt;

		
		private Graphic graphicInt;

		
		private int hitPointsInt = -1;

		
		public ThingOwner holdingOwner;

		
		public List<string> questTags;

		
		protected const sbyte UnspawnedState = -1;

		
		private const sbyte MemoryState = -2;

		
		private const sbyte DiscardedState = -3;

		
		public static bool allowDestroyNonDestroyable = false;

		
		private static List<string> tmpDeteriorationReasons = new List<string>();

		
		public const float SmeltCostRecoverFraction = 0.25f;
	}
}
