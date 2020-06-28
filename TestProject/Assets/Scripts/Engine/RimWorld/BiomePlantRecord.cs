using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B1 RID: 2225
	public class BiomePlantRecord
	{
		// Token: 0x060035B9 RID: 13753 RVA: 0x0012422A File Offset: 0x0012242A
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04001D6F RID: 7535
		public ThingDef plant;

		// Token: 0x04001D70 RID: 7536
		public float commonality;
	}
}
