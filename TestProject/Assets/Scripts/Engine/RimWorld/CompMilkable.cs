using System;
using Verse;

namespace RimWorld
{
	
	public class CompMilkable : CompHasGatherableBodyResource
	{
		
		// (get) Token: 0x060051F5 RID: 20981 RVA: 0x001B6528 File Offset: 0x001B4728
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		
		// (get) Token: 0x060051F6 RID: 20982 RVA: 0x001B6535 File Offset: 0x001B4735
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		
		// (get) Token: 0x060051F7 RID: 20983 RVA: 0x001B6542 File Offset: 0x001B4742
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		
		// (get) Token: 0x060051F8 RID: 20984 RVA: 0x001B654F File Offset: 0x001B474F
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		
		// (get) Token: 0x060051F9 RID: 20985 RVA: 0x001B6556 File Offset: 0x001B4756
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		
		// (get) Token: 0x060051FA RID: 20986 RVA: 0x001B6564 File Offset: 0x001B4764
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.milkFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "MilkFullness".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
