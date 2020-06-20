using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4E RID: 3150
	public static class InstallBlueprintUtility
	{
		// Token: 0x06004B33 RID: 19251 RVA: 0x00195AF8 File Offset: 0x00193CF8
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x00195B18 File Offset: 0x00193D18
		public static Blueprint_Install ExistingBlueprintFor(Thing th)
		{
			ThingDef installBlueprintDef = th.GetInnerIfMinified().def.installBlueprintDef;
			if (installBlueprintDef == null)
			{
				return null;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsMatching(ThingRequest.ForDef(installBlueprintDef));
				for (int j = 0; j < list.Count; j++)
				{
					Blueprint_Install blueprint_Install = list[j] as Blueprint_Install;
					if (blueprint_Install != null && blueprint_Install.MiniToInstallOrBuildingToReinstall == th)
					{
						return blueprint_Install;
					}
				}
			}
			return null;
		}
	}
}
