using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200041F RID: 1055
	public sealed class ThingOption
	{
		// Token: 0x06001FA6 RID: 8102 RVA: 0x000C1921 File Offset: 0x000BFB21
		public ThingOption()
		{
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x000C1934 File Offset: 0x000BFB34
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000C1958 File Offset: 0x000BFB58
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingOption: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x000C19B4 File Offset: 0x000BFBB4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				", weight=",
				this.weight.ToString("0.##"),
				")"
			});
		}

		// Token: 0x04001324 RID: 4900
		public ThingDef thingDef;

		// Token: 0x04001325 RID: 4901
		public float weight = 1f;
	}
}
