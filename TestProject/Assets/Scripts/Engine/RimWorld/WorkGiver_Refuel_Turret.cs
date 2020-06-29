using System;
using Verse;

namespace RimWorld
{
	
	public class WorkGiver_Refuel_Turret : WorkGiver_Refuel
	{
		
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x001130D5 File Offset: 0x001112D5
		public override JobDef JobStandard
		{
			get
			{
				return JobDefOf.RearmTurret;
			}
		}

		
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x001130DC File Offset: 0x001112DC
		public override JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RearmTurretAtomic;
			}
		}

		
		public override bool CanRefuelThing(Thing t)
		{
			return t is Building_Turret;
		}
	}
}
