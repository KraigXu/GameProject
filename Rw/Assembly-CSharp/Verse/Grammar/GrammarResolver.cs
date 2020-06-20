using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse.Grammar
{
	// Token: 0x020004C3 RID: 1219
	public static class GrammarResolver
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x000D6C70 File Offset: 0x000D4E70
		public static string Resolve(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false, string untranslatedRootKeyword = null, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
			{
				return GrammarResolver.ResolveUnsafe(rootKeyword, request, debugLabel, forceLog, false, extraTags, outTags, capitalizeFirstSentence);
			}
			bool flag;
			string text;
			Exception ex;
			try
			{
				text = GrammarResolver.ResolveUnsafe(rootKeyword, request, out flag, debugLabel, forceLog, false, extraTags, outTags, capitalizeFirstSentence);
				ex = null;
			}
			catch (Exception ex2)
			{
				flag = false;
				text = "";
				ex = ex2;
			}
			if (flag)
			{
				return text;
			}
			string text2 = "Failed to resolve text. Trying again with English.";
			if (ex != null)
			{
				text2 = text2 + " Exception: " + ex;
			}
			Log.ErrorOnce(text2, text.GetHashCode(), false);
			if (outTags != null)
			{
				outTags.Clear();
			}
			return GrammarResolver.ResolveUnsafe(untranslatedRootKeyword ?? rootKeyword, request, out flag, debugLabel, forceLog, true, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000D6D18 File Offset: 0x000D4F18
		public static string ResolveUnsafe(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false, bool useUntranslatedRules = false, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			bool flag;
			return GrammarResolver.ResolveUnsafe(rootKeyword, request, out flag, debugLabel, forceLog, useUntranslatedRules, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000D6D38 File Offset: 0x000D4F38
		public static string ResolveUnsafe(string rootKeyword, GrammarRequest request, out bool success, string debugLabel = null, bool forceLog = false, bool useUntranslatedRules = false, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			bool flag = forceLog || DebugViewSettings.logGrammarResolution;
			GrammarResolver.rules.Clear();
			GrammarResolver.rulePool.Clear();
			if (flag)
			{
				GrammarResolver.logSbTrace = new StringBuilder();
				GrammarResolver.logSbMid = new StringBuilder();
				GrammarResolver.logSbRules = new StringBuilder();
			}
			List<Rule> rulesAllowNull = request.RulesAllowNull;
			if (rulesAllowNull != null)
			{
				if (flag)
				{
					GrammarResolver.logSbRules.AppendLine("CUSTOM RULES");
				}
				for (int i = 0; i < rulesAllowNull.Count; i++)
				{
					GrammarResolver.AddRule(rulesAllowNull[i]);
					if (flag)
					{
						GrammarResolver.logSbRules.AppendLine("■" + rulesAllowNull[i].ToString());
					}
				}
				if (flag)
				{
					GrammarResolver.logSbRules.AppendLine();
				}
			}
			List<RulePackDef> includesAllowNull = request.IncludesAllowNull;
			if (includesAllowNull != null)
			{
				HashSet<RulePackDef> hashSet = new HashSet<RulePackDef>();
				List<RulePackDef> list = new List<RulePackDef>(includesAllowNull);
				if (flag)
				{
					GrammarResolver.logSbMid.AppendLine("INCLUDES");
				}
				while (list.Count > 0)
				{
					RulePackDef rulePackDef = list[list.Count - 1];
					list.RemoveLast<RulePackDef>();
					if (!hashSet.Contains(rulePackDef))
					{
						if (flag)
						{
							GrammarResolver.logSbMid.AppendLine(string.Format("{0}", rulePackDef.defName));
						}
						hashSet.Add(rulePackDef);
						List<Rule> list2 = useUntranslatedRules ? rulePackDef.UntranslatedRulesImmediate : rulePackDef.RulesImmediate;
						if (list2 != null)
						{
							foreach (Rule rule in list2)
							{
								GrammarResolver.AddRule(rule);
							}
						}
						if (!rulePackDef.include.NullOrEmpty<RulePackDef>())
						{
							list.AddRange(rulePackDef.include);
						}
					}
				}
			}
			List<RulePack> includesBareAllowNull = request.IncludesBareAllowNull;
			if (includesBareAllowNull != null)
			{
				if (flag)
				{
					GrammarResolver.logSbMid.AppendLine();
					GrammarResolver.logSbMid.AppendLine("BARE INCLUDES");
				}
				for (int j = 0; j < includesBareAllowNull.Count; j++)
				{
					List<Rule> list3 = useUntranslatedRules ? includesBareAllowNull[j].UntranslatedRules : includesBareAllowNull[j].Rules;
					for (int k = 0; k < list3.Count; k++)
					{
						GrammarResolver.AddRule(list3[k]);
						if (flag)
						{
							GrammarResolver.logSbMid.AppendLine("  " + list3[k].ToString());
						}
					}
				}
			}
			if (flag && !extraTags.NullOrEmpty<string>())
			{
				GrammarResolver.logSbMid.AppendLine();
				GrammarResolver.logSbMid.AppendLine("EXTRA TAGS");
				for (int l = 0; l < extraTags.Count; l++)
				{
					GrammarResolver.logSbMid.AppendLine("  " + extraTags[l]);
				}
			}
			List<Rule> list4 = useUntranslatedRules ? RulePackDefOf.GlobalUtility.UntranslatedRulesPlusIncludes : RulePackDefOf.GlobalUtility.RulesPlusIncludes;
			for (int m = 0; m < list4.Count; m++)
			{
				GrammarResolver.AddRule(list4[m]);
			}
			GrammarResolver.loopCount = 0;
			Dictionary<string, string> constantsAllowNull = request.ConstantsAllowNull;
			if (flag && constantsAllowNull != null)
			{
				GrammarResolver.logSbMid.AppendLine("CONSTANTS");
				foreach (KeyValuePair<string, string> keyValuePair in constantsAllowNull)
				{
					GrammarResolver.logSbMid.AppendLine(keyValuePair.Key.PadRight(38) + " " + keyValuePair.Value);
				}
			}
			if (flag)
			{
				GrammarResolver.logSbTrace.Append("GRAMMAR RESOLUTION TRACE");
			}
			string text = "err";
			bool flag2 = false;
			List<string> list5 = new List<string>();
			if (GrammarResolver.TryResolveRecursive(new GrammarResolver.RuleEntry(new Rule_String("", "[" + rootKeyword + "]")), 0, constantsAllowNull, out text, flag, extraTags, list5))
			{
				if (outTags != null)
				{
					outTags.Clear();
					outTags.AddRange(list5);
				}
			}
			else
			{
				flag2 = true;
				if (request.Rules.NullOrEmpty<Rule>())
				{
					text = "ERR";
				}
				else
				{
					text = "ERR: " + request.Rules[0].Generate();
				}
				if (flag)
				{
					GrammarResolver.logSbTrace.Insert(0, "Grammar unresolvable. Root '" + rootKeyword + "'\n\n");
				}
				else
				{
					GrammarResolver.ResolveUnsafe(rootKeyword, request, debugLabel, true, useUntranslatedRules, extraTags, null, true);
				}
			}
			text = GenText.CapitalizeSentences(Find.ActiveLanguageWorker.PostProcessed(text), capitalizeFirstSentence);
			text = GrammarResolver.Spaces.Replace(text, (Match match) => match.Groups[1].Value);
			text = text.Trim();
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(GrammarResolver.logSbTrace.ToString().TrimEndNewlines());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append(GrammarResolver.logSbMid.ToString().TrimEndNewlines());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append(GrammarResolver.logSbRules.ToString().TrimEndNewlines());
				if (flag2)
				{
					if (DebugViewSettings.logGrammarResolution)
					{
						Log.Error(stringBuilder.ToString().Trim() + "\n", false);
					}
					else
					{
						Log.ErrorOnce(stringBuilder.ToString().Trim() + "\n", stringBuilder.ToString().Trim().GetHashCode(), false);
					}
				}
				else
				{
					Log.Message(stringBuilder.ToString().Trim() + "\n", false);
				}
				GrammarResolver.logSbTrace = null;
				GrammarResolver.logSbMid = null;
				GrammarResolver.logSbRules = null;
			}
			success = !flag2;
			return text;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000D72F4 File Offset: 0x000D54F4
		private static void AddRule(Rule rule)
		{
			List<GrammarResolver.RuleEntry> list = null;
			if (!GrammarResolver.rules.TryGetValue(rule.keyword, out list))
			{
				list = GrammarResolver.rulePool.Get();
				list.Clear();
				GrammarResolver.rules[rule.keyword] = list;
			}
			list.Add(new GrammarResolver.RuleEntry(rule));
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000D7348 File Offset: 0x000D5548
		private static bool TryResolveRecursive(GrammarResolver.RuleEntry entry, int depth, Dictionary<string, string> constants, out string output, bool log, List<string> extraTags, List<string> resolvedTags)
		{
			string text = "";
			for (int i = 0; i < depth; i++)
			{
				text += "  ";
			}
			if (log && depth > 0)
			{
				GrammarResolver.logSbTrace.AppendLine();
				GrammarResolver.logSbTrace.Append(depth.ToStringCached().PadRight(3));
				GrammarResolver.logSbTrace.Append(text + entry);
			}
			text += "     ";
			GrammarResolver.loopCount++;
			if (GrammarResolver.loopCount > 1000)
			{
				Log.Error("Hit loops limit resolving grammar.", false);
				output = "HIT_LOOPS_LIMIT";
				if (log)
				{
					GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Hit loops limit");
				}
				return false;
			}
			if (depth > 50)
			{
				Log.Error("Grammar recurred too deep while resolving keyword (>" + 50 + " deep)", false);
				output = "DEPTH_LIMIT_REACHED";
				if (log)
				{
					GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Depth limit reached");
				}
				return false;
			}
			string text2 = entry.rule.Generate();
			bool flag = false;
			int num = -1;
			for (int j = 0; j < text2.Length; j++)
			{
				char c = text2[j];
				if (c == '[')
				{
					num = j;
				}
				if (c == ']')
				{
					if (num == -1)
					{
						Log.Error("Could not resolve rule because of mismatched brackets: " + text2, false);
						output = "MISMATCHED_BRACKETS";
						if (log)
						{
							GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Mismatched brackets");
						}
						flag = true;
					}
					else
					{
						string text3 = text2.Substring(num + 1, j - num - 1);
						GrammarResolver.RuleEntry ruleEntry;
						List<string> list;
						string str;
						for (;;)
						{
							ruleEntry = GrammarResolver.RandomPossiblyResolvableEntry(text3, constants, extraTags, resolvedTags);
							if (ruleEntry == null)
							{
								break;
							}
							ruleEntry.uses++;
							list = resolvedTags.ToList<string>();
							if (GrammarResolver.TryResolveRecursive(ruleEntry, depth + 1, constants, out str, log, extraTags, list))
							{
								goto Block_14;
							}
							ruleEntry.MarkKnownUnresolvable();
						}
						entry.MarkKnownUnresolvable();
						output = "CANNOT_RESOLVE_SUBSYMBOL:" + text3;
						if (log)
						{
							GrammarResolver.logSbTrace.Append("\n" + text + text3 + " → UNRESOLVABLE");
						}
						flag = true;
						goto IL_271;
						Block_14:
						text2 = text2.Substring(0, num) + str + text2.Substring(j + 1);
						j = num;
						resolvedTags.Clear();
						resolvedTags.AddRange(list);
						if (!ruleEntry.rule.tag.NullOrEmpty() && !resolvedTags.Contains(ruleEntry.rule.tag))
						{
							resolvedTags.Add(ruleEntry.rule.tag);
						}
					}
				}
				IL_271:;
			}
			output = text2;
			return !flag;
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000D75E0 File Offset: 0x000D57E0
		private static GrammarResolver.RuleEntry RandomPossiblyResolvableEntry(string keyword, Dictionary<string, string> constants, List<string> extraTags, List<string> resolvedTags)
		{
			List<GrammarResolver.RuleEntry> list = GrammarResolver.rules.TryGetValue(keyword, null);
			if (list == null)
			{
				return null;
			}
			float maxPriority = float.MinValue;
			for (int i = 0; i < list.Count; i++)
			{
				GrammarResolver.RuleEntry ruleEntry = list[i];
				if (!ruleEntry.knownUnresolvable && ruleEntry.ValidateConstantConstraints(constants) && ruleEntry.ValidateRequiredTag(extraTags, resolvedTags) && ruleEntry.SelectionWeight != 0f)
				{
					maxPriority = Mathf.Max(maxPriority, ruleEntry.Priority);
				}
			}
			return list.RandomElementByWeightWithFallback(delegate(GrammarResolver.RuleEntry rule)
			{
				if (rule.knownUnresolvable || !rule.ValidateConstantConstraints(constants) || !rule.ValidateRequiredTag(extraTags, resolvedTags) || rule.Priority != maxPriority)
				{
					return 0f;
				}
				return rule.SelectionWeight;
			}, null);
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x000D769F File Offset: 0x000D589F
		public static bool ContainsSpecialChars(string str)
		{
			return str.IndexOfAny(GrammarResolver.SpecialChars) >= 0;
		}

		// Token: 0x040015B0 RID: 5552
		private static SimpleLinearPool<List<GrammarResolver.RuleEntry>> rulePool = new SimpleLinearPool<List<GrammarResolver.RuleEntry>>();

		// Token: 0x040015B1 RID: 5553
		private static Dictionary<string, List<GrammarResolver.RuleEntry>> rules = new Dictionary<string, List<GrammarResolver.RuleEntry>>();

		// Token: 0x040015B2 RID: 5554
		private static int loopCount;

		// Token: 0x040015B3 RID: 5555
		private static StringBuilder logSbTrace;

		// Token: 0x040015B4 RID: 5556
		private static StringBuilder logSbMid;

		// Token: 0x040015B5 RID: 5557
		private static StringBuilder logSbRules;

		// Token: 0x040015B6 RID: 5558
		private const int DepthLimit = 50;

		// Token: 0x040015B7 RID: 5559
		private const int LoopsLimit = 1000;

		// Token: 0x040015B8 RID: 5560
		private static Regex Spaces = new Regex(" +([,.])");

		// Token: 0x040015B9 RID: 5561
		public const char SymbolStartChar = '[';

		// Token: 0x040015BA RID: 5562
		public const char SymbolEndChar = ']';

		// Token: 0x040015BB RID: 5563
		private static readonly char[] SpecialChars = new char[]
		{
			'[',
			']',
			'{',
			'}'
		};

		// Token: 0x020016C6 RID: 5830
		private class RuleEntry
		{
			// Token: 0x1700151A RID: 5402
			// (get) Token: 0x060085C7 RID: 34247 RVA: 0x002B3475 File Offset: 0x002B1675
			public float SelectionWeight
			{
				get
				{
					return this.rule.BaseSelectionWeight * 100000f / (float)((this.uses + 1) * 1000);
				}
			}

			// Token: 0x1700151B RID: 5403
			// (get) Token: 0x060085C8 RID: 34248 RVA: 0x002B3498 File Offset: 0x002B1698
			public float Priority
			{
				get
				{
					return this.rule.Priority;
				}
			}

			// Token: 0x060085C9 RID: 34249 RVA: 0x002B34A5 File Offset: 0x002B16A5
			public RuleEntry(Rule rule)
			{
				this.rule = rule;
				this.knownUnresolvable = false;
			}

			// Token: 0x060085CA RID: 34250 RVA: 0x002B34BB File Offset: 0x002B16BB
			public void MarkKnownUnresolvable()
			{
				this.knownUnresolvable = true;
			}

			// Token: 0x060085CB RID: 34251 RVA: 0x002B34C4 File Offset: 0x002B16C4
			public bool ValidateConstantConstraints(Dictionary<string, string> constraints)
			{
				if (!this.constantConstraintsChecked)
				{
					this.constantConstraintsValid = true;
					if (this.rule.constantConstraints != null)
					{
						for (int i = 0; i < this.rule.constantConstraints.Count; i++)
						{
							Rule.ConstantConstraint constantConstraint = this.rule.constantConstraints[i];
							string text = (constraints != null) ? constraints.TryGetValue(constantConstraint.key, "") : "";
							float num = 0f;
							float num2 = 0f;
							bool flag = !text.NullOrEmpty() && !constantConstraint.value.NullOrEmpty() && float.TryParse(text, out num) && float.TryParse(constantConstraint.value, out num2);
							bool flag2;
							switch (constantConstraint.type)
							{
							case Rule.ConstantConstraint.Type.Equal:
								flag2 = text.EqualsIgnoreCase(constantConstraint.value);
								break;
							case Rule.ConstantConstraint.Type.NotEqual:
								flag2 = !text.EqualsIgnoreCase(constantConstraint.value);
								break;
							case Rule.ConstantConstraint.Type.Less:
								flag2 = (flag && num < num2);
								break;
							case Rule.ConstantConstraint.Type.Greater:
								flag2 = (flag && num > num2);
								break;
							case Rule.ConstantConstraint.Type.LessOrEqual:
								flag2 = (flag && num <= num2);
								break;
							case Rule.ConstantConstraint.Type.GreaterOrEqual:
								flag2 = (flag && num >= num2);
								break;
							default:
								Log.Error("Unknown ConstantConstraint type: " + constantConstraint.type, false);
								flag2 = false;
								break;
							}
							if (!flag2)
							{
								this.constantConstraintsValid = false;
								break;
							}
						}
					}
					this.constantConstraintsChecked = true;
				}
				return this.constantConstraintsValid;
			}

			// Token: 0x060085CC RID: 34252 RVA: 0x002B364A File Offset: 0x002B184A
			public bool ValidateRequiredTag(List<string> extraTags, List<string> resolvedTags)
			{
				return this.rule.requiredTag.NullOrEmpty() || (extraTags != null && extraTags.Contains(this.rule.requiredTag)) || resolvedTags.Contains(this.rule.requiredTag);
			}

			// Token: 0x060085CD RID: 34253 RVA: 0x002B3689 File Offset: 0x002B1889
			public override string ToString()
			{
				return this.rule.ToString();
			}

			// Token: 0x04005748 RID: 22344
			public Rule rule;

			// Token: 0x04005749 RID: 22345
			public bool knownUnresolvable;

			// Token: 0x0400574A RID: 22346
			public bool constantConstraintsChecked;

			// Token: 0x0400574B RID: 22347
			public bool constantConstraintsValid;

			// Token: 0x0400574C RID: 22348
			public int uses;
		}
	}
}
