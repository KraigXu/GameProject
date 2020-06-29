using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class IncidentCategoryEntry
	{
		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public IncidentCategoryDef category;

		
		public float weight;
	}
}
