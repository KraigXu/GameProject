using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001A3 RID: 419
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x00042E73 File Offset: 0x00041073
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00042E7C File Offset: 0x0004107C
		public override void Generate(Map map, GenStepParams parms)
		{
			RandomGenStepSelectorOption randomGenStepSelectorOption = this.options.RandomElementByWeight((RandomGenStepSelectorOption opt) => opt.weight);
			if (randomGenStepSelectorOption.genStep != null)
			{
				randomGenStepSelectorOption.genStep.Generate(map, parms);
			}
			if (randomGenStepSelectorOption.def != null)
			{
				randomGenStepSelectorOption.def.genStep.Generate(map, parms);
			}
		}

		// Token: 0x04000962 RID: 2402
		public List<RandomGenStepSelectorOption> options;
	}
}
