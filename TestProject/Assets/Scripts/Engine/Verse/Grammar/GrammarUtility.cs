using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse.Grammar
{
	
	public static class GrammarUtility
	{
		
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Pawn pawn, Dictionary<string, string> constants = null, bool addRelationInfoSymbol = true, bool addTags = true)
		{
			if (pawn == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null pawn", pawnSymbol), 16015097, false);
				return Enumerable.Empty<Rule>();
			}
			TaggedString taggedString = "";
			if (addRelationInfoSymbol)
			{
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			}
			return GrammarUtility.RulesForPawn(pawnSymbol, pawn.Name, (pawn.story != null) ? pawn.story.Title : null, pawn.kindDef, pawn.gender, pawn.Faction, pawn.ageTracker.AgeBiologicalYears, pawn.ageTracker.AgeChronologicalYears, taggedString, PawnUtility.EverBeenColonistOrTameAnimal(pawn), PawnUtility.EverBeenQuestLodger(pawn), pawn.Faction != null && pawn.Faction.leader == pawn, (pawn.royalty != null) ? pawn.royalty.AllTitlesForReading : null, constants, addTags);
		}

		
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Name name, string title, PawnKindDef kind, Gender gender, Faction faction, int age, int chronologicalAge, string relationInfo, bool everBeenColonistOrTameAnimal, bool everBeenQuestLodger, bool isFactionLeader, List<RoyalTitle> royalTitles, Dictionary<string, string> constants = null, bool addTags = true)
		{
			string prefix = "";
			if (!pawnSymbol.NullOrEmpty())
			{
				prefix = prefix + pawnSymbol + "_";
			}
			string kindLabel;
			if (gender == Gender.Female)
			{
				kindLabel = (kind.labelFemale.NullOrEmpty() ? kind.label : kind.labelFemale);
			}
			else if (gender == Gender.Male)
			{
				kindLabel = (kind.labelMale.NullOrEmpty() ? kind.label : kind.labelMale);
			}
			else
			{
				kindLabel = kind.label;
			}
			string s;
			if (name != null)
			{
				s = Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringFull, gender, false, true);
			}
			else
			{
				s = Find.ActiveLanguageWorker.WithIndefiniteArticle(kindLabel, gender, false, false);
			}
			yield return new Rule_String(prefix + "nameFull", s.ApplyTag(TagType.Name, null).Resolve());
			string nameShort;
			if (name != null)
			{
				nameShort = name.ToStringShort;
			}
			else
			{
				nameShort = kindLabel;
			}
			yield return new Rule_String(prefix + "label", addTags ? nameShort.ApplyTag(TagType.Name, null).Resolve() : nameShort);
			string nameShortDef;
			if (name != null)
			{
				nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(name.ToStringShort, gender, false, true);
			}
			else
			{
				nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(kindLabel, gender, false, false);
			}
			yield return new Rule_String(prefix + "definite", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			yield return new Rule_String(prefix + "nameDef", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			nameShortDef = null;
			if (name != null)
			{
				nameShortDef = Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringShort, gender, false, true);
			}
			else
			{
				nameShortDef = Find.ActiveLanguageWorker.WithIndefiniteArticle(kindLabel, gender, false, false);
			}
			yield return new Rule_String(prefix + "indefinite", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			yield return new Rule_String(prefix + "nameIndef", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			nameShortDef = null;
			yield return new Rule_String(prefix + "pronoun", gender.GetPronoun());
			yield return new Rule_String(prefix + "possessive", gender.GetPossessive());
			yield return new Rule_String(prefix + "objective", gender.GetObjective());
			if (faction != null)
			{
				yield return new Rule_String(prefix + "factionName", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
				if (constants != null)
				{
					constants[prefix + "faction"] = faction.def.defName;
				}
			}
			if (constants != null && isFactionLeader)
			{
				constants[prefix + "factionLeader"] = "True";
			}
			if (kind != null)
			{
				yield return new Rule_String(prefix + "kind", GenLabel.BestKindLabel(kind, gender, false, -1));
			}
			if (title != null)
			{
				yield return new Rule_String(prefix + "title", title);
				yield return new Rule_String(prefix + "titleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(title, gender, false, false));
				yield return new Rule_String(prefix + "titleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(title, gender, false, false));
			}
			if (royalTitles != null)
			{
				int royalTitleIndex = 0;
				RoyalTitle bestTitle = null;
				foreach (RoyalTitle royalTitle in from x in royalTitles
				orderby x.def.index
				select x)
				{
					yield return new Rule_String(prefix + "royalTitle" + royalTitleIndex, royalTitle.def.GetLabelFor(gender));
					yield return new Rule_String(string.Concat(new object[]
					{
						prefix,
						"royalTitle",
						royalTitleIndex,
						"Indef"
					}), Find.ActiveLanguageWorker.WithIndefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(string.Concat(new object[]
					{
						prefix,
						"royalTitle",
						royalTitleIndex,
						"Def"
					}), Find.ActiveLanguageWorker.WithDefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "royalTitleFaction" + royalTitleIndex, royalTitle.faction.Name.ApplyTag(royalTitle.faction).Resolve());
					if (royalTitle.faction == faction)
					{
						yield return new Rule_String(prefix + "royalTitleInCurrentFaction", royalTitle.def.GetLabelFor(gender));
						yield return new Rule_String(prefix + "royalTitleInCurrentFactionIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
						yield return new Rule_String(prefix + "royalTitleInCurrentFactionDef", Find.ActiveLanguageWorker.WithDefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
						if (constants != null)
						{
							constants[prefix + "royalInCurrentFaction"] = "True";
						}
					}
					if (bestTitle == null || royalTitle.def.favorCost > bestTitle.def.favorCost)
					{
						bestTitle = royalTitle;
					}
					int num = royalTitleIndex;
					royalTitleIndex = num + 1;

				}
				IEnumerator<RoyalTitle> enumerator = null;
				if (bestTitle != null)
				{
					yield return new Rule_String(prefix + "bestRoyalTitle", bestTitle.def.GetLabelFor(gender));
					yield return new Rule_String(prefix + "bestRoyalTitleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(bestTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "bestRoyalTitleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(bestTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "bestRoyalTitleFaction", bestTitle.faction.Name);
				}
				bestTitle = null;
			}
			yield return new Rule_String(prefix + "age", age.ToString());
			yield return new Rule_String(prefix + "chronologicalAge", chronologicalAge.ToString());
			if (everBeenColonistOrTameAnimal)
			{
				yield return new Rule_String("formerlyColonistInfo", "PawnWasFormerlyColonist".Translate(nameShort));
				if (constants != null)
				{
					constants[prefix + "formerlyColonist"] = "True";
				}
			}
			else if (everBeenQuestLodger)
			{
				yield return new Rule_String("formerlyColonistInfo", "PawnWasFormerlyLodger".Translate(nameShort));
				if (constants != null)
				{
					constants[prefix + "formerlyColonist"] = "True";
				}
			}
			yield return new Rule_String(prefix + "relationInfo", relationInfo);
			if (constants != null && kind != null)
			{
				constants[prefix + "flesh"] = kind.race.race.FleshType.defName;
			}
			if (constants != null)
			{
				constants[prefix + "gender"] = gender.ToString();
			}
			yield break;
			yield break;
		}

		
		public static IEnumerable<Rule> RulesForDef(string prefix, Def def)
		{
			if (def == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null def", prefix), 79641686, false);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", def.label);
			if (def is PawnKindDef)
			{
				yield return new Rule_String(prefix + "labelPlural", ((PawnKindDef)def).GetLabelPlural(-1));
			}
			else
			{
				yield return new Rule_String(prefix + "labelPlural", Find.ActiveLanguageWorker.Pluralize(def.label, -1));
			}
			yield return new Rule_String(prefix + "description", def.description);
			yield return new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(def.label, false, false));
			yield return new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label, false, false));
			yield return new Rule_String(prefix + "possessive", "Proits".Translate());
			yield break;
		}

		
		public static IEnumerable<Rule> RulesForBodyPartRecord(string prefix, BodyPartRecord part)
		{
			if (part == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null body part", prefix), 394876778, false);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", part.Label);
			yield return new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(part.Label, false, false));
			yield return new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(part.Label, false, false));
			yield return new Rule_String(prefix + "possessive", "Proits".Translate());
			yield break;
		}

		
		public static IEnumerable<Rule> RulesForHediffDef(string prefix, HediffDef def, BodyPartRecord part)
		{
			foreach (Rule rule in GrammarUtility.RulesForDef(prefix, def))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", def.label);
			string output = (!def.labelNoun.NullOrEmpty()) ? def.labelNoun : def.label;
			yield return new Rule_String(prefix + "labelNoun", output);
			string text = def.PrettyTextForPart(part);
			if (!text.NullOrEmpty())
			{
				yield return new Rule_String(prefix + "labelNounPretty", text);
			}
			yield break;
			yield break;
		}

		
		public static IEnumerable<Rule> RulesForFaction(string prefix, Faction faction, bool addTags = true)
		{
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			if (faction == null)
			{
				yield return new Rule_String(prefix + "name", "FactionUnaffiliated".Translate());
				yield break;
			}
			yield return new Rule_String(prefix + "name", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
			yield return new Rule_String(prefix + "pawnsPlural", faction.def.pawnsPlural);
			yield return new Rule_String(prefix + "pawnsPluralDef", Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), true, false));
			yield return new Rule_String(prefix + "pawnsPluralIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), true, false));
			yield return new Rule_String(prefix + "leaderTitle", faction.LeaderTitle);
			yield return new Rule_String(prefix + "royalFavorLabel", faction.def.royalFavorLabel);
			yield break;
		}

		
		public static IEnumerable<Rule> RulesForWorldObject(string prefix, WorldObject worldObject, bool addTags = true)
		{
			//GrammarUtility.c__DisplayClass6_0 c__DisplayClass6_;
			//c__DisplayClass6_.worldObject = worldObject;
			//c__DisplayClass6_.addTags = addTags;
			//if (!prefix.NullOrEmpty())
			//{
			//	prefix += "_";
			//}
			//yield return new Rule_String(prefix + "label", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|6_0(c__DisplayClass6_.worldObject.Label, ref c__DisplayClass6_));
			//yield return new Rule_String(prefix + "definite", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|6_0(Find.ActiveLanguageWorker.WithDefiniteArticle(c__DisplayClass6_.worldObject.Label, false, c__DisplayClass6_.worldObject.HasName), ref c__DisplayClass6_));
			//yield return new Rule_String(prefix + "indefinite", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|6_0(Find.ActiveLanguageWorker.WithIndefiniteArticle(c__DisplayClass6_.worldObject.Label, false, c__DisplayClass6_.worldObject.HasName), ref c__DisplayClass6_));
			yield break;
		}
	}
}
