using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class AnimalBiomeRecord
	{
		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public BiomeDef biome;

		
		public float commonality;
	}
}
