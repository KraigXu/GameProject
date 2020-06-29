using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	
	public class RulePack
	{
		
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

		
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		
		[MayTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesFiles = new List<string>();

		
		private List<Rule> rulesRaw;

		
		public List<RulePackDef> include;

		
		[Unsaved(false)]
		private List<Rule> rulesResolved;

		
		[Unsaved(false)]
		private List<Rule> untranslatedRulesResolved;

		
		[Unsaved(false)]
		private List<string> untranslatedRulesStrings;

		
		[Unsaved(false)]
		private List<string> untranslatedRulesFiles;

		
		[Unsaved(false)]
		private List<Rule> untranslatedRulesRaw;
	}
}
