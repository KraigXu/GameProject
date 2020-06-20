using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000353 RID: 851
	public static class DebugTools_MapGen
	{
		// Token: 0x06001A02 RID: 6658 RVA: 0x0009FDBC File Offset: 0x0009DFBC
		public static List<DebugMenuOption> Options_Scatterers()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localSt2 in typeof(GenStep_Scatterer).AllLeafSubclasses())
			{
				Type localSt = localSt2;
				list.Add(new DebugMenuOption(localSt.ToString(), DebugMenuOptionMode.Tool, delegate
				{
					((GenStep_Scatterer)Activator.CreateInstance(localSt)).ForceScatterAt(UI.MouseCell(), Find.CurrentMap);
				}));
			}
			return list;
		}
	}
}
