using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class TraitEntry
	{
		
		public TraitEntry()
		{
		}

		
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.def = DefDatabase<TraitDef>.GetNamed(xmlRoot.Name, true);
			if (xmlRoot.HasChildNodes)
			{
				this.degree = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
				return;
			}
			this.degree = 0;
		}

		
		public TraitDef def;

		
		public int degree;
	}
}
