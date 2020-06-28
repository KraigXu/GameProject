using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000757 RID: 1879
	public class WorkGiver_Refuel_Turret : WorkGiver_Refuel
	{
		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x001130D5 File Offset: 0x001112D5
		public override JobDef JobStandard
		{
			get
			{
				return JobDefOf.RearmTurret;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x001130DC File Offset: 0x001112DC
		public override JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RearmTurretAtomic;
			}
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x001130E3 File Offset: 0x001112E3
		public override bool CanRefuelThing(Thing t)
		{
			return t is Building_Turret;
		}
	}
}
