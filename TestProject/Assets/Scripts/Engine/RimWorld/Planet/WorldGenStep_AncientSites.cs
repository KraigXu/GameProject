using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		
		// (get) Token: 0x06006AF9 RID: 27385 RVA: 0x002555D2 File Offset: 0x002537D2
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientSites();
		}

		
		private void GenerateAncientSites()
		{
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				Find.World.genData.ancientSites.Add(TileFinder.RandomSettlementTileFor(null, false, null));
			}
		}

		
		public FloatRange ancientSitesPer100kTiles;
	}
}
