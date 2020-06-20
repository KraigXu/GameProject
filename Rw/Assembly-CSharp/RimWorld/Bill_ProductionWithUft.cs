using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C88 RID: 3208
	public class Bill_ProductionWithUft : Bill_Production
	{
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06004D3F RID: 19775 RVA: 0x0019E394 File Offset: 0x0019C594
		protected override string StatusString
		{
			get
			{
				if (this.BoundWorker == null)
				{
					return (base.StatusString ?? "").Trim();
				}
				return ("BoundWorkerIs".Translate(this.BoundWorker.LabelShort, this.BoundWorker) + base.StatusString).Trim();
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06004D40 RID: 19776 RVA: 0x0019E3FC File Offset: 0x0019C5FC
		public Pawn BoundWorker
		{
			get
			{
				if (this.boundUftInt == null)
				{
					return null;
				}
				Pawn creator = this.boundUftInt.Creator;
				if (creator == null || creator.Downed || creator.HostFaction != null || creator.Destroyed || !creator.Spawned)
				{
					this.boundUftInt = null;
					return null;
				}
				Thing thing = this.billStack.billGiver as Thing;
				if (thing != null)
				{
					WorkTypeDef workTypeDef = null;
					List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].fixedBillGiverDefs != null && allDefsListForReading[i].fixedBillGiverDefs.Contains(thing.def))
						{
							workTypeDef = allDefsListForReading[i].workType;
							break;
						}
					}
					if (workTypeDef != null && !creator.workSettings.WorkIsActive(workTypeDef))
					{
						this.boundUftInt = null;
						return null;
					}
				}
				return creator;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004D41 RID: 19777 RVA: 0x0019E4D0 File Offset: 0x0019C6D0
		public UnfinishedThing BoundUft
		{
			get
			{
				return this.boundUftInt;
			}
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x0019E4D8 File Offset: 0x0019C6D8
		public void SetBoundUft(UnfinishedThing value, bool setOtherLink = true)
		{
			if (value == this.boundUftInt)
			{
				return;
			}
			UnfinishedThing unfinishedThing = this.boundUftInt;
			this.boundUftInt = value;
			if (setOtherLink)
			{
				if (unfinishedThing != null && unfinishedThing.BoundBill == this)
				{
					unfinishedThing.BoundBill = null;
				}
				if (value != null && value.BoundBill != this)
				{
					this.boundUftInt.BoundBill = this;
				}
			}
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0019E52B File Offset: 0x0019C72B
		public Bill_ProductionWithUft()
		{
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x0019E533 File Offset: 0x0019C733
		public Bill_ProductionWithUft(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0019E53C File Offset: 0x0019C73C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnfinishedThing>(ref this.boundUftInt, "boundUft", false);
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0019E555 File Offset: 0x0019C755
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			this.ClearBoundUft();
			base.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0019E565 File Offset: 0x0019C765
		public void ClearBoundUft()
		{
			this.boundUftInt = null;
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0019E56E File Offset: 0x0019C76E
		public override Bill Clone()
		{
			return (Bill_Production)base.Clone();
		}

		// Token: 0x04002B39 RID: 11065
		private UnfinishedThing boundUftInt;
	}
}
