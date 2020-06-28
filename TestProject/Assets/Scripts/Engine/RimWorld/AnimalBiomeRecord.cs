using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B4 RID: 2228
	public class AnimalBiomeRecord
	{
		// Token: 0x060035BF RID: 13759 RVA: 0x0012429C File Offset: 0x0012249C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04001D75 RID: 7541
		public BiomeDef biome;

		// Token: 0x04001D76 RID: 7542
		public float commonality;
	}
}
