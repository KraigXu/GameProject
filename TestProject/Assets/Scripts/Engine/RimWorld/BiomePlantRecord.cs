using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class BiomePlantRecord
	{
		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public ThingDef plant;

		
		public float commonality;
	}
}
