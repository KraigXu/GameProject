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
    // Token: 0x0200030F RID: 783
    public class Thing : Entity, IExposable, ISelectable, ILoadReferenceable, ISignalReceiver
    {
        // Token: 0x17000483 RID: 1155
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

        // Token: 0x17000484 RID: 1156
        // (get) Token: 0x06001606 RID: 5638 RVA: 0x00080332 File Offset: 0x0007E532
        public int MaxHitPoints
        {
            get
            {
                return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true));
            }
        }

        // Token: 0x17000485 RID: 1157
        // (get) Token: 0x06001607 RID: 5639 RVA: 0x00080345 File Offset: 0x0007E545
        public float MarketValue
        {
            get
            {
                return this.GetStatValue(StatDefOf.MarketValue, true);
            }
        }

        // Token: 0x17000486 RID: 1158
        // (get) Token: 0x06001608 RID: 5640 RVA: 0x00080353 File Offset: 0x0007E553
        public virtual float RoyalFavorValue
        {
            get
            {
                return this.GetStatValue(StatDefOf.RoyalFavorValue, true);
            }
        }

        // Token: 0x17000487 RID: 1159
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

        // Token: 0x17000488 RID: 1160
        // (get) Token: 0x0600160A RID: 5642 RVA: 0x000803CD File Offset: 0x0007E5CD
        public virtual bool FireBulwark
        {
            get
            {
                return this.def.Fillage == FillCategory.Full;
            }
        }

        // Token: 0x17000489 RID: 1161
        // (get) Token: 0x0600160B RID: 5643 RVA: 0x000803DD File Offset: 0x0007E5DD
        public bool Destroyed
        {
            get
            {
                return this.mapIndexOrState == -2 || this.mapIndexOrState == -3;
            }
        }

        // Token: 0x1700048A RID: 1162
        // (get) Token: 0x0600160C RID: 5644 RVA: 0x000803F5 File Offset: 0x0007E5F5
        public bool Discarded
        {
            get
            {
                return this.mapIndexOrState == -3;
            }
        }

        // Token: 0x1700048B RID: 1163
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

        // Token: 0x1700048C RID: 1164
        // (get) Token: 0x0600160E RID: 5646 RVA: 0x00080433 File Offset: 0x0007E633
        public bool SpawnedOrAnyParentSpawned
        {
            get
            {
                return this.SpawnedParentOrMe != null;
            }
        }

        // Token: 0x1700048D RID: 1165
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

        // Token: 0x1700048E RID: 1166
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

        // Token: 0x1700048F RID: 1167
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

        // Token: 0x17000490 RID: 1168
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

        // Token: 0x17000491 RID: 1169
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

        // Token: 0x17000492 RID: 1170
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

        // Token: 0x17000493 RID: 1171
        // (get) Token: 0x06001617 RID: 5655 RVA: 0x0008069B File Offset: 0x0007E89B
        public bool Smeltable
        {
            get
            {
                return this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.smeltable);
            }
        }

        // Token: 0x17000494 RID: 1172
        // (get) Token: 0x06001618 RID: 5656 RVA: 0x000806C6 File Offset: 0x0007E8C6
        public bool BurnableByRecipe
        {
            get
            {
                return this.def.burnableByRecipe && (!this.def.MadeFromStuff || this.Stuff.burnableByRecipe);
            }
        }

        // Token: 0x17000495 RID: 1173
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

        // Token: 0x17000496 RID: 1174
        // (get) Token: 0x0600161A RID: 5658 RVA: 0x00080708 File Offset: 0x0007E908
        public Faction Faction
        {
            get
            {
                return this.factionInt;
            }
        }

        // Token: 0x17000497 RID: 1175
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

        // Token: 0x0600161D RID: 5661 RVA: 0x00080754 File Offset: 0x0007E954
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

        // Token: 0x17000498 RID: 1176
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

        // Token: 0x17000499 RID: 1177
        // (get) Token: 0x0600161F RID: 5663 RVA: 0x00080814 File Offset: 0x0007EA14
        public virtual CellRect? CustomRectForSelector
        {
            get
            {
                return null;
            }
        }

        // Token: 0x1700049A RID: 1178
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

        // Token: 0x1700049B RID: 1179
        // (get) Token: 0x06001621 RID: 5665 RVA: 0x00080857 File Offset: 0x0007EA57
        public virtual string LabelNoCount
        {
            get
            {
                return GenLabel.ThingLabel(this, 1, true);
            }
        }

        // Token: 0x1700049C RID: 1180
        // (get) Token: 0x06001622 RID: 5666 RVA: 0x00080861 File Offset: 0x0007EA61
        public override string LabelCap
        {
            get
            {
                return this.Label.CapitalizeFirst(this.def);
            }
        }

        // Token: 0x1700049D RID: 1181
        // (get) Token: 0x06001623 RID: 5667 RVA: 0x00080874 File Offset: 0x0007EA74
        public virtual string LabelCapNoCount
        {
            get
            {
                return this.LabelNoCount.CapitalizeFirst(this.def);
            }
        }

        // Token: 0x1700049E RID: 1182
        // (get) Token: 0x06001624 RID: 5668 RVA: 0x00080887 File Offset: 0x0007EA87
        public override string LabelShort
        {
            get
            {
                return this.LabelNoCount;
            }
        }

        // Token: 0x1700049F RID: 1183
        // (get) Token: 0x06001625 RID: 5669 RVA: 0x0008088F File Offset: 0x0007EA8F
        public virtual bool IngestibleNow
        {
            get
            {
                return !this.IsBurning() && this.def.IsIngestible;
            }
        }

        // Token: 0x170004A0 RID: 1184
        // (get) Token: 0x06001626 RID: 5670 RVA: 0x000808A6 File Offset: 0x0007EAA6
        public ThingDef Stuff
        {
            get
            {
                return this.stuffInt;
            }
        }

        // Token: 0x170004A1 RID: 1185
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

        // Token: 0x170004A2 RID: 1186
        // (get) Token: 0x06001628 RID: 5672 RVA: 0x000808E8 File Offset: 0x0007EAE8
        public virtual Graphic Graphic
        {
            get
            {
                return this.DefaultGraphic;
            }
        }

        // Token: 0x170004A3 RID: 1187
        // (get) Token: 0x06001629 RID: 5673 RVA: 0x000808F0 File Offset: 0x0007EAF0
        public virtual IntVec3 InteractionCell
        {
            get
            {
                return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
            }
        }

        // Token: 0x170004A4 RID: 1188
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

        // Token: 0x170004A5 RID: 1189
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

        // Token: 0x170004A6 RID: 1190
        // (get) Token: 0x0600162C RID: 5676 RVA: 0x000809BE File Offset: 0x0007EBBE
        public bool Suspended
        {
            get
            {
                return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
            }
        }

        // Token: 0x170004A7 RID: 1191
        // (get) Token: 0x0600162D RID: 5677 RVA: 0x000809DF File Offset: 0x0007EBDF
        public virtual string DescriptionDetailed
        {
            get
            {
                return this.def.DescriptionDetailed;
            }
        }

        // Token: 0x170004A8 RID: 1192
        // (get) Token: 0x0600162E RID: 5678 RVA: 0x000809EC File Offset: 0x0007EBEC
        public virtual string DescriptionFlavor
        {
            get
            {
                return this.def.description;
            }
        }

        // Token: 0x170004A9 RID: 1193
        // (get) Token: 0x0600162F RID: 5679 RVA: 0x000809F9 File Offset: 0x0007EBF9
        public TerrainAffordanceDef TerrainAffordanceNeeded
        {
            get
            {
                return this.def.GetTerrainAffordanceNeed(this.stuffInt);
            }
        }

        // Token: 0x06001631 RID: 5681 RVA: 0x00080A46 File Offset: 0x0007EC46
        public virtual void PostMake()
        {
            ThingIDMaker.GiveIDTo(this);
            if (this.def.useHitPoints)
            {
                this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
            }
        }

        // Token: 0x06001632 RID: 5682 RVA: 0x00080A83 File Offset: 0x0007EC83
        public string GetUniqueLoadID()
        {
            return "Thing_" + this.ThingID;
        }

        // Token: 0x06001633 RID: 5683 RVA: 0x00080A98 File Offset: 0x0007EC98
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

        // Token: 0x06001634 RID: 5684 RVA: 0x00080E2C File Offset: 0x0007F02C
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

        // Token: 0x06001635 RID: 5685 RVA: 0x000810AC File Offset: 0x0007F2AC
        public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
        {
            this.Destroy(DestroyMode.KillFinalize);
        }

        // Token: 0x06001636 RID: 5686 RVA: 0x000810B8 File Offset: 0x0007F2B8
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

        // Token: 0x06001637 RID: 5687 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
        {
        }

        // Token: 0x06001638 RID: 5688 RVA: 0x000811CC File Offset: 0x0007F3CC
        public virtual void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
        {
            if (this.def.colorGeneratorInTraderStock != null)
            {
                this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
            }
        }

        // Token: 0x06001639 RID: 5689 RVA: 0x000811F2 File Offset: 0x0007F3F2
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

        // Token: 0x0600163A RID: 5690 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void Notify_LordDestroyed()
        {
        }

        // Token: 0x0600163B RID: 5691 RVA: 0x00081222 File Offset: 0x0007F422
        public void ForceSetStateToUnspawned()
        {
            this.mapIndexOrState = -1;
        }

        // Token: 0x0600163C RID: 5692 RVA: 0x0008122C File Offset: 0x0007F42C
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

        // Token: 0x0600163D RID: 5693 RVA: 0x00081288 File Offset: 0x0007F488
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

        // Token: 0x0600163E RID: 5694 RVA: 0x00081314 File Offset: 0x0007F514
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

        // Token: 0x0600163F RID: 5695 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void PostMapInit()
        {
        }

        // Token: 0x170004AA RID: 1194
        // (get) Token: 0x06001640 RID: 5696 RVA: 0x000814DC File Offset: 0x0007F6DC
        public virtual Vector3 DrawPos
        {
            get
            {
                return this.TrueCenter();
            }
        }

        // Token: 0x170004AB RID: 1195
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

        // Token: 0x170004AC RID: 1196
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

        // Token: 0x06001644 RID: 5700 RVA: 0x00081596 File Offset: 0x0007F796
        public virtual void Draw()
        {
            this.DrawAt(this.DrawPos, false);
        }

        // Token: 0x06001645 RID: 5701 RVA: 0x000815A8 File Offset: 0x0007F7A8
        public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            this.Graphic.Draw(drawLoc, flip ? this.Rotation.Opposite : this.Rotation, this, 0f);
        }

        // Token: 0x06001646 RID: 5702 RVA: 0x000815E0 File Offset: 0x0007F7E0
        public virtual void Print(SectionLayer layer)
        {
            this.Graphic.Print(layer, this);
        }

        // Token: 0x06001647 RID: 5703 RVA: 0x000815F0 File Offset: 0x0007F7F0
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

        // Token: 0x06001648 RID: 5704 RVA: 0x0008165C File Offset: 0x0007F85C
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

        // Token: 0x06001649 RID: 5705 RVA: 0x000816B8 File Offset: 0x0007F8B8
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

        // Token: 0x0600164A RID: 5706 RVA: 0x00081774 File Offset: 0x0007F974
        public virtual string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
            return stringBuilder.ToString();
        }

        // Token: 0x0600164B RID: 5707 RVA: 0x00081788 File Offset: 0x0007F988
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

        // Token: 0x0600164C RID: 5708 RVA: 0x000817E4 File Offset: 0x0007F9E4
        public virtual IEnumerable<Gizmo> GetGizmos()
        {
            yield break;
        }

        // Token: 0x0600164D RID: 5709 RVA: 0x000817ED File Offset: 0x0007F9ED
        public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            yield break;
        }

        // Token: 0x0600164E RID: 5710 RVA: 0x000817F6 File Offset: 0x0007F9F6
        public virtual IEnumerable<InspectTabBase> GetInspectTabs()
        {
            return this.def.inspectorTabsResolved;
        }

        // Token: 0x0600164F RID: 5711 RVA: 0x00081803 File Offset: 0x0007FA03
        public virtual string GetCustomLabelNoCount(bool includeHp = true)
        {
            return GenLabel.ThingLabel(this, 1, includeHp);
        }

        // Token: 0x06001650 RID: 5712 RVA: 0x00081810 File Offset: 0x0007FA10
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

        // Token: 0x06001651 RID: 5713 RVA: 0x0008197E File Offset: 0x0007FB7E
        public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
        }

        // Token: 0x06001652 RID: 5714 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
        }

        // Token: 0x06001653 RID: 5715 RVA: 0x00081984 File Offset: 0x0007FB84
        public virtual bool CanStackWith(Thing other)
        {
            return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && this.def == other.def && this.Stuff == other.Stuff;
        }

        // Token: 0x06001654 RID: 5716 RVA: 0x000819D4 File Offset: 0x0007FBD4
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

        // Token: 0x06001655 RID: 5717 RVA: 0x00081A7C File Offset: 0x0007FC7C
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

        // Token: 0x06001656 RID: 5718 RVA: 0x00081B73 File Offset: 0x0007FD73
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

        // Token: 0x06001657 RID: 5719 RVA: 0x00081B84 File Offset: 0x0007FD84
        public virtual void Notify_ColorChanged()
        {
            this.graphicInt = null;
            if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
            {
                this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
            }
        }

        // Token: 0x06001658 RID: 5720 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void Notify_Equipped(Pawn pawn)
        {
        }

        // Token: 0x06001659 RID: 5721 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void Notify_UsedWeapon(Pawn pawn)
        {
        }

        // Token: 0x0600165A RID: 5722 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void Notify_SignalReceived(Signal signal)
        {
        }

        // Token: 0x0600165B RID: 5723 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void Notify_Explosion(Explosion explosion)
        {
        }

        // Token: 0x0600165C RID: 5724 RVA: 0x00081BD4 File Offset: 0x0007FDD4
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

        // Token: 0x0600165D RID: 5725 RVA: 0x00081C43 File Offset: 0x0007FE43
        public virtual bool BlocksPawn(Pawn p)
        {
            return this.def.passability == Traversability.Impassable;
        }

        // Token: 0x0600165E RID: 5726 RVA: 0x00081C53 File Offset: 0x0007FE53
        public void SetFactionDirect(Faction newFaction)
        {
            if (!this.def.CanHaveFaction)
            {
                Log.Error("Tried to SetFactionDirect on " + this + " which cannot have a faction.", false);
                return;
            }
            this.factionInt = newFaction;
        }

        // Token: 0x0600165F RID: 5727 RVA: 0x00081C80 File Offset: 0x0007FE80
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

        // Token: 0x06001660 RID: 5728 RVA: 0x00081D01 File Offset: 0x0007FF01
        public void SetPositionDirect(IntVec3 newPos)
        {
            this.positionInt = newPos;
        }

        // Token: 0x06001661 RID: 5729 RVA: 0x00081D0A File Offset: 0x0007FF0A
        public void SetStuffDirect(ThingDef newStuff)
        {
            this.stuffInt = newStuff;
        }

        // Token: 0x06001662 RID: 5730 RVA: 0x00081D13 File Offset: 0x0007FF13
        public override string ToString()
        {
            if (this.def != null)
            {
                return this.ThingID;
            }
            return base.GetType().ToString();
        }

        // Token: 0x06001663 RID: 5731 RVA: 0x00081D2F File Offset: 0x0007FF2F
        public override int GetHashCode()
        {
            return this.thingIDNumber;
        }

        // Token: 0x06001664 RID: 5732 RVA: 0x00081D38 File Offset: 0x0007FF38
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

        // Token: 0x06001665 RID: 5733 RVA: 0x00081D93 File Offset: 0x0007FF93
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

        // Token: 0x06001666 RID: 5734 RVA: 0x00081DAA File Offset: 0x0007FFAA
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

        // Token: 0x06001667 RID: 5735 RVA: 0x00081DBC File Offset: 0x0007FFBC
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

        // Token: 0x06001668 RID: 5736 RVA: 0x00002681 File Offset: 0x00000881
        protected virtual void PrePostIngested(Pawn ingester)
        {
        }

        // Token: 0x06001669 RID: 5737 RVA: 0x00002681 File Offset: 0x00000881
        protected virtual void PostIngested(Pawn ingester)
        {
        }

        // Token: 0x0600166A RID: 5738 RVA: 0x00082018 File Offset: 0x00080218
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

        // Token: 0x0600166B RID: 5739 RVA: 0x00082085 File Offset: 0x00080285
        public virtual bool PreventPlayerSellingThingsNearby(out string reason)
        {
            reason = null;
            return false;
        }

        // Token: 0x0600166C RID: 5740 RVA: 0x00010306 File Offset: 0x0000E506
        public virtual ushort PathFindCostFor(Pawn p)
        {
            return 0;
        }

        // Token: 0x04000E6B RID: 3691
        public ThingDef def;

        // Token: 0x04000E6C RID: 3692
        public int thingIDNumber = -1;

        // Token: 0x04000E6D RID: 3693
        private sbyte mapIndexOrState = -1;

        // Token: 0x04000E6E RID: 3694
        private IntVec3 positionInt = IntVec3.Invalid;

        // Token: 0x04000E6F RID: 3695
        private Rot4 rotationInt = Rot4.North;

        // Token: 0x04000E70 RID: 3696
        public int stackCount = 1;

        // Token: 0x04000E71 RID: 3697
        protected Faction factionInt;

        // Token: 0x04000E72 RID: 3698
        private ThingDef stuffInt;

        // Token: 0x04000E73 RID: 3699
        private Graphic graphicInt;

        // Token: 0x04000E74 RID: 3700
        private int hitPointsInt = -1;

        // Token: 0x04000E75 RID: 3701
        public ThingOwner holdingOwner;

        // Token: 0x04000E76 RID: 3702
        public List<string> questTags;

        // Token: 0x04000E77 RID: 3703
        protected const sbyte UnspawnedState = -1;

        // Token: 0x04000E78 RID: 3704
        private const sbyte MemoryState = -2;

        // Token: 0x04000E79 RID: 3705
        private const sbyte DiscardedState = -3;

        // Token: 0x04000E7A RID: 3706
        public static bool allowDestroyNonDestroyable = false;

        // Token: 0x04000E7B RID: 3707
        private static List<string> tmpDeteriorationReasons = new List<string>();

        // Token: 0x04000E7C RID: 3708
        public const float SmeltCostRecoverFraction = 0.25f;
    }
}
