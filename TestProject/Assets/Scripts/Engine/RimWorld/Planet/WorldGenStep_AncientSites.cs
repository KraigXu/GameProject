using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001211 RID: 4625
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x06006AF9 RID: 27385 RVA: 0x002555D2 File Offset: 0x002537D2
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x002555D9 File Offset: 0x002537D9
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientSites();
		}

		// Token: 0x06006AFB RID: 27387 RVA: 0x002555E4 File Offset: 0x002537E4
		private void GenerateAncientSites()
		{
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				Find.World.genData.ancientSites.Add(TileFinder.RandomSettlementTileFor(null, false, null));
			}
		}

		// Token: 0x040042DB RID: 17115
		public FloatRange ancientSitesPer100kTiles;
	}
}
