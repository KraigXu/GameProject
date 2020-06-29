using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class BiomeAnimalRecord
	{
		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public PawnKindDef animal;

		
		public float commonality;
	}
}
