using System;
using System.Text.RegularExpressions;
using RimWorld.QuestGen;

namespace Verse.Grammar
{
	// Token: 0x020004C7 RID: 1223
	public class Rule_String : Rule
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x000D7DBD File Offset: 0x000D5FBD
		public override float BaseSelectionWeight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x000D7DC5 File Offset: 0x000D5FC5
		public override float Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000D7DCD File Offset: 0x000D5FCD
		public override Rule DeepCopy()
		{
			Rule_String rule_String = (Rule_String)base.DeepCopy();
			rule_String.output = this.output;
			rule_String.weight = this.weight;
			rule_String.priority = this.priority;
			return rule_String;
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000D7DFE File Offset: 0x000D5FFE
		public Rule_String()
		{
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000D7E11 File Offset: 0x000D6011
		public Rule_String(string keyword, string output)
		{
			this.keyword = keyword;
			this.output = output;
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000D7E34 File Offset: 0x000D6034
		public Rule_String(string rawString)
		{
			Match match = Rule_String.pattern.Match(rawString);
			if (!match.Success)
			{
				Log.Error(string.Format("Bad string pass when reading rule {0}", rawString), false);
				return;
			}
			this.keyword = match.Groups["keyword"].Value;
			this.output = match.Groups["output"].Value;
			for (int i = 0; i < match.Groups["paramname"].Captures.Count; i++)
			{
				string value = match.Groups["paramname"].Captures[i].Value;
				string value2 = match.Groups["paramoperator"].Captures[i].Value;
				string value3 = match.Groups["paramvalue"].Captures[i].Value;
				if (value == "p")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare p instead of assigning in rule {0}", rawString), false);
					}
					this.weight = float.Parse(value3);
				}
				else if (value == "priority")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare priority instead of assigning in rule {0}", rawString), false);
					}
					this.priority = float.Parse(value3);
				}
				else if (value == "tag")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare tag instead of assigning in rule {0}", rawString), false);
					}
					this.tag = value3;
				}
				else if (value == "requiredTag")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare requiredTag instead of assigning in rule {0}", rawString), false);
					}
					this.requiredTag = value3;
				}
				else if (value == "debug")
				{
					Log.Error(string.Format("Rule {0} contains debug flag; fix before commit", rawString), false);
				}
				else if (value2 == "==" || value2 == "!=" || value2 == ">" || value2 == "<" || value2 == ">=" || value2 == "<=")
				{
					base.AddConstantConstraint(value, value3, value2);
				}
				else
				{
					Log.Error(string.Format("Unknown parameter {0} in rule {1}", value, rawString), false);
				}
			}
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000D80B3 File Offset: 0x000D62B3
		public override string Generate()
		{
			return this.output;
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000D80BC File Offset: 0x000D62BC
		public override string ToString()
		{
			return ((this.keyword != null) ? this.keyword : "null_keyword") + " → " + ((this.output != null) ? this.output.Replace("\n", "\\n") : "null_output");
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000D810C File Offset: 0x000D630C
		public void AppendPrefixToAllKeywords(string prefix)
		{
			Rule_String.tmpPrefix = prefix;
			if (this.output == null)
			{
				Log.Error("Rule_String output was null.", false);
				this.output = "";
			}
			this.output = Regex.Replace(this.output, "\\[(.*?)\\]", new MatchEvaluator(Rule_String.RegexMatchEvaluatorAppendPrefix));
			if (this.constantConstraints != null)
			{
				for (int i = 0; i < this.constantConstraints.Count; i++)
				{
					Rule.ConstantConstraint constantConstraint = default(Rule.ConstantConstraint);
					constantConstraint.key = this.constantConstraints[i].key;
					if (!prefix.NullOrEmpty())
					{
						constantConstraint.key = prefix + "/" + constantConstraint.key;
					}
					constantConstraint.key = QuestGenUtility.NormalizeVarPath(constantConstraint.key);
					constantConstraint.value = this.constantConstraints[i].value;
					constantConstraint.type = this.constantConstraints[i].type;
					this.constantConstraints[i] = constantConstraint;
				}
			}
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000D8218 File Offset: 0x000D6418
		private static string RegexMatchEvaluatorAppendPrefix(Match match)
		{
			string text = match.Groups[1].Value;
			if (!Rule_String.tmpPrefix.NullOrEmpty())
			{
				text = Rule_String.tmpPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return "[" + text + "]";
		}

		// Token: 0x040015C9 RID: 5577
		[MustTranslate]
		private string output;

		// Token: 0x040015CA RID: 5578
		private float weight = 1f;

		// Token: 0x040015CB RID: 5579
		private float priority;

		// Token: 0x040015CC RID: 5580
		private static Regex pattern = new Regex("\r\n\t\t# hold on to your butts, this is gonna get weird\r\n\r\n\t\t^\r\n\t\t(?<keyword>[a-zA-Z0-9_/]+)\t\t\t\t\t# keyword; roughly limited to standard C# identifier rules\r\n\t\t(\t\t\t\t\t\t\t\t\t\t\t# parameter list is optional, open the capture group so we can keep it or ignore it\r\n\t\t\t\\(\t\t\t\t\t\t\t\t\t\t# this is the actual parameter list opening\r\n\t\t\t\t(\t\t\t\t\t\t\t\t\t# unlimited number of parameter groups\r\n\t\t\t\t\t(?<paramname>[a-zA-Z0-9_/]+)\t# parameter name is similar\r\n\t\t\t\t\t(?<paramoperator>==|=|!=|>=|<=|>|<|) # operators; empty operator is allowed\r\n\t\t\t\t\t(?<paramvalue>[^\\,\\)]*)\t\t\t# parameter value, however, allows everything except comma and closeparen!\r\n\t\t\t\t\t,?\t\t\t\t\t\t\t\t# comma can be used to separate blocks; it is also silently ignored if it's a trailing comma\r\n\t\t\t\t)*\r\n\t\t\t\\)\r\n\t\t)?\r\n\t\t->(?<output>.*)\t\t\t\t\t\t\t\t# output is anything-goes\r\n\t\t$\r\n\r\n\t\t", RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

		// Token: 0x040015CD RID: 5581
		private static string tmpPrefix;
	}
}
