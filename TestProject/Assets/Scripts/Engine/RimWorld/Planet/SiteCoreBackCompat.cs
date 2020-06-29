using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class SiteCoreBackCompat : IExposable
	{
		
		public void ExposeData()
		{
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
		}

		
		public SitePartDef def;

		
		public SitePartParams parms;
	}
}
