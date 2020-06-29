using System;
using Verse;

namespace RimWorld
{
	
	public class CompShearable : CompHasGatherableBodyResource
	{
		
		// (get) Token: 0x060052DF RID: 21215 RVA: 0x001BADD9 File Offset: 0x001B8FD9
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		
		// (get) Token: 0x060052E0 RID: 21216 RVA: 0x001BADE6 File Offset: 0x001B8FE6
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		
		// (get) Token: 0x060052E1 RID: 21217 RVA: 0x001BADF3 File Offset: 0x001B8FF3
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		
		// (get) Token: 0x060052E2 RID: 21218 RVA: 0x001BAE00 File Offset: 0x001B9000
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		
		// (get) Token: 0x060052E3 RID: 21219 RVA: 0x001BAE07 File Offset: 0x001B9007
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		
		// (get) Token: 0x060052E4 RID: 21220 RVA: 0x001BAE14 File Offset: 0x001B9014
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return pawn == null || pawn.ageTracker.CurLifeStage.shearable;
			}
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
