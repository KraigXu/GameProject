using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000639 RID: 1593
	public class JobDriver_Uninstall : JobDriver_RemoveBuilding
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x000FB6CA File Offset: 0x000F98CA
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002BA2 RID: 11170 RVA: 0x000FB6D4 File Offset: 0x000F98D4
		protected override float TotalNeededWork
		{
			get
			{
				return base.TargetA.Thing.def.building.uninstallWork;
			}
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000FB6FE File Offset: 0x000F98FE
		protected override void FinishedRemoving()
		{
			base.Building.Uninstall();
			this.pawn.records.Increment(RecordDefOf.ThingsUninstalled);
		}
	}
}
