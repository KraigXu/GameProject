using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C31 RID: 3121
	public class TaleData_Def : TaleData
	{
		// Token: 0x06004A6A RID: 19050 RVA: 0x00192878 File Offset: 0x00190A78
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpDefName = ((this.def != null) ? this.def.defName : null);
				this.tmpDefType = ((this.def != null) ? this.def.GetType() : null);
			}
			Scribe_Values.Look<string>(ref this.tmpDefName, "defName", null, false);
			Scribe_Values.Look<Type>(ref this.tmpDefType, "defType", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.tmpDefName != null)
			{
				this.def = GenDefDatabase.GetDef(this.tmpDefType, BackCompatibility.BackCompatibleDefName(this.tmpDefType, this.tmpDefName, false, null), true);
			}
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x0019291E File Offset: 0x00190B1E
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			if (this.def != null)
			{
				yield return new Rule_String(prefix + "_label", this.def.label);
				yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.def.label, false, false));
				yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.def.label, false, false));
			}
			yield break;
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x00192935 File Offset: 0x00190B35
		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}

		// Token: 0x04002A3B RID: 10811
		public Def def;

		// Token: 0x04002A3C RID: 10812
		private string tmpDefName;

		// Token: 0x04002A3D RID: 10813
		private Type tmpDefType;
	}
}
