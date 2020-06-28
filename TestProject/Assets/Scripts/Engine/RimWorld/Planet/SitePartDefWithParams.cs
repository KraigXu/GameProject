using System;

namespace RimWorld.Planet
{
	// Token: 0x0200125E RID: 4702
	public class SitePartDefWithParams
	{
		// Token: 0x06006E11 RID: 28177 RVA: 0x0026731B File Offset: 0x0026551B
		public SitePartDefWithParams(SitePartDef def, SitePartParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x04004408 RID: 17416
		public SitePartDef def;

		// Token: 0x04004409 RID: 17417
		public SitePartParams parms;
	}
}
