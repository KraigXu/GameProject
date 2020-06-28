using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B0 RID: 2224
	public class WeatherCommonalityRecord
	{
		// Token: 0x060035B7 RID: 13751 RVA: 0x00124204 File Offset: 0x00122404
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04001D6D RID: 7533
		public WeatherDef weather;

		// Token: 0x04001D6E RID: 7534
		public float commonality;
	}
}
