using System;
using System.Xml;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001107 RID: 4359
	public class PrefixCapturedVar
	{
		// Token: 0x06006645 RID: 26181 RVA: 0x0023D384 File Offset: 0x0023B584
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured PrefixCapturedVar: " + xmlRoot.OuterXml, false);
				return;
			}
			this.name = xmlRoot.Name;
			this.value = new SlateRef<object>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
			TKeySystem.MarkTreatAsList(xmlRoot.ParentNode);
		}

		// Token: 0x04003E5E RID: 15966
		[NoTranslate]
		[TranslationHandle]
		public string name;

		// Token: 0x04003E5F RID: 15967
		public SlateRef<object> value;
	}
}
