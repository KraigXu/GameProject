using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C27 RID: 3111
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06004A30 RID: 18992
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x06004A31 RID: 18993 RVA: 0x0019160C File Offset: 0x0018F80C
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			new GenStep_ScatterThings
			{
				nearPlayerStart = this.NearPlayerStart,
				allowFoggedPositions = !this.NearPlayerStart,
				thingDef = this.thingDef,
				stuff = this.stuff,
				count = this.count,
				spotMustBeStandable = true,
				minSpacing = 5f,
				clusterSize = ((this.thingDef.category == ThingCategory.Building) ? 1 : 4)
			}.Generate(map, default(GenStepParams));
		}
	}
}
