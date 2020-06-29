using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		
		// (get) Token: 0x06004D49 RID: 19785 RVA: 0x0019E57B File Offset: 0x0019C77B
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		
		// (get) Token: 0x06004D4A RID: 19786 RVA: 0x0019E5A1 File Offset: 0x0019C7A1
		public bool CanWorkWithoutFuel
		{
			get
			{
				return this.refuelableComp == null;
			}
		}

		
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.breakdownableComp = base.GetComp<CompBreakdownable>();
			foreach (Bill bill in this.billStack)
			{
				bill.ValidateSettings();
			}
		}

		
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		
		// (get) Token: 0x06004D4F RID: 19791 RVA: 0x0019E671 File Offset: 0x0019C871
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		
		// (get) Token: 0x06004D50 RID: 19792 RVA: 0x0019E679 File Offset: 0x0019C879
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		
		// (get) Token: 0x06004D51 RID: 19793 RVA: 0x0019E681 File Offset: 0x0019C881
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.CanWorkWithoutFuel || (this.refuelableComp != null && this.refuelableComp.HasFuel));
		}

		
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}

		
		public BillStack billStack;

		
		private CompPowerTrader powerComp;

		
		private CompRefuelable refuelableComp;

		
		private CompBreakdownable breakdownableComp;
	}
}
