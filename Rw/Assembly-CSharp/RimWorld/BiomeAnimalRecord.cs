using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B3 RID: 2227
	public class BiomeAnimalRecord
	{
		// Token: 0x060035BD RID: 13757 RVA: 0x00124276 File Offset: 0x00122476
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04001D73 RID: 7539
		public PawnKindDef animal;

		// Token: 0x04001D74 RID: 7540
		public float commonality;
	}
}
