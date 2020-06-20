using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CCB RID: 3275
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x06004F5D RID: 20317 RVA: 0x001AB8AA File Offset: 0x001A9AAA
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x001AB8BD File Offset: 0x001A9ABD
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x001AB8D6 File Offset: 0x001A9AD6
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x04002C89 RID: 11401
		public ThingSetMakerDef def;
	}
}
