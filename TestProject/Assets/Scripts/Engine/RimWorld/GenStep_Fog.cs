using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class GenStep_Fog : GenStep
	{
		
		// (get) Token: 0x06003E57 RID: 15959 RVA: 0x001490D8 File Offset: 0x001472D8
		public override int SeedPart
		{
			get
			{
				return 1568957891;
			}
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("GenerateInitialFogGrid");
			map.fogGrid.SetAllFogged();
			FloodFillerFog.FloodUnfog(MapGenerator.PlayerStartSpot, map);
			List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
			for (int i = 0; i < rootsToUnfog.Count; i++)
			{
				FloodFillerFog.FloodUnfog(rootsToUnfog[i], map);
			}
			DeepProfiler.End();
		}
	}
}
