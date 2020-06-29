using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		
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

		
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0006EBB0 File Offset: 0x0006CDB0
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		
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

		
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x0006EE22 File Offset: 0x0006D022
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop all inventory near pawn but the pawn is unspawned. pawn=" + this.pawn, false);
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				ThingOwner<Thing> thingOwner = this.innerContainer;
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				Map mapHeld = this.pawn.MapHeld;
				ThingPlaceMode mode = ThingPlaceMode.Near;
				Action<Thing, int> placedAction=null;
				if (placedAction == null)
				{
					placedAction = delegate(Thing t, int unused)
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
					};
				}
				Thing thing2;
				thingOwner.TryDrop(thing, pos, mapHeld, mode, out thing2, placedAction, null);
			}
		}

		
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		
		public Pawn pawn;

		
		public ThingOwner<Thing> innerContainer;

		
		private bool unloadEverything;

		
		private List<Thing> itemsNotForSale = new List<Thing>();

		
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		
		private static List<Thing> tmpThingList = new List<Thing>();
	}
}
