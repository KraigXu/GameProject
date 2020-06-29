using System;
using Verse;

namespace RimWorld
{
	
	public class GenStep_ScenParts : GenStep
	{
		
		// (get) Token: 0x06003EA6 RID: 16038 RVA: 0x0014C444 File Offset: 0x0014A644
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
