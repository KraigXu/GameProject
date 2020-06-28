using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200029B RID: 667
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0006C558 File Offset: 0x0006A758
		public Thing CarriedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0006C575 File Offset: 0x0006A775
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x0006C58E File Offset: 0x0006A78E
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0006C596 File Offset: 0x0006A796
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0006C5B3 File Offset: 0x0006A7B3
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0006C5CF File Offset: 0x0006A7CF
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0006C5D7 File Offset: 0x0006A7D7
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0006C5E8 File Offset: 0x0006A7E8
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0006C614 File Offset: 0x0006A814
		public int MaxStackSpaceEver(ThingDef td)
		{
			int b = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / td.VolumePerUnit);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0006C64C File Offset: 0x0006A84C
		public bool TryStartCarry(Thing item)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error("Dead/downed pawn " + this.pawn + " tried to start carry item.", false);
				return false;
			}
			if (this.innerContainer.TryAdd(item, true))
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				return true;
			}
			return false;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0006C6D0 File Offset: 0x0006A8D0
		public int TryStartCarry(Thing item, int count, bool reserve = true)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Dead/downed pawn ",
					this.pawn,
					" tried to start carry ",
					item.ToStringSafe<Thing>()
				}), false);
				return 0;
			}
			count = Mathf.Min(count, this.AvailableStackSpace(item.def));
			count = Mathf.Min(count, item.stackCount);
			int num = this.innerContainer.TryAdd(item.SplitOff(count), count, true);
			if (num > 0)
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				if (reserve)
				{
					this.pawn.Reserve(this.CarriedThing, this.pawn.CurJob, 1, -1, null, true);
				}
			}
			return num;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0006C7BC File Offset: 0x0006A9BC
		public bool TryDropCarriedThing(IntVec3 dropLoc, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0006C814 File Offset: 0x0006AA14
		public bool TryDropCarriedThing(IntVec3 dropLoc, int count, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, count, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0006C870 File Offset: 0x0006AA70
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0006C87E File Offset: 0x0006AA7E
		public void CarryHandsTick()
		{
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x04000CF5 RID: 3317
		public Pawn pawn;

		// Token: 0x04000CF6 RID: 3318
		public ThingOwner<Thing> innerContainer;
	}
}
