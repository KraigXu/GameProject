using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class GenStep_RandomSelector : GenStep
	{
		
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x00042E73 File Offset: 0x00041073
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		
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

		
		public List<RandomGenStepSelectorOption> options;
	}
}
