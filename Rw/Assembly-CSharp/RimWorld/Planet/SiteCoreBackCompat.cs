using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200125C RID: 4700
	public class SiteCoreBackCompat : IExposable
	{
		// Token: 0x06006DE9 RID: 28137 RVA: 0x002667DF File Offset: 0x002649DF
		public void ExposeData()
		{
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x040043FB RID: 17403
		public SitePartDef def;

		// Token: 0x040043FC RID: 17404
		public SitePartParams parms;
	}
}
