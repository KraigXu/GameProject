using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_Uninstall : JobDriver_RemoveBuilding
	{
		
		// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x000FB6CA File Offset: 0x000F98CA
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		
		// (get) Token: 0x06002BA2 RID: 11170 RVA: 0x000FB6D4 File Offset: 0x000F98D4
		protected override float TotalNeededWork
		{
			get
			{
				return base.TargetA.Thing.def.building.uninstallWork;
			}
		}

		
		protected override void FinishedRemoving()
		{
			base.Building.Uninstall();
			this.pawn.records.Increment(RecordDefOf.ThingsUninstalled);
		}
	}
}
