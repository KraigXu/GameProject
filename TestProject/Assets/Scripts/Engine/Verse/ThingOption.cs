using System;
using System.Xml;

namespace Verse
{
	
	public sealed class ThingOption
	{
		
		public ThingOption()
		{
		}

		
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		
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

		
		public ThingDef thingDef;

		
		public float weight = 1f;
	}
}
