using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200006F RID: 111
	public class DefHyperlink
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public DefHyperlink()
		{
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00016C73 File Offset: 0x00014E73
		public DefHyperlink(Def def)
		{
			this.def = def;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00016C82 File Offset: 0x00014E82
		public DefHyperlink(RoyalTitleDef def, Faction faction)
		{
			this.def = def;
			this.faction = faction;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00016C98 File Offset: 0x00014E98
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

		// Token: 0x06000456 RID: 1110 RVA: 0x00016D0F File Offset: 0x00014F0F
		public static implicit operator DefHyperlink(Def def)
		{
			return new DefHyperlink
			{
				def = def
			};
		}

		// Token: 0x0400016A RID: 362
		public Def def;

		// Token: 0x0400016B RID: 363
		public Faction faction;
	}
}
