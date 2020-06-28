using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC5 RID: 3269
	public abstract class ThingSetMaker_Conditional : ThingSetMaker
	{
		// Token: 0x06004F49 RID: 20297 RVA: 0x001AB5C7 File Offset: 0x001A97C7
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.Condition(parms) && this.thingSetMaker.CanGenerate(parms);
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x001AB5E0 File Offset: 0x001A97E0
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.thingSetMaker.Generate(parms));
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x001AB5F4 File Offset: 0x001A97F4
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.thingSetMaker.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x001AB602 File Offset: 0x001A9802
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.thingSetMaker.ResolveReferences();
		}

		// Token: 0x06004F4D RID: 20301
		protected abstract bool Condition(ThingSetMakerParams parms);

		// Token: 0x04002C7E RID: 11390
		public ThingSetMaker thingSetMaker;
	}
}
