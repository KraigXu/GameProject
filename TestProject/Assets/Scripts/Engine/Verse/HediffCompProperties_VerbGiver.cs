using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000273 RID: 627
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x060010D1 RID: 4305 RVA: 0x0005F88D File Offset: 0x0005DA8D
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0005F8A8 File Offset: 0x0005DAA8
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0005F8F1 File Offset: 0x0005DAF1
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.tools != null)
			{
				Tool tool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (tool != null)
				{
					yield return string.Format("duplicate hediff tool id {0}", tool.id);
				}
				foreach (Tool tool2 in this.tools)
				{
					foreach (string text2 in tool2.ConfigErrors())
					{
						yield return text2;
					}
					enumerator = null;
				}
				List<Tool>.Enumerator enumerator2 = default(List<Tool>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04000C3D RID: 3133
		public List<VerbProperties> verbs;

		// Token: 0x04000C3E RID: 3134
		public List<Tool> tools;
	}
}
