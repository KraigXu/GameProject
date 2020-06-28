using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A14 RID: 2580
	public class IncidentCategoryEntry
	{
		// Token: 0x06003D3E RID: 15678 RVA: 0x00143BD4 File Offset: 0x00141DD4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040023BC RID: 9148
		public IncidentCategoryDef category;

		// Token: 0x040023BD RID: 9149
		public float weight;
	}
}
