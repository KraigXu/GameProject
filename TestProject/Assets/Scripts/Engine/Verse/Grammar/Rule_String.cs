using System;
using System.Text.RegularExpressions;
using RimWorld.QuestGenNew;

namespace Verse.Grammar
{
	
	public class Rule_String : Rule
	{
		
		
		public override float BaseSelectionWeight
		{
			get
			{
				return this.weight;
			}
		}

		
		
		public override float Priority
		{
			get
			{
				return this.priority;
			}
		}

		
		public override Rule DeepCopy()
		{
			Rule_String rule_String = (Rule_String)base.DeepCopy();
			rule_String.output = this.output;
			rule_String.weight = this.weight;
			rule_String.priority = this.priority;
			return rule_String;
		}

		
		public Rule_String()
		{
		}

		
		public Rule_String(string keyword, string output)
		{
			this.keyword = keyword;
			this.output = output;
		}

		
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

		
		public override string Generate()
		{
			return this.output;
		}

		
		public override string ToString()
		{
			return ((this.keyword != null) ? this.keyword : "null_keyword") + " → " + ((this.output != null) ? this.output.Replace("\n", "\\n") : "null_output");
		}

		
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

		
		[MustTranslate]
		private string output;

		
		private float weight = 1f;

		
		private float priority;

		
		private static Regex pattern = new Regex("\r\n\t\t# hold on to your butts, this is gonna get weird\r\n\r\n\t\t^\r\n\t\t(?<keyword>[a-zA-Z0-9_/]+)\t\t\t\t\t# keyword; roughly limited to standard C# identifier rules\r\n\t\t(\t\t\t\t\t\t\t\t\t\t\t# parameter list is optional, open the capture group so we can keep it or ignore it\r\n\t\t\t\\(\t\t\t\t\t\t\t\t\t\t# this is the actual parameter list opening\r\n\t\t\t\t(\t\t\t\t\t\t\t\t\t# unlimited number of parameter groups\r\n\t\t\t\t\t(?<paramname>[a-zA-Z0-9_/]+)\t# parameter name is similar\r\n\t\t\t\t\t(?<paramoperator>==|=|!=|>=|<=|>|<|) # operators; empty operator is allowed\r\n\t\t\t\t\t(?<paramvalue>[^\\,\\)]*)\t\t\t# parameter value, however, allows everything except comma and closeparen!\r\n\t\t\t\t\t,?\t\t\t\t\t\t\t\t# comma can be used to separate blocks; it is also silently ignored if it's a trailing comma\r\n\t\t\t\t)*\r\n\t\t\t\\)\r\n\t\t)?\r\n\t\t->(?<output>.*)\t\t\t\t\t\t\t\t# output is anything-goes\r\n\t\t$\r\n\r\n\t\t", RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

		
		private static string tmpPrefix;
	}
}
