using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D6 RID: 214
	public class SkillRequirement
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001C4AD File Offset: 0x0001A6AD
		public string Summary
		{
			get
			{
				if (this.skill == null)
				{
					return "";
				}
				return string.Format("{0} ({1})", this.skill.LabelCap, this.minLevel);
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001C4E2 File Offset: 0x0001A6E2
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001C50F File Offset: 0x0001A70F
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null);
			this.minLevel = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001C53A File Offset: 0x0001A73A
		public override string ToString()
		{
			if (this.skill == null)
			{
				return "null-skill-requirement";
			}
			return this.skill.defName + "-" + this.minLevel;
		}

		// Token: 0x040004EF RID: 1263
		public SkillDef skill;

		// Token: 0x040004F0 RID: 1264
		public int minLevel;
	}
}
