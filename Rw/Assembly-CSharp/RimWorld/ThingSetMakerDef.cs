using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090F RID: 2319
	public class ThingSetMakerDef : Def
	{
		// Token: 0x0600371D RID: 14109 RVA: 0x00128EFE File Offset: 0x001270FE
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}

		// Token: 0x04002033 RID: 8243
		public ThingSetMaker root;

		// Token: 0x04002034 RID: 8244
		public ThingSetMakerParams debugParams;
	}
}
