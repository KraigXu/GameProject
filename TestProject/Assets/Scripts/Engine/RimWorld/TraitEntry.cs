using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6C RID: 2924
	public class TraitEntry
	{
		// Token: 0x06004468 RID: 17512 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public TraitEntry()
		{
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x00171F23 File Offset: 0x00170123
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x00171F39 File Offset: 0x00170139
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

		// Token: 0x040026F2 RID: 9970
		public TraitDef def;

		// Token: 0x040026F3 RID: 9971
		public int degree;
	}
}
