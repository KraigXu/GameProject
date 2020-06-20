using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D3 RID: 2259
	public class MTBByBiome
	{
		// Token: 0x06003636 RID: 13878 RVA: 0x00125E40 File Offset: 0x00124040
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured MTBByBiome: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name, null, null);
			this.mtbDays = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04001E6A RID: 7786
		public BiomeDef biome;

		// Token: 0x04001E6B RID: 7787
		public float mtbDays;
	}
}
