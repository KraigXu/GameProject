using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE8 RID: 3304
	public class CompProperties_AssignableToPawn : CompProperties
	{
		// Token: 0x0600504F RID: 20559 RVA: 0x001B0D37 File Offset: 0x001AEF37
		public CompProperties_AssignableToPawn()
		{
			this.compClass = typeof(CompAssignableToPawn);
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x0012283F File Offset: 0x00120A3F
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x001B0D64 File Offset: 0x001AEF64
		public override void PostLoadSpecial(ThingDef parent)
		{
			if (parent.thingClass == typeof(Building_Bed))
			{
				this.maxAssignedPawnsCount = BedUtility.GetSleepingSlotsCount(parent.size);
			}
		}

		// Token: 0x04002CC2 RID: 11458
		public int maxAssignedPawnsCount = 1;

		// Token: 0x04002CC3 RID: 11459
		public bool drawAssignmentOverlay = true;

		// Token: 0x04002CC4 RID: 11460
		public bool drawUnownedAssignmentOverlay = true;

		// Token: 0x04002CC5 RID: 11461
		public string singleton;
	}
}
