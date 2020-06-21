using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001213 RID: 4627
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x06006B02 RID: 27394 RVA: 0x0025566E File Offset: 0x0025386E
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x06006B03 RID: 27395 RVA: 0x00255675 File Offset: 0x00253875
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		// Token: 0x06006B04 RID: 27396 RVA: 0x00002681 File Offset: 0x00000881
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
