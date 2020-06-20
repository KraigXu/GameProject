using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020000E6 RID: 230
	public class RulePackDef : Def
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001DBDC File Offset: 0x0001BDDC
		public List<Rule> RulesPlusIncludes
		{
			get
			{
				if (this.cachedRules == null)
				{
					this.cachedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedRules.AddRange(this.rulePack.Rules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedRules.AddRange(this.include[i].RulesPlusIncludes);
						}
					}
				}
				return this.cachedRules;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0001DC5C File Offset: 0x0001BE5C
		public List<Rule> UntranslatedRulesPlusIncludes
		{
			get
			{
				if (this.cachedUntranslatedRules == null)
				{
					this.cachedUntranslatedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedUntranslatedRules.AddRange(this.rulePack.UntranslatedRules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedUntranslatedRules.AddRange(this.include[i].UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.cachedUntranslatedRules;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0001DCDA File Offset: 0x0001BEDA
		public List<Rule> RulesImmediate
		{
			get
			{
				if (this.rulePack == null)
				{
					return null;
				}
				return this.rulePack.Rules;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0001DCF1 File Offset: 0x0001BEF1
		public List<Rule> UntranslatedRulesImmediate
		{
			get
			{
				if (this.rulePack == null)
				{
					return null;
				}
				return this.rulePack.UntranslatedRules;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0001DD08 File Offset: 0x0001BF08
		public string FirstRuleKeyword
		{
			get
			{
				List<Rule> rulesPlusIncludes = this.RulesPlusIncludes;
				if (!rulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return rulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001DD38 File Offset: 0x0001BF38
		public string FirstUntranslatedRuleKeyword
		{
			get
			{
				List<Rule> untranslatedRulesPlusIncludes = this.UntranslatedRulesPlusIncludes;
				if (!untranslatedRulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return untranslatedRulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001DD66 File Offset: 0x0001BF66
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.include != null)
			{
				int num;
				for (int i = 0; i < this.include.Count; i = num + 1)
				{
					if (this.include[i].include != null && this.include[i].include.Contains(this))
					{
						yield return "includes other RulePackDef which includes it: " + this.include[i].defName;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001DD76 File Offset: 0x0001BF76
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		// Token: 0x04000559 RID: 1369
		public List<RulePackDef> include;

		// Token: 0x0400055A RID: 1370
		private RulePack rulePack;

		// Token: 0x0400055B RID: 1371
		[Unsaved(false)]
		private List<Rule> cachedRules;

		// Token: 0x0400055C RID: 1372
		[Unsaved(false)]
		private List<Rule> cachedUntranslatedRules;
	}
}
