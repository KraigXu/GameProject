using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B2 RID: 2226
	public class PlantBiomeRecord
	{
		// Token: 0x060035BB RID: 13755 RVA: 0x00124250 File Offset: 0x00122450
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04001D71 RID: 7537
		public BiomeDef biome;

		// Token: 0x04001D72 RID: 7538
		public float commonality;
	}
}
