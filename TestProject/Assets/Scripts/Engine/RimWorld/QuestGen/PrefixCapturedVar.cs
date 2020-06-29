using System;
using System.Xml;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class PrefixCapturedVar
	{
		
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

		
		[NoTranslate]
		[TranslationHandle]
		public string name;

		
		public SlateRef<object> value;
	}
}
