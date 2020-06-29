using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class MTBByBiome
	{
		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured MTBByBiome: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name, null, null);
			this.mtbDays = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public BiomeDef biome;

		
		public float mtbDays;
	}
}
