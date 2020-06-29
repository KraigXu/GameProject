using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public static class NameGenerator
	{
		
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			return NameGenerator.GenerateName(rootPack, (string x) => !extantNames.Contains(x), appendNumberIfNameUsed, rootKeyword, null);
		}

		
		public static string GenerateName(RulePackDef rootPack, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null, string testPawnNameSymbol = null)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(rootPack);
			if (testPawnNameSymbol != null)
			{
				request.Rules.Add(new Rule_String("ANYPAWN_nameDef", testPawnNameSymbol));
				request.Rules.Add(new Rule_String("ANYPAWN_nameIndef", testPawnNameSymbol));
			}
			string rootKeyword2 = (rootKeyword != null) ? rootKeyword : rootPack.FirstRuleKeyword;
			string untranslatedRootKeyword = (rootKeyword != null) ? rootKeyword : rootPack.FirstUntranslatedRuleKeyword;
			return NameGenerator.GenerateName(request, validator, appendNumberIfNameUsed, rootKeyword2, untranslatedRootKeyword);
		}

		
		public static string GenerateName(GrammarRequest request, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null, string untranslatedRootKeyword = null)
		{
			if (untranslatedRootKeyword == null)
			{
				untranslatedRootKeyword = rootKeyword;
			}
			string text = "ErrorName";
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						text = GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
						if (i != 0)
						{
							text = text + " " + (i + 1);
						}
						if (validator == null || validator(text))
						{
							return text;
						}
					}
				}
				return GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
			}
			for (int k = 0; k < 150; k++)
			{
				text = GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
				if (validator == null || validator(text))
				{
					return text;
				}
			}
			Log.Error("Could not get new name (first rule pack: " + request.Includes.FirstOrDefault<RulePackDef>().ToStringSafe<RulePackDef>() + ")", false);
			return text;
		}
	}
}
