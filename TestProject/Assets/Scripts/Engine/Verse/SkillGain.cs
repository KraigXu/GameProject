using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D7 RID: 215
	public class SkillGain
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public SkillGain()
		{
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001C56A File Offset: 0x0001A76A
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001C580 File Offset: 0x0001A780
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured SkillGain: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null);
			this.xp = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040004F1 RID: 1265
		public SkillDef skill;

		// Token: 0x040004F2 RID: 1266
		public int xp;
	}
}
