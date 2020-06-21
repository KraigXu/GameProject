using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirit
{
    public abstract class ThingOwner : IExposable, IList<Thing>, ICollection<Thing>, IEnumerable<Thing>, IEnumerable
    {
        // Token: 0x170004B3 RID: 1203
        // (get) Token: 0x06001693 RID: 5779 RVA: 0x000828FF File Offset: 0x00080AFF
        public IThingHolder Owner
        {
            get
            {
                return this.owner;
            }
        }

        // Token: 0x170004B4 RID: 1204
        // (get) Token: 0x06001694 RID: 5780
        public abstract int Count { get; }

        // Token: 0x170004B5 RID: 1205
        public Thing this[int index]
        {
            get
            {
                return this.GetAt(index);
            }
        }

        // Token: 0x170004B6 RID: 1206
        // (get) Token: 0x06001696 RID: 5782 RVA: 0x00082910 File Offset: 0x00080B10
        public bool Any
        {
            get
            {
                return this.Count > 0;
            }
        }

        // Token: 0x170004B7 RID: 1207
        // (get) Token: 0x06001697 RID: 5783 RVA: 0x0008291C File Offset: 0x00080B1C
        public int TotalStackCount
        {
            get
            {
                int num = 0;
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    num += this.GetAt(i).stackCount;
                }
                return num;
            }
        }

        // Token: 0x170004B8 RID: 1208
        // (get) Token: 0x06001698 RID: 5784 RVA: 0x0008294E File Offset: 0x00080B4E
        public string ContentsString
        {
            get
            {
                if (this.Any)
                {
                    return GenThing.ThingsToCommaList(this, true, true, -1);
                }
                return "NothingLower".Translate();
            }
        }

        // Token: 0x170004B9 RID: 1209
        Thing IList<Thing>.this[int index]
        {
            get
            {
                return this.GetAt(index);
            }
            set
            {
                throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
            }
        }

        // Token: 0x170004BA RID: 1210
        // (get) Token: 0x0600169B RID: 5787 RVA: 0x0001028D File Offset: 0x0000E48D
        bool ICollection<Thing>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        // Token: 0x0600169C RID: 5788 RVA: 0x00082971 File Offset: 0x00080B71
        public ThingOwner()
        {
        }

        // Token: 0x0600169D RID: 5789 RVA: 0x0008298B File Offset: 0x00080B8B
        public ThingOwner(IThingHolder owner)
        {
            this.owner = owner;
        }

        // Token: 0x0600169E RID: 5790 RVA: 0x000829AC File Offset: 0x00080BAC
        public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : this(owner)
        {
            this.maxStacks = (oneStackOnly ? 1 : 999999);
            this.contentsLookMode = contentsLookMode;
        }

        // Token: 0x0600169F RID: 5791 RVA: 0x000829CD File Offset: 0x00080BCD
        public virtual void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.maxStacks, "maxStacks", 999999, false);
            Scribe_Values.Look<LookMode>(ref this.contentsLookMode, "contentsLookMode", LookMode.Deep, false);
        }

        // Token: 0x060016A0 RID: 5792 RVA: 0x000829F8 File Offset: 0x00080BF8
        public void ThingOwnerTick(bool removeIfDestroyed = true)
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                Thing at = this.GetAt(i);
                if (at.def.tickerType == TickerType.Normal)
                {
                    at.Tick();
                    if (at.Destroyed && removeIfDestroyed)
                    {
                        this.Remove(at);
                    }
                }
            }
        }

        // Token: 0x060016A1 RID: 5793 RVA: 0x00082A48 File Offset: 0x00080C48
        public void ThingOwnerTickRare(bool removeIfDestroyed = true)
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                Thing at = this.GetAt(i);
                if (at.def.tickerType == TickerType.Rare)
                {
                    at.TickRare();
                    if (at.Destroyed && removeIfDestroyed)
                    {
                        this.Remove(at);
                    }
                }
            }
        }

        // Token: 0x060016A2 RID: 5794 RVA: 0x00082A98 File Offset: 0x00080C98
        public void ThingOwnerTickLong(bool removeIfDestroyed = true)
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                Thing at = this.GetAt(i);
                if (at.def.tickerType == TickerType.Long)
                {
                    at.TickRare();
                    if (at.Destroyed && removeIfDestroyed)
                    {
                        this.Remove(at);
                    }
                }
            }
        }

        // Token: 0x060016A3 RID: 5795 RVA: 0x00082AE8 File Offset: 0x00080CE8
        public void Clear()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.Remove(this.GetAt(i));
            }
        }

        // Token: 0x060016A4 RID: 5796 RVA: 0x00082B18 File Offset: 0x00080D18
        public void ClearAndDestroyContents(DestroyMode mode = DestroyMode.Vanish)
        {
            while (this.Any)
            {
                for (int i = this.Count - 1; i >= 0; i--)
                {
                    Thing at = this.GetAt(i);
                    at.Destroy(mode);
                    this.Remove(at);
                }
            }
        }

        // Token: 0x060016A5 RID: 5797 RVA: 0x00082B5C File Offset: 0x00080D5C
        public void ClearAndDestroyContentsOrPassToWorld(DestroyMode mode = DestroyMode.Vanish)
        {
            while (this.Any)
            {
                for (int i = this.Count - 1; i >= 0; i--)
                {
                    Thing at = this.GetAt(i);
                    at.DestroyOrPassToWorld(mode);
                    this.Remove(at);
                }
            }
        }

        // Token: 0x060016A6 RID: 5798 RVA: 0x00082B9D File Offset: 0x00080D9D
        public bool CanAcceptAnyOf(Thing item, bool canMergeWithExistingStacks = true)
        {
            return this.GetCountCanAccept(item, canMergeWithExistingStacks) > 0;
        }

        // Token: 0x060016A7 RID: 5799 RVA: 0x00082BAC File Offset: 0x00080DAC
        public virtual int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
        {
            if (item == null || item.stackCount <= 0)
            {
                return 0;
            }
            if (this.maxStacks == 999999)
            {
                return item.stackCount;
            }
            int num = 0;
            if (this.Count < this.maxStacks)
            {
                num += (this.maxStacks - this.Count) * item.def.stackLimit;
            }
            if (num >= item.stackCount)
            {
                return Mathf.Min(num, item.stackCount);
            }
            if (canMergeWithExistingStacks)
            {
                int i = 0;
                int count = this.Count;
                while (i < count)
                {
                    Thing at = this.GetAt(i);
                    if (at.stackCount < at.def.stackLimit && at.CanStackWith(item))
                    {
                        num += at.def.stackLimit - at.stackCount;
                        if (num >= item.stackCount)
                        {
                            return Mathf.Min(num, item.stackCount);
                        }
                    }
                    i++;
                }
            }
            return Mathf.Min(num, item.stackCount);
        }

        // Token: 0x060016A8 RID: 5800
        public abstract int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true);

        // Token: 0x060016A9 RID: 5801
        public abstract bool TryAdd(Thing item, bool canMergeWithExistingStacks = true);

        // Token: 0x060016AA RID: 5802
        public abstract int IndexOf(Thing item);

        // Token: 0x060016AB RID: 5803
        public abstract bool Remove(Thing item);

        // Token: 0x060016AC RID: 5804
        protected abstract Thing GetAt(int index);

        // Token: 0x060016AD RID: 5805 RVA: 0x00082C8E File Offset: 0x00080E8E
        public bool Contains(Thing item)
        {
            return item != null && item.holdingOwner == this;
        }

        // Token: 0x060016AE RID: 5806 RVA: 0x00082C9E File Offset: 0x00080E9E
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.Remove(this.GetAt(index));
        }

        // Token: 0x060016AF RID: 5807 RVA: 0x00082CC6 File Offset: 0x00080EC6
        public int TryAddOrTransfer(Thing item, int count, bool canMergeWithExistingStacks = true)
        {
            if (item == null)
            {
                Log.Warning("Tried to add or transfer null item to ThingOwner.", false);
                return 0;
            }
            if (item.holdingOwner != null)
            {
                return item.holdingOwner.TryTransferToContainer(item, this, count, canMergeWithExistingStacks);
            }
            return this.TryAdd(item, count, canMergeWithExistingStacks);
        }

        // Token: 0x060016B0 RID: 5808 RVA: 0x00082CF9 File Offset: 0x00080EF9
        public bool TryAddOrTransfer(Thing item, bool canMergeWithExistingStacks = true)
        {
            if (item == null)
            {
                Log.Warning("Tried to add or transfer null item to ThingOwner.", false);
                return false;
            }
            if (item.holdingOwner != null)
            {
                return item.holdingOwner.TryTransferToContainer(item, this, canMergeWithExistingStacks);
            }
            return this.TryAdd(item, canMergeWithExistingStacks);
        }

        // Token: 0x060016B1 RID: 5809 RVA: 0x00082D2C File Offset: 0x00080F2C
        public void TryAddRangeOrTransfer(IEnumerable<Thing> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
        {
            if (things == this)
            {
                return;
            }
            ThingOwner thingOwner = things as ThingOwner;
            if (thingOwner != null)
            {
                thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
                if (destroyLeftover)
                {
                    thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
                    return;
                }
            }
            else
            {
                IList<Thing> list = things as IList<Thing>;
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!this.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
                        {
                            list[i].Destroy(DestroyMode.Vanish);
                        }
                    }
                    return;
                }
                foreach (Thing thing in things)
                {
                    if (!this.TryAddOrTransfer(thing, canMergeWithExistingStacks) && destroyLeftover)
                    {
                        thing.Destroy(DestroyMode.Vanish);
                    }
                }
            }
        }

        // Token: 0x060016B2 RID: 5810 RVA: 0x00082DEC File Offset: 0x00080FEC
        public int RemoveAll(Predicate<Thing> predicate)
        {
            int num = 0;
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (predicate(this.GetAt(i)))
                {
                    this.Remove(this.GetAt(i));
                    num++;
                }
            }
            return num;
        }

        // Token: 0x060016B3 RID: 5811 RVA: 0x00082E30 File Offset: 0x00081030
        public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, bool canMergeWithExistingStacks = true)
        {
            return this.TryTransferToContainer(item, otherContainer, item.stackCount, canMergeWithExistingStacks) == item.stackCount;
        }

        // Token: 0x060016B4 RID: 5812 RVA: 0x00082E4C File Offset: 0x0008104C
        public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, bool canMergeWithExistingStacks = true)
        {
            Thing thing;
            return this.TryTransferToContainer(item, otherContainer, count, out thing, canMergeWithExistingStacks);
        }

        // Token: 0x060016B5 RID: 5813 RVA: 0x00082E68 File Offset: 0x00081068
        public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, out Thing resultingTransferredItem, bool canMergeWithExistingStacks = true)
        {
            if (!this.Contains(item))
            {
                Log.Error(string.Concat(new object[]
                {
                    "Can't transfer item ",
                    item,
                    " because it's not here. owner=",
                    this.owner.ToStringSafe<IThingHolder>()
                }), false);
                resultingTransferredItem = null;
                return 0;
            }
            if (otherContainer == this && count > 0)
            {
                resultingTransferredItem = item;
                return item.stackCount;
            }
            if (!otherContainer.CanAcceptAnyOf(item, canMergeWithExistingStacks))
            {
                resultingTransferredItem = null;
                return 0;
            }
            if (count <= 0)
            {
                resultingTransferredItem = null;
                return 0;
            }
            if (this.owner is Map || otherContainer.owner is Map)
            {
                Log.Warning("Can't transfer items to or from Maps directly. They must be spawned or despawned manually. Use TryAdd(item.SplitOff(count))", false);
                resultingTransferredItem = null;
                return 0;
            }
            int num = Mathf.Min(item.stackCount, count);
            Thing thing = item.SplitOff(num);
            if (this.Contains(thing))
            {
                this.Remove(thing);
            }
            if (otherContainer.TryAdd(thing, canMergeWithExistingStacks))
            {
                resultingTransferredItem = thing;
                return thing.stackCount;
            }
            resultingTransferredItem = null;
            if (otherContainer.Contains(thing) || thing.stackCount <= 0 || thing.Destroyed)
            {
                return thing.stackCount;
            }
            int result = num - thing.stackCount;
            if (item != thing)
            {
                item.TryAbsorbStack(thing, false);
                return result;
            }
            this.TryAdd(thing, false);
            return result;
        }

        // Token: 0x060016B6 RID: 5814 RVA: 0x00082F90 File Offset: 0x00081190
        public void TryTransferAllToContainer(ThingOwner other, bool canMergeWithExistingStacks = true)
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.TryTransferToContainer(this.GetAt(i), other, canMergeWithExistingStacks);
            }
        }

        // Token: 0x060016B7 RID: 5815 RVA: 0x00082FC0 File Offset: 0x000811C0
        public Thing Take(Thing thing, int count)
        {
            if (!this.Contains(thing))
            {
                Log.Error("Tried to take " + thing.ToStringSafe<Thing>() + " but it's not here.", false);
                return null;
            }
            if (count > thing.stackCount)
            {
                Log.Error(string.Concat(new object[]
                {
                    "Tried to get ",
                    count,
                    " of ",
                    thing.ToStringSafe<Thing>(),
                    " while only having ",
                    thing.stackCount
                }), false);
                count = thing.stackCount;
            }
            if (count == thing.stackCount)
            {
                this.Remove(thing);
                return thing;
            }
            Thing thing2 = thing.SplitOff(count);
            thing2.holdingOwner = null;
            return thing2;
        }

        // Token: 0x060016B8 RID: 5816 RVA: 0x0008306E File Offset: 0x0008126E
        public Thing Take(Thing thing)
        {
            return this.Take(thing, thing.stackCount);
        }

        // Token: 0x060016B9 RID: 5817 RVA: 0x00083080 File Offset: 0x00081280
        public bool TryDrop(Thing thing, ThingPlaceMode mode, int count, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
            IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
            if (rootMap == null || !rootPosition.IsValid)
            {
                Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.", false);
                lastResultingThing = null;
                return false;
            }
            return this.TryDrop(thing, rootPosition, rootMap, mode, count, out lastResultingThing, placedAction, nearPlaceValidator);
        }

        // Token: 0x060016BA RID: 5818 RVA: 0x000830E0 File Offset: 0x000812E0
        public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            if (!this.Contains(thing))
            {
                Log.Error("Tried to drop " + thing.ToStringSafe<Thing>() + " but it's not here.", false);
                resultingThing = null;
                return false;
            }
            if (thing.stackCount < count)
            {
                Log.Error(string.Concat(new object[]
                {
                    "Tried to drop ",
                    count,
                    " of ",
                    thing,
                    " while only having ",
                    thing.stackCount
                }), false);
                count = thing.stackCount;
            }
            if (count == thing.stackCount)
            {
                if (GenDrop.TryDropSpawn_NewTmp(thing, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
                {
                    this.Remove(thing);
                    return true;
                }
                return false;
            }
            else
            {
                Thing thing2 = thing.SplitOff(count);
                if (GenDrop.TryDropSpawn_NewTmp(thing2, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
                {
                    return true;
                }
                thing.TryAbsorbStack(thing2, false);
                return false;
            }
        }

        // Token: 0x060016BB RID: 5819 RVA: 0x000831C0 File Offset: 0x000813C0
        public bool TryDrop(Thing thing, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
            IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
            if (rootMap == null || !rootPosition.IsValid)
            {
                Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.", false);
                lastResultingThing = null;
                return false;
            }
            return this.TryDrop_NewTmp(thing, rootPosition, rootMap, mode, out lastResultingThing, placedAction, nearPlaceValidator, true);
        }

        // Token: 0x060016BC RID: 5820 RVA: 0x0008321C File Offset: 0x0008141C
        [Obsolete("Only used for mod compatibility")]
        public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            return this.TryDrop_NewTmp(thing, dropLoc, map, mode, out lastResultingThing, placedAction, nearPlaceValidator, true);
        }

        // Token: 0x060016BD RID: 5821 RVA: 0x0008323C File Offset: 0x0008143C
        public bool TryDrop_NewTmp(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
        {
            if (!this.Contains(thing))
            {
                Log.Error(this.owner.ToStringSafe<IThingHolder>() + " container tried to drop  " + thing.ToStringSafe<Thing>() + " which it didn't contain.", false);
                lastResultingThing = null;
                return false;
            }
            if (GenDrop.TryDropSpawn_NewTmp(thing, dropLoc, map, mode, out lastResultingThing, placedAction, nearPlaceValidator, playDropSound))
            {
                this.Remove(thing);
                return true;
            }
            return false;
        }

        // Token: 0x060016BE RID: 5822 RVA: 0x000832A0 File Offset: 0x000814A0
        public bool TryDropAll(IntVec3 dropLoc, Map map, ThingPlaceMode mode, Action<Thing, int> placeAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            bool result = true;
            for (int i = this.Count - 1; i >= 0; i--)
            {
                Thing thing;
                if (!this.TryDrop_NewTmp(this.GetAt(i), dropLoc, map, mode, out thing, placeAction, nearPlaceValidator, true))
                {
                    result = false;
                }
            }
            return result;
        }

        // Token: 0x060016BF RID: 5823 RVA: 0x000832DE File Offset: 0x000814DE
        public bool Contains(ThingDef def)
        {
            return this.Contains(def, 1);
        }

        // Token: 0x060016C0 RID: 5824 RVA: 0x000832E8 File Offset: 0x000814E8
        public bool Contains(ThingDef def, int minCount)
        {
            if (minCount <= 0)
            {
                return true;
            }
            int num = 0;
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.GetAt(i).def == def)
                {
                    num += this.GetAt(i).stackCount;
                }
                if (num >= minCount)
                {
                    return true;
                }
            }
            return false;
        }

        // Token: 0x060016C1 RID: 5825 RVA: 0x00083338 File Offset: 0x00081538
        public int TotalStackCountOfDef(ThingDef def)
        {
            int num = 0;
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.GetAt(i).def == def)
                {
                    num += this.GetAt(i).stackCount;
                }
            }
            return num;
        }

        // Token: 0x060016C2 RID: 5826 RVA: 0x00083379 File Offset: 0x00081579
        public void Notify_ContainedItemDestroyed(Thing t)
        {
            if (ThingOwnerUtility.ShouldAutoRemoveDestroyedThings(this.owner))
            {
                this.Remove(t);
            }
        }

        // Token: 0x060016C3 RID: 5827 RVA: 0x00083390 File Offset: 0x00081590
        protected void NotifyAdded(Thing item)
        {
            if (ThingOwnerUtility.ShouldAutoExtinguishInnerThings(this.owner) && item.HasAttachment(ThingDefOf.Fire))
            {
                item.GetAttachment(ThingDefOf.Fire).Destroy(DestroyMode.Vanish);
            }
            if (ThingOwnerUtility.ShouldRemoveDesignationsOnAddedThings(this.owner))
            {
                List<Map> maps = Find.Maps;
                for (int i = 0; i < maps.Count; i++)
                {
                    maps[i].designationManager.RemoveAllDesignationsOn(item, false);
                }
            }
            CompTransporter compTransporter = this.owner as CompTransporter;
            if (compTransporter != null)
            {
                compTransporter.Notify_ThingAdded(item);
            }
            Caravan caravan = this.owner as Caravan;
            if (caravan != null)
            {
                caravan.Notify_PawnAdded((Pawn)item);
            }
            Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
            if (pawn_ApparelTracker != null)
            {
                pawn_ApparelTracker.Notify_ApparelAdded((Apparel)item);
            }
            Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
            if (pawn_EquipmentTracker != null)
            {
                pawn_EquipmentTracker.Notify_EquipmentAdded((ThingWithComps)item);
            }
            this.NotifyColonistBarIfColonistCorpse(item);
        }

        // Token: 0x060016C4 RID: 5828 RVA: 0x00083478 File Offset: 0x00081678
        protected void NotifyAddedAndMergedWith(Thing item, int mergedCount)
        {
            CompTransporter compTransporter = this.owner as CompTransporter;
            if (compTransporter != null)
            {
                compTransporter.Notify_ThingAddedAndMergedWith(item, mergedCount);
            }
        }

        // Token: 0x060016C5 RID: 5829 RVA: 0x0008349C File Offset: 0x0008169C
        protected void NotifyRemoved(Thing item)
        {
            Pawn_InventoryTracker pawn_InventoryTracker = this.owner as Pawn_InventoryTracker;
            if (pawn_InventoryTracker != null)
            {
                pawn_InventoryTracker.Notify_ItemRemoved(item);
            }
            Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
            if (pawn_ApparelTracker != null)
            {
                pawn_ApparelTracker.Notify_ApparelRemoved((Apparel)item);
            }
            Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
            if (pawn_EquipmentTracker != null)
            {
                pawn_EquipmentTracker.Notify_EquipmentRemoved((ThingWithComps)item);
            }
            Caravan caravan = this.owner as Caravan;
            if (caravan != null)
            {
                caravan.Notify_PawnRemoved((Pawn)item);
            }
            this.NotifyColonistBarIfColonistCorpse(item);
        }

        // Token: 0x060016C6 RID: 5830 RVA: 0x00083518 File Offset: 0x00081718
        private void NotifyColonistBarIfColonistCorpse(Thing thing)
        {
            Corpse corpse = thing as Corpse;
            if (corpse != null && !corpse.Bugged && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
            {
                Find.ColonistBar.MarkColonistsDirty();
            }
        }

        // Token: 0x060016C7 RID: 5831 RVA: 0x000828A6 File Offset: 0x00080AA6
        void IList<Thing>.Insert(int index, Thing item)
        {
            throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
        }

        // Token: 0x060016C8 RID: 5832 RVA: 0x00083568 File Offset: 0x00081768
        void ICollection<Thing>.Add(Thing item)
        {
            this.TryAdd(item, true);
        }

        // Token: 0x060016C9 RID: 5833 RVA: 0x00083574 File Offset: 0x00081774
        void ICollection<Thing>.CopyTo(Thing[] array, int arrayIndex)
        {
            for (int i = 0; i < this.Count; i++)
            {
                array[i + arrayIndex] = this.GetAt(i);
            }
        }

        // Token: 0x060016CA RID: 5834 RVA: 0x0008359E File Offset: 0x0008179E
        IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
        {
            int num;
            for (int i = 0; i < this.Count; i = num + 1)
            {
                yield return this.GetAt(i);
                num = i;
            }
            yield break;
        }

        // Token: 0x060016CB RID: 5835 RVA: 0x000835AD File Offset: 0x000817AD
        IEnumerator IEnumerable.GetEnumerator()
        {
            int num;
            for (int i = 0; i < this.Count; i = num + 1)
            {
                yield return this.GetAt(i);
                num = i;
            }
            yield break;
        }

        // Token: 0x04000E8A RID: 3722
        protected IThingHolder owner;

        // Token: 0x04000E8B RID: 3723
        protected int maxStacks = 999999;

        // Token: 0x04000E8C RID: 3724
        public LookMode contentsLookMode = LookMode.Deep;

        // Token: 0x04000E8D RID: 3725
        private const int InfMaxStacks = 999999;
    }

    // Token: 0x02000312 RID: 786
    public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Thing
    {
        // Token: 0x170004AD RID: 1197
        // (get) Token: 0x0600166F RID: 5743 RVA: 0x0008217D File Offset: 0x0008037D
        public List<T> InnerListForReading
        {
            get
            {
                return this.innerList;
            }
        }

        // Token: 0x170004AE RID: 1198
        public new T this[int index]
        {
            get
            {
                return this.innerList[index];
            }
        }

        // Token: 0x170004AF RID: 1199
        // (get) Token: 0x06001671 RID: 5745 RVA: 0x00082193 File Offset: 0x00080393
        public override int Count
        {
            get
            {
                return this.innerList.Count;
            }
        }

        // Token: 0x170004B0 RID: 1200
        T IList<T>.this[int index]
        {
            get
            {
                return this.innerList[index];
            }
            set
            {
                throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
            }
        }

        // Token: 0x170004B1 RID: 1201
        // (get) Token: 0x06001674 RID: 5748 RVA: 0x0001028D File Offset: 0x0000E48D
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        // Token: 0x06001675 RID: 5749 RVA: 0x000821AC File Offset: 0x000803AC
        public ThingOwner()
        {
        }

        // Token: 0x06001676 RID: 5750 RVA: 0x000821BF File Offset: 0x000803BF
        public ThingOwner(IThingHolder owner) : base(owner)
        {
        }

        // Token: 0x06001677 RID: 5751 RVA: 0x000821D3 File Offset: 0x000803D3
        public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
        {
        }

        // Token: 0x06001678 RID: 5752 RVA: 0x000821EC File Offset: 0x000803EC
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<T>(ref this.innerList, true, "innerList", this.contentsLookMode, Array.Empty<object>());
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                this.innerList.RemoveAll((T x) => x == null);
            }
            if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                for (int i = 0; i < this.innerList.Count; i++)
                {
                    if (this.innerList[i] != null)
                    {
                        this.innerList[i].holdingOwner = this;
                    }
                }
            }
        }

        // Token: 0x06001679 RID: 5753 RVA: 0x0008229E File Offset: 0x0008049E
        public List<T>.Enumerator GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }

        // Token: 0x0600167A RID: 5754 RVA: 0x000822AB File Offset: 0x000804AB
        public override int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
        {
            if (!(item is T))
            {
                return 0;
            }
            return base.GetCountCanAccept(item, canMergeWithExistingStacks);
        }

        // Token: 0x0600167B RID: 5755 RVA: 0x000822C0 File Offset: 0x000804C0
        public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
        {
            if (count <= 0)
            {
                return 0;
            }
            if (item == null)
            {
                Log.Warning("Tried to add null item to ThingOwner.", false);
                return 0;
            }
            if (base.Contains(item))
            {
                Log.Warning("Tried to add " + item + " to ThingOwner but this item is already here.", false);
                return 0;
            }
            if (item.holdingOwner != null)
            {
                Log.Warning(string.Concat(new object[]
                {
                    "Tried to add ",
                    count,
                    " of ",
                    item.ToStringSafe<Thing>(),
                    " to ThingOwner but this thing is already in another container. owner=",
                    this.owner.ToStringSafe<IThingHolder>(),
                    ", current container owner=",
                    item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
                    ". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
                }), false);
                return 0;
            }
            if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
            {
                return 0;
            }
            int stackCount = item.stackCount;
            int num = Mathf.Min(stackCount, count);
            Thing thing = item.SplitOff(num);
            if (this.TryAdd((T)((object)thing), canMergeWithExistingStacks))
            {
                return num;
            }
            if (thing != item)
            {
                int result = stackCount - item.stackCount - thing.stackCount;
                item.TryAbsorbStack(thing, false);
                return result;
            }
            return stackCount - item.stackCount;
        }

        // Token: 0x0600167C RID: 5756 RVA: 0x000823DC File Offset: 0x000805DC
        public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
        {
            if (item == null)
            {
                Log.Warning("Tried to add null item to ThingOwner.", false);
                return false;
            }
            T t = item as T;
            if (t == null)
            {
                return false;
            }
            if (base.Contains(item))
            {
                Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.", false);
                return false;
            }
            if (item.holdingOwner != null)
            {
                Log.Warning(string.Concat(new string[]
                {
                    "Tried to add ",
                    item.ToStringSafe<Thing>(),
                    " to ThingOwner but this thing is already in another container. owner=",
                    this.owner.ToStringSafe<IThingHolder>(),
                    ", current container owner=",
                    item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
                    ". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
                }), false);
                return false;
            }
            if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
            {
                return false;
            }
            if (canMergeWithExistingStacks)
            {
                for (int i = 0; i < this.innerList.Count; i++)
                {
                    T t2 = this.innerList[i];
                    if (t2.CanStackWith(item))
                    {
                        int num = Mathf.Min(item.stackCount, t2.def.stackLimit - t2.stackCount);
                        if (num > 0)
                        {
                            Thing other = item.SplitOff(num);
                            int stackCount = t2.stackCount;
                            t2.TryAbsorbStack(other, true);
                            if (t2.stackCount > stackCount)
                            {
                                base.NotifyAddedAndMergedWith(t2, t2.stackCount - stackCount);
                            }
                            if (item.Destroyed || item.stackCount == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            if (this.Count >= this.maxStacks)
            {
                return false;
            }
            item.holdingOwner = this;
            this.innerList.Add(t);
            base.NotifyAdded(t);
            return true;
        }

        // Token: 0x0600167D RID: 5757 RVA: 0x000825A4 File Offset: 0x000807A4
        public void TryAddRangeOrTransfer(IEnumerable<T> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
        {
            if (things == this)
            {
                return;
            }
            ThingOwner thingOwner = things as ThingOwner;
            if (thingOwner != null)
            {
                thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
                if (destroyLeftover)
                {
                    thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
                    return;
                }
            }
            else
            {
                IList<T> list = things as IList<T>;
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!base.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
                        {
                            list[i].Destroy(DestroyMode.Vanish);
                        }
                    }
                    return;
                }
                foreach (T t in things)
                {
                    if (!base.TryAddOrTransfer(t, canMergeWithExistingStacks) && destroyLeftover)
                    {
                        t.Destroy(DestroyMode.Vanish);
                    }
                }
            }
        }

        // Token: 0x0600167E RID: 5758 RVA: 0x00082678 File Offset: 0x00080878
        public override int IndexOf(Thing item)
        {
            T t = item as T;
            if (t == null)
            {
                return -1;
            }
            return this.innerList.IndexOf(t);
        }

        // Token: 0x0600167F RID: 5759 RVA: 0x000826A8 File Offset: 0x000808A8
        public override bool Remove(Thing item)
        {
            if (!base.Contains(item))
            {
                return false;
            }
            if (item.holdingOwner == this)
            {
                item.holdingOwner = null;
            }
            int index = this.innerList.LastIndexOf((T)((object)item));
            this.innerList.RemoveAt(index);
            base.NotifyRemoved(item);
            return true;
        }

        // Token: 0x06001680 RID: 5760 RVA: 0x000826F8 File Offset: 0x000808F8
        public int RemoveAll(Predicate<T> predicate)
        {
            int num = 0;
            for (int i = this.innerList.Count - 1; i >= 0; i--)
            {
                if (predicate(this.innerList[i]))
                {
                    this.Remove(this.innerList[i]);
                    num++;
                }
            }
            return num;
        }

        // Token: 0x06001681 RID: 5761 RVA: 0x00082750 File Offset: 0x00080950
        protected override Thing GetAt(int index)
        {
            return this.innerList[index];
        }

        // Token: 0x06001682 RID: 5762 RVA: 0x00082764 File Offset: 0x00080964
        public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
        {
            Thing thing;
            int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
            resultingTransferredItem = (T)((object)thing);
            return result;
        }

        // Token: 0x06001683 RID: 5763 RVA: 0x0008278B File Offset: 0x0008098B
        public new T Take(Thing thing, int count)
        {
            return (T)((object)base.Take(thing, count));
        }

        // Token: 0x06001684 RID: 5764 RVA: 0x0008279A File Offset: 0x0008099A
        public new T Take(Thing thing)
        {
            return (T)((object)base.Take(thing));
        }

        // Token: 0x06001685 RID: 5765 RVA: 0x000827A8 File Offset: 0x000809A8
        public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out T resultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            Action<Thing, int> placedAction2 = null;
            if (placedAction != null)
            {
                placedAction2 = delegate (Thing t, int c)
                {
                    placedAction((T)((object)t), c);
                };
            }
            Thing thing2;
            bool result = base.TryDrop(thing, dropLoc, map, mode, count, out thing2, placedAction2, nearPlaceValidator);
            resultingThing = (T)((object)thing2);
            return result;
        }

        // Token: 0x06001686 RID: 5766 RVA: 0x000827FC File Offset: 0x000809FC
        public bool TryDrop(Thing thing, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            Action<Thing, int> placedAction2 = null;
            if (placedAction != null)
            {
                placedAction2 = delegate (Thing t, int c)
                {
                    placedAction((T)((object)t), c);
                };
            }
            Thing thing2;
            bool result = base.TryDrop(thing, mode, out thing2, placedAction2, nearPlaceValidator);
            lastResultingThing = (T)((object)thing2);
            return result;
        }

        // Token: 0x06001687 RID: 5767 RVA: 0x00082848 File Offset: 0x00080A48
        public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
        {
            Action<Thing, int> placedAction2 = null;
            if (placedAction != null)
            {
                placedAction2 = delegate (Thing t, int c)
                {
                    placedAction((T)((object)t), c);
                };
            }
            Thing thing2;
            bool result = base.TryDrop_NewTmp(thing, dropLoc, map, mode, out thing2, placedAction2, nearPlaceValidator, true);
            lastResultingThing = (T)((object)thing2);
            return result;
        }

        // Token: 0x06001688 RID: 5768 RVA: 0x00082898 File Offset: 0x00080A98
        int IList<T>.IndexOf(T item)
        {
            return this.innerList.IndexOf(item);
        }

        // Token: 0x06001689 RID: 5769 RVA: 0x000828A6 File Offset: 0x00080AA6
        void IList<T>.Insert(int index, T item)
        {
            throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
        }

        // Token: 0x0600168A RID: 5770 RVA: 0x000828B2 File Offset: 0x00080AB2
        void ICollection<T>.Add(T item)
        {
            this.TryAdd(item, true);
        }

        // Token: 0x0600168B RID: 5771 RVA: 0x000828C2 File Offset: 0x00080AC2
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.innerList.CopyTo(array, arrayIndex);
        }

        // Token: 0x0600168C RID: 5772 RVA: 0x000828D1 File Offset: 0x00080AD1
        bool ICollection<T>.Contains(T item)
        {
            return this.innerList.Contains(item);
        }

        // Token: 0x0600168D RID: 5773 RVA: 0x000828DF File Offset: 0x00080ADF
        bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }

        // Token: 0x0600168E RID: 5774 RVA: 0x000828ED File Offset: 0x00080AED
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }

        // Token: 0x0600168F RID: 5775 RVA: 0x000828ED File Offset: 0x00080AED
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }

        // Token: 0x04000E89 RID: 3721
        private List<T> innerList = new List<T>();
    }
}
