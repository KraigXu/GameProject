using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4F RID: 3151
	public interface IConstructible
	{
		// Token: 0x06004B35 RID: 19253
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x06004B36 RID: 19254
		ThingDef EntityToBuildStuff();
	}
}
