using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldGenStep_Factions : WorldGenStep
	{
		
		// (get) Token: 0x06006B02 RID: 27394 RVA: 0x0025566E File Offset: 0x0025386E
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
