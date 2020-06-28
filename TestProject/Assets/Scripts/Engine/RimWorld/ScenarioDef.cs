using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008FE RID: 2302
	public class ScenarioDef : Def
	{
		// Token: 0x060036DC RID: 14044 RVA: 0x00128590 File Offset: 0x00126790
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = this.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = this.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x001285F5 File Offset: 0x001267F5
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.scenario == null)
			{
				yield return "null scenario";
			}
			foreach (string text in this.scenario.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04001F8F RID: 8079
		public Scenario scenario;
	}
}
