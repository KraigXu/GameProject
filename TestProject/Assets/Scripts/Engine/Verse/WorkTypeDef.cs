using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020000FF RID: 255
	public class WorkTypeDef : Def
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x0001FDCE File Offset: 0x0001DFCE
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.naturalPriority < 0 || this.naturalPriority > 10000)
			{
				yield return "naturalPriority is " + this.naturalPriority + ", but it must be between 0 and 10000";
			}
			yield break;
			yield break;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001FDE0 File Offset: 0x0001DFE0
		public override void ResolveReferences()
		{
			foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
			{
				this.workGiversByPriority.Add(item);
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001FE68 File Offset: 0x0001E068
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		// Token: 0x0400067E RID: 1662
		public WorkTags workTags;

		// Token: 0x0400067F RID: 1663
		[MustTranslate]
		public string labelShort;

		// Token: 0x04000680 RID: 1664
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x04000681 RID: 1665
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x04000682 RID: 1666
		[MustTranslate]
		public string verb;

		// Token: 0x04000683 RID: 1667
		public bool visible = true;

		// Token: 0x04000684 RID: 1668
		public int naturalPriority;

		// Token: 0x04000685 RID: 1669
		public bool alwaysStartActive;

		// Token: 0x04000686 RID: 1670
		public bool requireCapableColonist;

		// Token: 0x04000687 RID: 1671
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x04000688 RID: 1672
		[Unsaved(false)]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();
	}
}
