using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class WeatherCommonalityRecord
	{
		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public WeatherDef weather;

		
		public float commonality;
	}
}
