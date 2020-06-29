using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{

	public static class GrammarResolverSimple
	{

		private static bool working;


		private static StringBuilder tmpResultBuffer = new StringBuilder();

		private static StringBuilder tmpSymbolBuffer = new StringBuilder();


		private static StringBuilder tmpSymbolBuffer_objectLabel = new StringBuilder();


		private static StringBuilder tmpSymbolBuffer_subSymbol = new StringBuilder();


		private static StringBuilder tmpSymbolBuffer_args = new StringBuilder();


		private static List<string> tmpArgsLabels = new List<string>();

		private static List<object> tmpArgsObjects = new List<object>();

		private static StringBuilder tmpArg = new StringBuilder();

		public static TaggedString Formatted(TaggedString str, List<string> argsLabelsArg, List<object> argsObjectsArg)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			bool flag;
			StringBuilder stringBuilder;
			StringBuilder stringBuilder2;
			StringBuilder stringBuilder3;
			StringBuilder stringBuilder4;
			StringBuilder stringBuilder5;
			List<string> list;
			List<object> list2;
			if (GrammarResolverSimple.working)
			{
				flag = false;
				stringBuilder = new StringBuilder();
				stringBuilder2 = new StringBuilder();
				stringBuilder3 = new StringBuilder();
				stringBuilder4 = new StringBuilder();
				stringBuilder5 = new StringBuilder();
				list = argsLabelsArg.ToList<string>();
				list2 = argsObjectsArg.ToList<object>();
			}
			else
			{
				flag = true;
				stringBuilder = GrammarResolverSimple.tmpResultBuffer;
				stringBuilder2 = GrammarResolverSimple.tmpSymbolBuffer;
				stringBuilder3 = GrammarResolverSimple.tmpSymbolBuffer_objectLabel;
				stringBuilder4 = GrammarResolverSimple.tmpSymbolBuffer_subSymbol;
				stringBuilder5 = GrammarResolverSimple.tmpSymbolBuffer_args;
				list = GrammarResolverSimple.tmpArgsLabels;
				list.Clear();
				list.AddRange(argsLabelsArg);
				list2 = GrammarResolverSimple.tmpArgsObjects;
				list2.Clear();
				list2.AddRange(argsObjectsArg);
			}
			if (flag)
			{
				GrammarResolverSimple.working = true;
			}
			TaggedString result;
			try
			{
				stringBuilder.Length = 0;
				for (int i = 0; i < str.Length; i++)
				{
					char c = str[i];
					if (c == '{')
					{
						stringBuilder2.Length = 0;
						stringBuilder3.Length = 0;
						stringBuilder4.Length = 0;
						stringBuilder5.Length = 0;
						bool flag2 = false;
						bool flag3 = false;
						bool flag4 = false;
						i++;
						bool flag5 = i < str.Length && str[i] == '{';
						while (i < str.Length)
						{
							char c2 = str[i];
							if (c2 == '}')
							{
								flag2 = true;
								break;
							}
							stringBuilder2.Append(c2);
							if (c2 == '_' && !flag3)
							{
								flag3 = true;
							}
							else if (c2 == '?' && !flag4)
							{
								flag4 = true;
							}
							else if (flag4)
							{
								stringBuilder5.Append(c2);
							}
							else if (flag3)
							{
								stringBuilder4.Append(c2);
							}
							else
							{
								stringBuilder3.Append(c2);
							}
							i++;
						}
						if (!flag2)
						{
							Log.ErrorOnce("Could not find matching '}' in \"" + str + "\".", str.GetHashCode() ^ 194857261, false);
						}
						else if (flag5)
						{
							stringBuilder.Append(stringBuilder2);
						}
						else
						{
							if (flag4)
							{
								while (stringBuilder4.Length != 0 && stringBuilder4[stringBuilder4.Length - 1] == ' ')
								{
									StringBuilder stringBuilder6 = stringBuilder4;
									int length = stringBuilder6.Length;
									stringBuilder6.Length = length - 1;
								}
							}
							string text = stringBuilder3.ToString();
							bool flag6 = false;
							int num = -1;
							if (int.TryParse(text, out num))
							{
								TaggedString taggedString;
								if (num >= 0 && num < list2.Count && GrammarResolverSimple.TryResolveSymbol(list2[num], stringBuilder4.ToString(), stringBuilder5.ToString(), out taggedString, str))
								{
									flag6 = true;
									stringBuilder.Append(taggedString.RawText);
								}
							}
							else
							{
								int j = 0;
								while (j < list.Count)
								{
									if (list[j] == text)
									{
										TaggedString taggedString2;
										if (GrammarResolverSimple.TryResolveSymbol(list2[j], stringBuilder4.ToString(), stringBuilder5.ToString(), out taggedString2, str))
										{
											flag6 = true;
											stringBuilder.Append(taggedString2.RawText);
											break;
										}
										break;
									}
									else
									{
										j++;
									}
								}
							}
							if (!flag6)
							{
								Log.ErrorOnce("Could not resolve symbol \"" + stringBuilder2 + "\" for string \"" + str + "\".", str.GetHashCode() ^ stringBuilder2.ToString().GetHashCode() ^ 879654654, false);
							}
						}
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				string text2 = GenText.CapitalizeSentences(stringBuilder.ToString(), false);
				text2 = Find.ActiveLanguageWorker.PostProcessedKeyedTranslation(text2);
				result = text2;
			}
			finally
			{
				if (flag)
				{
					GrammarResolverSimple.working = false;
				}
			}
			return result;
		}

		private static bool TryResolveSymbol(object obj, string subSymbol, string symbolArgs, out TaggedString resolvedStr, string fullStringForReference)
		{
            resolvedStr = default;
            //Pawn pawn = obj as Pawn;
            //if (pawn != null)
            //{
            //	uint num = PrivateImplementationDetails.ComputeStringHash(subSymbol);
            //	if (num <= 2306218066u)
            //	{
            //		if (num <= 1147977518u)
            //		{
            //			if (num <= 543181407u)
            //			{
            //				if (num <= 267723693u)
            //				{
            //					if (num != 176126825u)
            //					{
            //						if (num != 238594642u)
            //						{
            //							if (num == 267723693u)
            //							{
            //								if (subSymbol == "nameDef")
            //								{
            //									resolvedStr = (pawn.Name != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelDefinite();
            //									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //									return true;
            //								}
            //							}
            //						}
            //						else if (subSymbol == "factionPawnSingularIndef")
            //						{
            //							resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Faction.def.pawnSingular, false, false) : "");
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //					else if (subSymbol == "kindPlural")
            //					{
            //						resolvedStr = pawn.GetKindLabelPlural(-1);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //				else if (num != 356082287u)
            //				{
            //					if (num != 418492385u)
            //					{
            //						if (num == 543181407u)
            //						{
            //							if (subSymbol == "lifeStageIndef")
            //							{
            //								resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.ageTracker.CurLifeStage.label, pawn.gender, false, false);
            //								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //								return true;
            //							}
            //						}
            //					}
            //					else if (subSymbol == "definite")
            //					{
            //						resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelDefinite());
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //				else if (subSymbol == "nameFull")
            //				{
            //					resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringFull, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite());
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (num <= 667530978u)
            //			{
            //				if (num != 575730602u)
            //				{
            //					if (num != 658875246u)
            //					{
            //						if (num == 667530978u)
            //						{
            //							if (subSymbol == "lifeStageAdjective")
            //							{
            //								resolvedStr = pawn.ageTracker.CurLifeStage.Adjective;
            //								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //								return true;
            //							}
            //						}
            //					}
            //					else if (subSymbol == "relationInfoInParentheses")
            //					{
            //						resolvedStr = "";
            //						PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref resolvedStr, pawn);
            //						if (!resolvedStr.NullOrEmpty())
            //						{
            //							resolvedStr = "(" + resolvedStr + ")";
            //						}
            //						return true;
            //					}
            //				}
            //				else if (subSymbol == "lifeStageDef")
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.ageTracker.CurLifeStage.label, pawn.gender, false, false);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (num <= 861101311u)
            //			{
            //				if (num != 742476188u)
            //				{
            //					if (num == 861101311u)
            //					{
            //						if (subSymbol == "factionPawnsPluralDef")
            //						{
            //							resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(pawn.Faction.def.pawnsPlural, pawn.Faction.def.pawnSingular), true, false) : "");
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "age")
            //				{
            //					resolvedStr = pawn.ageTracker.AgeBiologicalYears.ToString();
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (num != 998961680u)
            //			{
            //				if (num == 1147977518u)
            //				{
            //					if (subSymbol == "nameFullDef")
            //					{
            //						resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Name.ToStringFull, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelDefinite());
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "nameIndef")
            //			{
            //				resolvedStr = (pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite();
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num <= 1653343472u)
            //		{
            //			if (num <= 1277025515u)
            //			{
            //				if (num != 1162320608u)
            //				{
            //					if (num != 1167748615u)
            //					{
            //						if (num == 1277025515u)
            //						{
            //							if (subSymbol == "possessive")
            //							{
            //								resolvedStr = pawn.gender.GetPossessive();
            //								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //								return true;
            //							}
            //						}
            //					}
            //					else if (subSymbol == "kindIndef")
            //					{
            //						resolvedStr = pawn.KindLabelIndefinite();
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //				else if (subSymbol == "factionName")
            //				{
            //					resolvedStr = ((pawn.Faction != null) ? pawn.Faction.Name.ApplyTag(pawn.Faction) : "");
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (num <= 1387836843u)
            //			{
            //				if (num != 1365350650u)
            //				{
            //					if (num == 1387836843u)
            //					{
            //						if (subSymbol == "lifeStage")
            //						{
            //							resolvedStr = pawn.ageTracker.CurLifeStage.label;
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "royalTitleInCurrentFaction")
            //				{
            //					resolvedStr = GrammarResolverSimple.PawnResolveRoyalTitleInCurrentFaction(pawn);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (num != 1587320192u)
            //			{
            //				if (num == 1653343472u)
            //				{
            //					if (subSymbol == "kindPluralDef")
            //					{
            //						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.GetKindLabelPlural(-1), pawn.gender, true, false);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "gender")
            //			{
            //				resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(pawn.gender, pawn.RaceProps.Animal, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num <= 1911534845u)
            //		{
            //			if (num != 1670603679u)
            //			{
            //				if (num != 1691639576u)
            //				{
            //					if (num == 1911534845u)
            //					{
            //						if (subSymbol == "labelShort")
            //						{
            //							resolvedStr = ((pawn.Name != null) ? pawn.Name.ToStringShort.ApplyTag(TagType.Name, null) : pawn.KindLabel);
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "factionRoyalFavorLabel")
            //				{
            //					resolvedStr = ((pawn.Faction != null) ? pawn.Faction.def.royalFavorLabel : "");
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (subSymbol == "raceDef")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.def.label, false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num <= 2166136261u)
            //		{
            //			if (num != 1998225958u)
            //			{
            //				if (num == 2166136261u)
            //				{
            //					if (subSymbol != null)
            //					{
            //						if (subSymbol.Length == 0)
            //						{
            //							resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite());
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //			}
            //			else if (subSymbol == "factionPawnsPluralIndef")
            //			{
            //				resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(pawn.Faction.def.pawnsPlural, pawn.Faction.def.pawnSingular), true, false) : "");
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 2279990553u)
            //		{
            //			if (num == 2306218066u)
            //			{
            //				if (subSymbol == "indefinite")
            //				{
            //					resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite());
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "relationInfo")
            //		{
            //			resolvedStr = "";
            //			TaggedString taggedString = resolvedStr;
            //			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
            //			resolvedStr = taggedString.RawText;
            //			return true;
            //		}
            //	}
            //	else if (num <= 3317904369u)
            //	{
            //		if (num <= 2740648940u)
            //		{
            //			if (num <= 2528592613u)
            //			{
            //				if (num != 2360586432u)
            //				{
            //					if (num != 2394669720u)
            //					{
            //						if (num == 2528592613u)
            //						{
            //							if (subSymbol == "kindBaseDef")
            //							{
            //								resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.kindDef.label, false, false);
            //								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //								return true;
            //							}
            //						}
            //					}
            //					else if (subSymbol == "race")
            //					{
            //						resolvedStr = pawn.def.label;
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //				else if (subSymbol == "kindBasePlural")
            //				{
            //					resolvedStr = pawn.kindDef.GetLabelPlural(-1);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (num <= 2605899927u)
            //			{
            //				if (num != 2556802313u)
            //				{
            //					if (num == 2605899927u)
            //					{
            //						if (subSymbol == "kindBasePluralDef")
            //						{
            //							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawn.kindDef.GetLabelPlural(-1), pawn.kindDef.label), true, false);
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "title")
            //				{
            //					resolvedStr = ((pawn.story != null) ? pawn.story.Title : "");
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (num != 2618666040u)
            //			{
            //				if (num == 2740648940u)
            //				{
            //					if (subSymbol == "factionPawnSingular")
            //					{
            //						resolvedStr = ((pawn.Faction != null) ? pawn.Faction.def.pawnSingular : "");
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "objective")
            //			{
            //				resolvedStr = pawn.gender.GetObjective();
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num <= 2994657680u)
            //		{
            //			if (num != 2835508048u)
            //			{
            //				if (num != 2892888801u)
            //				{
            //					if (num == 2994657680u)
            //					{
            //						if (subSymbol == "bestRoyalTitle")
            //						{
            //							resolvedStr = GrammarResolverSimple.PawnResolveBestRoyalTitle(pawn);
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "kindPluralIndef")
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.GetKindLabelPlural(-1), pawn.gender, true, false);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (subSymbol == "titleDef")
            //			{
            //				resolvedStr = ((pawn.story != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.story.Title, pawn.gender, false, false) : "");
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num <= 3124331847u)
            //		{
            //			if (num != 3109671438u)
            //			{
            //				if (num == 3124331847u)
            //				{
            //					if (subSymbol == "bestRoyalTitleDef")
            //					{
            //						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(GrammarResolverSimple.PawnResolveBestRoyalTitle(pawn), false, false);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "bestRoyalTitleIndef")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(GrammarResolverSimple.PawnResolveBestRoyalTitle(pawn), false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 3276274232u)
            //		{
            //			if (num == 3317904369u)
            //			{
            //				if (subSymbol == "humanlike")
            //				{
            //					resolvedStr = GrammarResolverSimple.ResolveHumanlikeSymbol(pawn.RaceProps.Humanlike, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "chronologicalAge")
            //		{
            //			resolvedStr = pawn.ageTracker.AgeChronologicalYears.ToString();
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 3868512966u)
            //	{
            //		if (num <= 3641958979u)
            //		{
            //			if (num != 3444987233u)
            //			{
            //				if (num != 3638871208u)
            //				{
            //					if (num == 3641958979u)
            //					{
            //						if (subSymbol == "kind")
            //						{
            //							resolvedStr = pawn.KindLabel;
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "kindBaseIndef")
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.kindDef.label, false, false);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (subSymbol == "ageFull")
            //			{
            //				resolvedStr = pawn.ageTracker.AgeNumberString;
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num <= 3802171214u)
            //		{
            //			if (num != 3651983315u)
            //			{
            //				if (num == 3802171214u)
            //				{
            //					if (subSymbol == "kindBase")
            //					{
            //						resolvedStr = pawn.kindDef.label;
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "factionPawnSingularDef")
            //			{
            //				resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Faction.def.pawnSingular, false, false) : "");
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 3866324033u)
            //		{
            //			if (num == 3868512966u)
            //			{
            //				if (subSymbol == "raceIndef")
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.def.label, false, false);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "titleIndef")
            //		{
            //			resolvedStr = ((pawn.story != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.story.Title, pawn.gender, false, false) : "");
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 4044507857u)
            //	{
            //		if (num != 3976846386u)
            //		{
            //			if (num != 3996112312u)
            //			{
            //				if (num == 4044507857u)
            //				{
            //					if (subSymbol == "royalTitleInCurrentFactionDef")
            //					{
            //						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(GrammarResolverSimple.PawnResolveRoyalTitleInCurrentFaction(pawn), false, false);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "factionPawnsPlural")
            //			{
            //				resolvedStr = ((pawn.Faction != null) ? pawn.Faction.def.pawnsPlural : "");
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (subSymbol == "kindDef")
            //		{
            //			resolvedStr = pawn.KindLabelDefinite();
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 4062297208u)
            //	{
            //		if (num != 4059209310u)
            //		{
            //			if (num == 4062297208u)
            //			{
            //				if (subSymbol == "pronoun")
            //				{
            //					resolvedStr = pawn.gender.GetPronoun();
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "kindBasePluralIndef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawn.kindDef.GetLabelPlural(-1), pawn.kindDef.label), true, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num != 4137097213u)
            //	{
            //		if (num == 4201427756u)
            //		{
            //			if (subSymbol == "royalTitleInCurrentFactionIndef")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(GrammarResolverSimple.PawnResolveRoyalTitleInCurrentFaction(pawn), false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //	}
            //	else if (subSymbol == "label")
            //	{
            //		resolvedStr = pawn.LabelNoCountColored;
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	resolvedStr = "";
            //	return false;
            //}
            //Thing thing = obj as Thing;
            //if (thing != null)
            //{
            //	uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
            //	if (num <= 1911534845u)
            //	{
            //		if (num <= 1277025515u)
            //		{
            //			if (num != 418492385u)
            //			{
            //				if (num != 1162320608u)
            //				{
            //					if (num == 1277025515u)
            //					{
            //						if (subSymbol == "possessive")
            //						{
            //							resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null).GetPossessive();
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "factionName")
            //				{
            //					resolvedStr = ((thing.Faction != null) ? thing.Faction.Name : "");
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (subSymbol == "definite")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(thing.Label, false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 1339988243u)
            //		{
            //			if (num != 1587320192u)
            //			{
            //				if (num == 1911534845u)
            //				{
            //					if (subSymbol == "labelShort")
            //					{
            //						resolvedStr = thing.LabelShort;
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "gender")
            //			{
            //				resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null), false, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (subSymbol == "labelPluralIndef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), thing.LabelNoCount), true, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 2306218066u)
            //	{
            //		if (num != 2084067798u)
            //		{
            //			if (num != 2166136261u)
            //			{
            //				if (num == 2306218066u)
            //				{
            //					if (subSymbol == "indefinite")
            //					{
            //						resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.Label, false, false);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol != null)
            //			{
            //				if (subSymbol.Length == 0)
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.Label, false, false);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "labelPluralDef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), thing.LabelNoCount), true, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 4062297208u)
            //	{
            //		if (num != 2618666040u)
            //		{
            //			if (num == 4062297208u)
            //			{
            //				if (subSymbol == "pronoun")
            //				{
            //					resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null).GetPronoun();
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "objective")
            //		{
            //			resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null).GetObjective();
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num != 4137097213u)
            //	{
            //		if (num == 4252169255u)
            //		{
            //			if (subSymbol == "labelPlural")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //	}
            //	else if (subSymbol == "label")
            //	{
            //		resolvedStr = thing.Label;
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	resolvedStr = "";
            //	return false;
            //}
            //Hediff hediff = obj as Hediff;
            //if (hediff != null)
            //{
            //	if (subSymbol == "label")
            //	{
            //		resolvedStr = hediff.Label;
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	if (subSymbol == "labelNoun")
            //	{
            //		resolvedStr = ((!hediff.def.labelNoun.NullOrEmpty()) ? hediff.def.labelNoun : hediff.Label);
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //}
            //WorldObject worldObject = obj as WorldObject;
            //if (worldObject != null)
            //{
            //	uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
            //	if (num <= 2084067798u)
            //	{
            //		if (num <= 1277025515u)
            //		{
            //			if (num != 418492385u)
            //			{
            //				if (num != 1162320608u)
            //				{
            //					if (num == 1277025515u)
            //					{
            //						if (subSymbol == "possessive")
            //						{
            //							resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null).GetPossessive();
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //				else if (subSymbol == "factionName")
            //				{
            //					resolvedStr = ((worldObject.Faction != null) ? worldObject.Faction.Name.ApplyTag(worldObject.Faction) : "");
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //			else if (subSymbol == "definite")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(worldObject.Label, false, worldObject.HasName);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 1339988243u)
            //		{
            //			if (num != 1587320192u)
            //			{
            //				if (num == 2084067798u)
            //				{
            //					if (subSymbol == "labelPluralDef")
            //					{
            //						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), worldObject.Label), true, worldObject.HasName);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "gender")
            //			{
            //				resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null), false, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (subSymbol == "labelPluralIndef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), worldObject.Label), true, worldObject.HasName);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 2618666040u)
            //	{
            //		if (num != 2166136261u)
            //		{
            //			if (num != 2306218066u)
            //			{
            //				if (num == 2618666040u)
            //				{
            //					if (subSymbol == "objective")
            //					{
            //						resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null).GetObjective();
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "indefinite")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(worldObject.Label, false, worldObject.HasName);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (subSymbol != null)
            //		{
            //			if (subSymbol.Length == 0)
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(worldObject.Label, false, worldObject.HasName);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //	}
            //	else if (num != 4062297208u)
            //	{
            //		if (num != 4137097213u)
            //		{
            //			if (num == 4252169255u)
            //			{
            //				if (subSymbol == "labelPlural")
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "label")
            //		{
            //			resolvedStr = worldObject.Label;
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (subSymbol == "pronoun")
            //	{
            //		resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null).GetPronoun();
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	resolvedStr = "";
            //	return false;
            //}
            //Faction faction = obj as Faction;
            //if (faction != null)
            //{
            //	uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
            //	if (num <= 2166136261u)
            //	{
            //		if (num <= 1812998298u)
            //		{
            //			if (num != 493124349u)
            //			{
            //				if (num == 1812998298u)
            //				{
            //					if (subSymbol == "royalFavorLabel")
            //					{
            //						resolvedStr = faction.def.royalFavorLabel;
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "pawnsPluralDef")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), true, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 2028987726u)
            //		{
            //			if (num == 2166136261u)
            //			{
            //				if (subSymbol != null)
            //				{
            //					if (subSymbol.Length == 0)
            //					{
            //						resolvedStr = faction.Name.ApplyTag(faction);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //		}
            //		else if (subSymbol == "pawnSingular")
            //		{
            //			resolvedStr = faction.def.pawnSingular;
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 2461125861u)
            //	{
            //		if (num != 2369371622u)
            //		{
            //			if (num == 2461125861u)
            //			{
            //				if (subSymbol == "pawnSingularDef")
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnSingular, false, false);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "name")
            //		{
            //			resolvedStr = faction.Name.ApplyTag(faction);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num != 2873559712u)
            //	{
            //		if (num != 2892717736u)
            //		{
            //			if (num == 2965909334u)
            //			{
            //				if (subSymbol == "pawnsPlural")
            //				{
            //					resolvedStr = faction.def.pawnsPlural;
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "pawnSingularIndef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnSingular, false, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (subSymbol == "pawnsPluralIndef")
            //	{
            //		resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), true, false);
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	resolvedStr = "";
            //	return false;
            //}
            //Def def = obj as Def;
            //if (def != null)
            //{
            //	PawnKindDef pawnKindDef = def as PawnKindDef;
            //	if (pawnKindDef != null)
            //	{
            //		if (subSymbol == "labelPlural")
            //		{
            //			resolvedStr = pawnKindDef.GetLabelPlural(-1);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //		if (subSymbol == "labelPluralDef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawnKindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawnKindDef.GetLabelPlural(-1), pawnKindDef.label), true, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //		if (subSymbol == "labelPluralIndef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawnKindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawnKindDef.GetLabelPlural(-1), pawnKindDef.label), true, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
            //	if (num <= 2084067798u)
            //	{
            //		if (num <= 1277025515u)
            //		{
            //			if (num != 418492385u)
            //			{
            //				if (num == 1277025515u)
            //				{
            //					if (subSymbol == "possessive")
            //					{
            //						resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(def.label, null).GetPossessive();
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "definite")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(def.label, false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 1339988243u)
            //		{
            //			if (num != 1587320192u)
            //			{
            //				if (num == 2084067798u)
            //				{
            //					if (subSymbol == "labelPluralDef")
            //					{
            //						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(def.label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(def.label, -1), def.label), true, false);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "gender")
            //			{
            //				resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(def.label, null), false, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (subSymbol == "labelPluralIndef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(def.label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(def.label, -1), def.label), true, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 2618666040u)
            //	{
            //		if (num != 2166136261u)
            //		{
            //			if (num != 2306218066u)
            //			{
            //				if (num == 2618666040u)
            //				{
            //					if (subSymbol == "objective")
            //					{
            //						resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(def.label, null).GetObjective();
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "indefinite")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label, false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (subSymbol != null)
            //		{
            //			if (subSymbol.Length == 0)
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label, false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //	}
            //	else if (num != 4062297208u)
            //	{
            //		if (num != 4137097213u)
            //		{
            //			if (num == 4252169255u)
            //			{
            //				if (subSymbol == "labelPlural")
            //				{
            //					resolvedStr = Find.ActiveLanguageWorker.Pluralize(def.label, -1);
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "label")
            //		{
            //			resolvedStr = def.label;
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (subSymbol == "pronoun")
            //	{
            //		resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(def.label, null).GetPronoun();
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	resolvedStr = "";
            //	return false;
            //}
            //string text = obj as string;
            //if (text != null)
            //{
            //	uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
            //	if (num <= 2166136261u)
            //	{
            //		if (num <= 686961615u)
            //		{
            //			if (num != 418492385u)
            //			{
            //				if (num == 686961615u)
            //				{
            //					if (subSymbol == "plural")
            //					{
            //						resolvedStr = Find.ActiveLanguageWorker.Pluralize(text, -1);
            //						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //						return true;
            //					}
            //				}
            //			}
            //			else if (subSymbol == "definite")
            //			{
            //				resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(text, false, false);
            //				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (num != 1277025515u)
            //		{
            //			if (num != 1587320192u)
            //			{
            //				if (num == 2166136261u)
            //				{
            //					if (subSymbol != null)
            //					{
            //						if (subSymbol.Length == 0)
            //						{
            //							resolvedStr = text;
            //							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //							return true;
            //						}
            //					}
            //				}
            //			}
            //			else if (subSymbol == "gender")
            //			{
            //				resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(text, null), false, symbolArgs, fullStringForReference);
            //				return true;
            //			}
            //		}
            //		else if (subSymbol == "possessive")
            //		{
            //			resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(text, null).GetPossessive();
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num <= 2618666040u)
            //	{
            //		if (num != 2306218066u)
            //		{
            //			if (num == 2618666040u)
            //			{
            //				if (subSymbol == "objective")
            //				{
            //					resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(text, null).GetObjective();
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "indefinite")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(text, false, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (num != 3774422699u)
            //	{
            //		if (num != 3784914766u)
            //		{
            //			if (num == 4062297208u)
            //			{
            //				if (subSymbol == "pronoun")
            //				{
            //					resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(text, null).GetPronoun();
            //					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //					return true;
            //				}
            //			}
            //		}
            //		else if (subSymbol == "pluralDef")
            //		{
            //			resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(text, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(text, -1), text), true, false);
            //			GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //			return true;
            //		}
            //	}
            //	else if (subSymbol == "pluralIndef")
            //	{
            //		resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(text, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(text, -1), text), true, false);
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	resolvedStr = "";
            //	return false;
            //}
            //if (obj is int || obj is long)
            //{
            //	int number = (obj is int) ? ((int)obj) : ((int)((long)obj));
            //	if (subSymbol != null && subSymbol.Length == 0)
            //	{
            //		resolvedStr = number.ToString();
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		return true;
            //	}
            //	if (!(subSymbol == "ordinal"))
            //	{
            //		resolvedStr = "";
            //		return false;
            //	}
            //	resolvedStr = Find.ActiveLanguageWorker.OrdinalNumber(number, Gender.None).ToString();
            //	GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //	return true;
            //}
            //else
            //{
            //	if (obj is TaggedString)
            //	{
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		resolvedStr = ((TaggedString)obj).RawText;
            //	}
            //	if (subSymbol.NullOrEmpty())
            //	{
            //		GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
            //		if (obj == null)
            //		{
            //			resolvedStr = "";
            //		}
            //		else
            //		{
            //			resolvedStr = obj.ToString();
            //		}
            //		return true;
            //	}
            //	resolvedStr = "";
            //	return false;
            //}
            return false;
		}

		private static void EnsureNoArgs(string subSymbol, string symbolArgs, string fullStringForReference)
		{
			if (!symbolArgs.NullOrEmpty())
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Symbol \"",
					subSymbol,
					"\" doesn't expect any args but \"",
					symbolArgs,
					"\" args were provided. Full string: \"",
					fullStringForReference,
					"\"."
				}), subSymbol.GetHashCode() ^ symbolArgs.GetHashCode() ^ fullStringForReference.GetHashCode() ^ 958090126, false);
			}
		}

		
		private static string ResolveGenderSymbol(Gender gender, bool animal, string args, string fullStringForReference)
		{
			if (args.NullOrEmpty())
			{
				return gender.GetLabel(animal);
			}
			int argsCount = GrammarResolverSimple.GetArgsCount(args);
			if (argsCount == 2)
			{
				switch (gender)
				{
				case Gender.None:
					return GrammarResolverSimple.GetArg(args, 0);
				case Gender.Male:
					return GrammarResolverSimple.GetArg(args, 0);
				case Gender.Female:
					return GrammarResolverSimple.GetArg(args, 1);
				default:
					return "";
				}
			}
			else
			{
				if (argsCount != 3)
				{
					Log.ErrorOnce("Invalid args count in \"" + fullStringForReference + "\" for symbol \"gender\".", args.GetHashCode() ^ fullStringForReference.GetHashCode() ^ 787618371, false);
					return "";
				}
				switch (gender)
				{
				case Gender.None:
					return GrammarResolverSimple.GetArg(args, 2);
				case Gender.Male:
					return GrammarResolverSimple.GetArg(args, 0);
				case Gender.Female:
					return GrammarResolverSimple.GetArg(args, 1);
				default:
					return "";
				}
			}
		}

		
		private static string ResolveHumanlikeSymbol(bool humanlike, string args, string fullStringForReference)
		{
			if (GrammarResolverSimple.GetArgsCount(args) != 2)
			{
				Log.ErrorOnce("Invalid args count in \"" + fullStringForReference + "\" for symbol \"humanlike\".", args.GetHashCode() ^ fullStringForReference.GetHashCode() ^ 895109845, false);
				return "";
			}
			if (humanlike)
			{
				return GrammarResolverSimple.GetArg(args, 0);
			}
			return GrammarResolverSimple.GetArg(args, 1);
		}

		
		private static int GetArgsCount(string args)
		{
			int num = 1;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == ':')
				{
					num++;
				}
			}
			return num;
		}


		private static string GetArg(string args, int argIndex)
		{
            //GrammarResolverSimple.tmpArg.Length = 0;
            //int num = 0;
            //foreach (char c in args)
            //{
            //	if (c == ':')
            //	{
            //		num++;
            //	}
            //	else if (num == argIndex)
            //	{
            //		GrammarResolverSimple.tmpArg.Append(c);
            //	}
            //	else if (num > argIndex)
            //	{
            //		IL_56:
            //		while (GrammarResolverSimple.tmpArg.Length != 0)
            //		{
            //			if (GrammarResolverSimple.tmpArg[0] != ' ')
            //			{
            //				break;
            //			}
            //			GrammarResolverSimple.tmpArg.Remove(0, 1);
            //		}
            //		while (GrammarResolverSimple.tmpArg.Length != 0 && GrammarResolverSimple.tmpArg[GrammarResolverSimple.tmpArg.Length - 1] == ' ')
            //		{
            //			StringBuilder stringBuilder = GrammarResolverSimple.tmpArg;
            //			int length = stringBuilder.Length;
            //			stringBuilder.Length = length - 1;
            //		}
            //		return GrammarResolverSimple.tmpArg.ToString();
            //	}
            //}
            //goto IL_56;
            return "";
		}

		
		public static string PawnResolveBestRoyalTitle(Pawn pawn)
		{
			if (pawn.royalty == null)
			{
				return "";
			}
			RoyalTitle royalTitle = null;
			foreach (RoyalTitle royalTitle2 in from x in pawn.royalty.AllTitlesForReading
			orderby x.def.index
			select x)
			{
				if (royalTitle == null || royalTitle2.def.favorCost > royalTitle.def.favorCost)
				{
					royalTitle = royalTitle2;
				}
			}
			if (royalTitle == null)
			{
				return "";
			}
			return royalTitle.def.GetLabelFor(pawn.gender);
		}

		
		public static string PawnResolveRoyalTitleInCurrentFaction(Pawn pawn)
		{
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in from x in pawn.royalty.AllTitlesForReading
				orderby x.def.index
				select x)
				{
					if (royalTitle.faction == pawn.Faction)
					{
						return royalTitle.def.GetLabelFor(pawn.gender);
					}
				}
			}
			return "";
		}



	}
}
