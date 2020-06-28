using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C49 RID: 3145
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x06004B06 RID: 19206 RVA: 0x00195400 File Offset: 0x00193600
		public static Designator_Install DesignatorFor(ThingDef artDef)
		{
			Designator_Install designator_Install;
			if (InstallationDesignatorDatabase.designators.TryGetValue(artDef, out designator_Install))
			{
				return designator_Install;
			}
			designator_Install = InstallationDesignatorDatabase.NewDesignatorFor(artDef);
			InstallationDesignatorDatabase.designators.Add(artDef, designator_Install);
			return designator_Install;
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x00195432 File Offset: 0x00193632
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			return new Designator_Install
			{
				hotKey = KeyBindingDefOf.Misc1
			};
		}

		// Token: 0x04002A7E RID: 10878
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();
	}
}
