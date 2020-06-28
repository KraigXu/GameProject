using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1C RID: 3356
	public class CompMaintainable : ThingComp
	{
		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x060051A8 RID: 20904 RVA: 0x001B59AA File Offset: 0x001B3BAA
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001B59B7 File Offset: 0x001B3BB7
		public MaintainableStage CurStage
		{
			get
			{
				if (this.ticksSinceMaintain < this.Props.ticksHealthy)
				{
					return MaintainableStage.Healthy;
				}
				if (this.ticksSinceMaintain < this.Props.ticksHealthy + this.Props.ticksNeedsMaintenance)
				{
					return MaintainableStage.NeedsMaintenance;
				}
				return MaintainableStage.Damaging;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x060051AA RID: 20906 RVA: 0x001B59F0 File Offset: 0x001B3BF0
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.CompDormant.Awake;
			}
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001B5A19 File Offset: 0x001B3C19
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x001B5A2D File Offset: 0x001B3C2D
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain++;
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x001B5A64 File Offset: 0x001B3C64
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain += 250;
			this.CheckTakeDamage();
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x001B5A90 File Offset: 0x001B3C90
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x001B5AD7 File Offset: 0x001B3CD7
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001B5AE0 File Offset: 0x001B3CE0
		public override string CompInspectStringExtra()
		{
			MaintainableStage curStage = this.CurStage;
			if (curStage == MaintainableStage.NeedsMaintenance)
			{
				return "DueForMaintenance".Translate();
			}
			if (curStage != MaintainableStage.Damaging)
			{
				return null;
			}
			return "DeterioratingDueToLackOfMaintenance".Translate();
		}

		// Token: 0x04002D21 RID: 11553
		public int ticksSinceMaintain;
	}
}
