using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C89 RID: 3209
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06004D49 RID: 19785 RVA: 0x0019E57B File Offset: 0x0019C77B
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06004D4A RID: 19786 RVA: 0x0019E5A1 File Offset: 0x0019C7A1
		public bool CanWorkWithoutFuel
		{
			get
			{
				return this.refuelableComp == null;
			}
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0019E5AC File Offset: 0x0019C7AC
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x0019E5C0 File Offset: 0x0019C7C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0019E5E4 File Offset: 0x0019C7E4
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

		// Token: 0x06004D4E RID: 19790 RVA: 0x0019E65C File Offset: 0x0019C85C
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06004D4F RID: 19791 RVA: 0x0019E671 File Offset: 0x0019C871
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004D50 RID: 19792 RVA: 0x0019E679 File Offset: 0x0019C879
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06004D51 RID: 19793 RVA: 0x0019E681 File Offset: 0x0019C881
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x0019E68C File Offset: 0x0019C88C
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.CanWorkWithoutFuel || (this.refuelableComp != null && this.refuelableComp.HasFuel));
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0019E6E2 File Offset: 0x0019C8E2
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}

		// Token: 0x04002B3A RID: 11066
		public BillStack billStack;

		// Token: 0x04002B3B RID: 11067
		private CompPowerTrader powerComp;

		// Token: 0x04002B3C RID: 11068
		private CompRefuelable refuelableComp;

		// Token: 0x04002B3D RID: 11069
		private CompBreakdownable breakdownableComp;
	}
}
