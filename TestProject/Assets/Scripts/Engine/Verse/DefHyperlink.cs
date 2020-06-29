using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	
	public class DefHyperlink
	{
		
		public DefHyperlink()
		{
		}

		
		public DefHyperlink(Def def)
		{
			this.def = def;
		}

		
		public DefHyperlink(RoyalTitleDef def, Faction faction)
		{
			this.def = def;
			this.faction = faction;
		}

		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured DefHyperlink: " + xmlRoot.OuterXml, false);
				return;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlRoot.Name, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Misconfigured DefHyperlink. Could not find def of type " + xmlRoot.Name, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.FirstChild.Value, null, typeInAnyAssembly);
		}

		
		public static implicit operator DefHyperlink(Def def)
		{
			return new DefHyperlink
			{
				def = def
			};
		}

		
		public Def def;

		
		public Faction faction;
	}
}
