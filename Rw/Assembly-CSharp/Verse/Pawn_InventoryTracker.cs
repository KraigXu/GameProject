using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200029F RID: 671
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x0006EB82 File Offset: 0x0006CD82
		// (set) Token: 0x06001336 RID: 4918 RVA: 0x0006EB94 File Offset: 0x0006CD94
		public bool UnloadEverything
		{
			get
			{
				return this.unloadEverything && this.HasAnyUnloadableThing;
			}
			set
			{
				if (value && this.HasAnyUnloadableThing)
				{
					this.unloadEverything = true;
					return;
				}
				this.unloadEverything = false;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0006EBB0 File Offset: 0x0006CDB0
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x0006EBD4 File Offset: 0x0006CDD4
		public ThingCount FirstUnloadableThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return default(ThingCount);
				}
				if (this.pawn.drugs != null && this.pawn.drugs.CurrentPolicy != null)
				{
					DrugPolicy currentPolicy = this.pawn.drugs.CurrentPolicy;
					Pawn_InventoryTracker.tmpDrugsToKeep.Clear();
					for (int i = 0; i < currentPolicy.Count; i++)
					{
						if (currentPolicy[i].takeToInventory > 0)
						{
							Pawn_InventoryTracker.tmpDrugsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
						}
					}
					for (int j = 0; j < this.innerContainer.Count; j++)
					{
						if (!this.innerContainer[j].def.IsDrug)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						int num = -1;
						for (int k = 0; k < Pawn_InventoryTracker.tmpDrugsToKeep.Count; k++)
						{
							if (this.innerContainer[j].def == Pawn_InventoryTracker.tmpDrugsToKeep[k].ThingDef)
							{
								num = k;
								break;
							}
						}
						if (num < 0)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						if (this.innerContainer[j].stackCount > Pawn_InventoryTracker.tmpDrugsToKeep[num].Count)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount - Pawn_InventoryTracker.tmpDrugsToKeep[num].Count);
						}
						Pawn_InventoryTracker.tmpDrugsToKeep[num] = new ThingDefCount(Pawn_InventoryTracker.tmpDrugsToKeep[num].ThingDef, Pawn_InventoryTracker.tmpDrugsToKeep[num].Count - this.innerContainer[j].stackCount);
					}
					return default(ThingCount);
				}
				return new ThingCount(this.innerContainer[0], this.innerContainer[0].stackCount);
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x0006EE22 File Offset: 0x0006D022
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0006EE2A File Offset: 0x0006D02A
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0006EE54 File Offset: 0x0006D054
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0006EEA3 File Offset: 0x0006D0A3
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0006EEC8 File Offset: 0x0006D0C8
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0006EED8 File Offset: 0x0006D0D8
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop all inventory near pawn but the pawn is unspawned. pawn=" + this.pawn, false);
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			Action<Thing, int> <>9__0;
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				ThingOwner<Thing> thingOwner = this.innerContainer;
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				Map mapHeld = this.pawn.MapHeld;
				ThingPlaceMode mode = ThingPlaceMode.Near;
				Action<Thing, int> placedAction;
				if ((placedAction = <>9__0) == null)
				{
					placedAction = (<>9__0 = delegate(Thing t, int unused)
					{
						if (forbid)
						{
							t.SetForbiddenIfOutsideHomeArea();
						}
						if (unforbid)
						{
							t.SetForbidden(false, false);
						}
						if (t.def.IsPleasureDrug)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
						}
					});
				}
				Thing thing2;
				thingOwner.TryDrop(thing, pos, mapHeld, mode, out thing2, placedAction, null);
			}
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0006EF92 File Offset: 0x0006D192
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0006EFA0 File Offset: 0x0006D1A0
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0006EFAE File Offset: 0x0006D1AE
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0006EFBC File Offset: 0x0006D1BC
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0006EFD9 File Offset: 0x0006D1D9
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0006EFFF File Offset: 0x0006D1FF
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0006F007 File Offset: 0x0006D207
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04000D08 RID: 3336
		public Pawn pawn;

		// Token: 0x04000D09 RID: 3337
		public ThingOwner<Thing> innerContainer;

		// Token: 0x04000D0A RID: 3338
		private bool unloadEverything;

		// Token: 0x04000D0B RID: 3339
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x04000D0C RID: 3340
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		// Token: 0x04000D0D RID: 3341
		private static List<Thing> tmpThingList = new List<Thing>();
	}
}
