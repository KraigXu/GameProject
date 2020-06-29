using System;

namespace RimWorld.Planet
{
	
	public class SitePartDefWithParams
	{
		
		public SitePartDefWithParams(SitePartDef def, SitePartParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		
		public SitePartDef def;

		
		public SitePartParams parms;
	}
}
