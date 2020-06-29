using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	
	public class RulePackDef : Def
	{
		
		
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

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
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

		
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		
		public List<RulePackDef> include;

		
		private RulePack rulePack;

		
		[Unsaved(false)]
		private List<Rule> cachedRules;

		
		[Unsaved(false)]
		private List<Rule> cachedUntranslatedRules;
	}
}
