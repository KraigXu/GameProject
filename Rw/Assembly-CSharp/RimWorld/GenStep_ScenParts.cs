using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A5A RID: 2650
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06003EA6 RID: 16038 RVA: 0x0014C444 File Offset: 0x0014A644
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x0014C44B File Offset: 0x0014A64B
		public override void Generate(Map map, GenStepParams parms)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
