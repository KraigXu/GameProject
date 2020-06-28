using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x020004C5 RID: 1221
	public class RulePack
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x000D791C File Offset: 0x000D5B1C
		public List<Rule> Rules
		{
			get
			{
				if (this.rulesResolved == null)
				{
					this.rulesResolved = RulePack.GetRulesResolved(this.rulesRaw, this.rulesStrings, this.rulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.rulesResolved.AddRange(rulePackDef.RulesPlusIncludes);
						}
					}
				}
				return this.rulesResolved;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x000D79AC File Offset: 0x000D5BAC
		public List<Rule> UntranslatedRules
		{
			get
			{
				if (this.untranslatedRulesResolved == null)
				{
					this.untranslatedRulesResolved = RulePack.GetRulesResolved(this.untranslatedRulesRaw, this.untranslatedRulesStrings, this.untranslatedRulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.untranslatedRulesResolved.AddRange(rulePackDef.UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.untranslatedRulesResolved;
			}
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x000D7A3C File Offset: 0x000D5C3C
		public void PostLoad()
		{
			this.untranslatedRulesStrings = this.rulesStrings.ToList<string>();
			this.untranslatedRulesFiles = this.rulesFiles.ToList<string>();
			if (this.rulesRaw != null)
			{
				this.untranslatedRulesRaw = new List<Rule>();
				for (int i = 0; i < this.rulesRaw.Count; i++)
				{
					this.untranslatedRulesRaw.Add(this.rulesRaw[i].DeepCopy());
				}
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000D7AB0 File Offset: 0x000D5CB0
		private static List<Rule> GetRulesResolved(List<Rule> rulesRaw, List<string> rulesStrings, List<string> rulesFiles)
		{
			List<Rule> list = new List<Rule>();
			for (int i = 0; i < rulesStrings.Count; i++)
			{
				try
				{
					Rule_String rule_String = new Rule_String(rulesStrings[i]);
					rule_String.Init();
					list.Add(rule_String);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing grammar rule from ",
						rulesStrings[i],
						": ",
						ex
					}), false);
				}
			}
			for (int j = 0; j < rulesFiles.Count; j++)
			{
				try
				{
					string[] array = rulesFiles[j].Split(new string[]
					{
						"->"
					}, StringSplitOptions.None);
					Rule_File rule_File = new Rule_File();
					rule_File.keyword = array[0].Trim();
					rule_File.path = array[1].Trim();
					rule_File.Init();
					list.Add(rule_File);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error initializing Rule_File ",
						rulesFiles[j],
						": ",
						ex2
					}), false);
				}
			}
			if (rulesRaw != null)
			{
				for (int k = 0; k < rulesRaw.Count; k++)
				{
					try
					{
						rulesRaw[k].Init();
						list.Add(rulesRaw[k]);
					}
					catch (Exception ex3)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error initializing rule ",
							rulesRaw[k].ToStringSafe<Rule>(),
							": ",
							ex3
						}), false);
					}
				}
			}
			return list;
		}

		// Token: 0x040015BC RID: 5564
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		// Token: 0x040015BD RID: 5565
		[MayTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesFiles = new List<string>();

		// Token: 0x040015BE RID: 5566
		private List<Rule> rulesRaw;

		// Token: 0x040015BF RID: 5567
		public List<RulePackDef> include;

		// Token: 0x040015C0 RID: 5568
		[Unsaved(false)]
		private List<Rule> rulesResolved;

		// Token: 0x040015C1 RID: 5569
		[Unsaved(false)]
		private List<Rule> untranslatedRulesResolved;

		// Token: 0x040015C2 RID: 5570
		[Unsaved(false)]
		private List<string> untranslatedRulesStrings;

		// Token: 0x040015C3 RID: 5571
		[Unsaved(false)]
		private List<string> untranslatedRulesFiles;

		// Token: 0x040015C4 RID: 5572
		[Unsaved(false)]
		private List<Rule> untranslatedRulesRaw;
	}
}
