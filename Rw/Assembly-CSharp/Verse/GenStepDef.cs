using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B2 RID: 178
	public class GenStepDef : Def
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x0001B3CE File Offset: 0x000195CE
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x040003A6 RID: 934
		public SitePartDef linkWithSite;

		// Token: 0x040003A7 RID: 935
		public float order;

		// Token: 0x040003A8 RID: 936
		public GenStep genStep;
	}
}
