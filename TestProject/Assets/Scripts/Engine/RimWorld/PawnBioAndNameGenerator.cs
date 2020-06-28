using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B18 RID: 2840
	public static class PawnBioAndNameGenerator
	{
		// Token: 0x060042CE RID: 17102 RVA: 0x00166AA0 File Offset: 0x00164CA0
		public static void GiveAppropriateBioAndNameTo(Pawn pawn, string requiredLastName, FactionDef factionType)
		{
			List<BackstoryCategoryFilter> backstoryCategoryFiltersFor = PawnBioAndNameGenerator.GetBackstoryCategoryFiltersFor(pawn, factionType);
			if ((Rand.Value < 0.25f || pawn.kindDef.factionLeader) && PawnBioAndNameGenerator.TryGiveSolidBioTo(pawn, requiredLastName, backstoryCategoryFiltersFor))
			{
				return;
			}
			PawnBioAndNameGenerator.GiveShuffledBioTo(pawn, factionType, requiredLastName, backstoryCategoryFiltersFor);
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x00166AE4 File Offset: 0x00164CE4
		private static void GiveShuffledBioTo(Pawn pawn, FactionDef factionType, string requiredLastName, List<BackstoryCategoryFilter> backstoryCategories)
		{
			PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Childhood, ref pawn.story.childhood, pawn.story.adulthood, backstoryCategories, factionType);
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20f)
			{
				PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Adulthood, ref pawn.story.adulthood, pawn.story.childhood, backstoryCategories, factionType);
			}
			pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, requiredLastName);
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x00166B50 File Offset: 0x00164D50
		private static void FillBackstorySlotShuffled(Pawn pawn, BackstorySlot slot, ref Backstory backstory, Backstory backstoryOtherSlot, List<BackstoryCategoryFilter> backstoryCategories, FactionDef factionType)
		{
			BackstoryCategoryFilter backstoryCategoryFilter = backstoryCategories.RandomElementByWeight((BackstoryCategoryFilter c) => c.commonality);
			if (backstoryCategoryFilter == null)
			{
				backstoryCategoryFilter = PawnBioAndNameGenerator.FallbackCategoryGroup;
			}
			if (!(from bs in BackstoryDatabase.ShuffleableBackstoryList(slot, backstoryCategoryFilter).TakeRandom(20)
			where slot != BackstorySlot.Adulthood || !bs.requiredWorkTags.OverlapsWithOnAnyWorkType(pawn.story.childhood.workDisables)
			select bs).TryRandomElementByWeight(new Func<Backstory, float>(PawnBioAndNameGenerator.BackstorySelectionWeight), out backstory))
			{
				Log.Error(string.Concat(new object[]
				{
					"No shuffled ",
					slot,
					" found for ",
					pawn.ToStringSafe<Pawn>(),
					" of ",
					factionType.ToStringSafe<FactionDef>(),
					". Choosing random."
				}), false);
				backstory = (from kvp in BackstoryDatabase.allBackstories
				where kvp.Value.slot == slot
				select kvp).RandomElement<KeyValuePair<string, Backstory>>().Value;
			}
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x00166C58 File Offset: 0x00164E58
		private static bool TryGiveSolidBioTo(Pawn pawn, string requiredLastName, List<BackstoryCategoryFilter> backstoryCategories)
		{
			PawnBio pawnBio;
			if (!PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(backstoryCategories, pawn.kindDef, pawn.gender, requiredLastName, out pawnBio))
			{
				return false;
			}
			if (pawnBio.name.First == "Tynan" && pawnBio.name.Last == "Sylvester" && Rand.Value < 0.5f && !PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(backstoryCategories, pawn.kindDef, pawn.gender, requiredLastName, out pawnBio))
			{
				return false;
			}
			pawn.Name = pawnBio.name;
			pawn.story.childhood = pawnBio.childhood;
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20f)
			{
				pawn.story.adulthood = pawnBio.adulthood;
			}
			return true;
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x00166D14 File Offset: 0x00164F14
		private static bool IsBioUseable(PawnBio bio, BackstoryCategoryFilter categoryFilter, PawnKindDef kind, Gender gender, string requiredLastName)
		{
			if (bio.gender != GenderPossibility.Either)
			{
				if (gender == Gender.Male && bio.gender != GenderPossibility.Male)
				{
					return false;
				}
				if (gender == Gender.Female && bio.gender != GenderPossibility.Female)
				{
					return false;
				}
			}
			if (!requiredLastName.NullOrEmpty() && bio.name.Last != requiredLastName)
			{
				return false;
			}
			if (kind.factionLeader && !bio.pirateKing)
			{
				return false;
			}
			if (!categoryFilter.Matches(bio))
			{
				return false;
			}
			if (bio.name.UsedThisGame)
			{
				return false;
			}
			if (kind.requiredWorkTags != WorkTags.None)
			{
				if (bio.childhood != null && (bio.childhood.workDisables & kind.requiredWorkTags) != WorkTags.None)
				{
					return false;
				}
				if (bio.adulthood != null && (bio.adulthood.workDisables & kind.requiredWorkTags) != WorkTags.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00166DD8 File Offset: 0x00164FD8
		private static bool TryGetRandomUnusedSolidBioFor(List<BackstoryCategoryFilter> backstoryCategories, PawnKindDef kind, Gender gender, string requiredLastName, out PawnBio result)
		{
			BackstoryCategoryFilter categoryFilter = backstoryCategories.RandomElementByWeight((BackstoryCategoryFilter c) => c.commonality);
			if (categoryFilter == null)
			{
				categoryFilter = PawnBioAndNameGenerator.FallbackCategoryGroup;
			}
			if (Rand.Value < 0.5f)
			{
				PawnBioAndNameGenerator.tmpNames.Clear();
				PawnBioAndNameGenerator.tmpNames.AddRange(Prefs.PreferredNames);
				PawnBioAndNameGenerator.tmpNames.Shuffle<string>();
				foreach (string a in PawnBioAndNameGenerator.tmpNames)
				{
					foreach (PawnBio pawnBio in SolidBioDatabase.allBios)
					{
						if (a == pawnBio.name.ToString() && PawnBioAndNameGenerator.IsBioUseable(pawnBio, categoryFilter, kind, gender, requiredLastName))
						{
							result = pawnBio;
							return true;
						}
					}
				}
			}
			return (from bio in SolidBioDatabase.allBios.TakeRandom(20)
			where PawnBioAndNameGenerator.IsBioUseable(bio, categoryFilter, kind, gender, requiredLastName)
			select bio).TryRandomElementByWeight(new Func<PawnBio, float>(PawnBioAndNameGenerator.BioSelectionWeight), out result);
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x00166F5C File Offset: 0x0016515C
		public static NameTriple TryGetRandomUnusedSolidName(Gender gender, string requiredLastName = null)
		{
			List<NameTriple> listForGender = PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Either);
			List<NameTriple> list = (gender == Gender.Male) ? PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Male) : PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Female);
			float num = ((float)listForGender.Count + 0.1f) / ((float)(listForGender.Count + list.Count) + 0.1f);
			List<NameTriple> list2;
			if (Rand.Value < num)
			{
				list2 = listForGender;
			}
			else
			{
				list2 = list;
			}
			if (list2.Count == 0)
			{
				Log.Error("Empty solid pawn name list for gender: " + gender + ".", false);
				return null;
			}
			if (Rand.Value < 0.5f)
			{
				PawnBioAndNameGenerator.tmpNames.Clear();
				PawnBioAndNameGenerator.tmpNames.AddRange(Prefs.PreferredNames);
				PawnBioAndNameGenerator.tmpNames.Shuffle<string>();
				foreach (string rawName in PawnBioAndNameGenerator.tmpNames)
				{
					NameTriple nameTriple = NameTriple.FromString(rawName);
					if (list2.Contains(nameTriple) && !nameTriple.UsedThisGame && (requiredLastName == null || !(nameTriple.Last != requiredLastName)))
					{
						return nameTriple;
					}
				}
			}
			list2.Shuffle<NameTriple>();
			return (from name in list2
			where (requiredLastName == null || !(name.Last != requiredLastName)) && !name.UsedThisGame
			select name).FirstOrDefault<NameTriple>();
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x001670B4 File Offset: 0x001652B4
		private static List<BackstoryCategoryFilter> GetBackstoryCategoryFiltersFor(Pawn pawn, FactionDef faction)
		{
			if (!pawn.kindDef.backstoryFiltersOverride.NullOrEmpty<BackstoryCategoryFilter>())
			{
				return pawn.kindDef.backstoryFiltersOverride;
			}
			List<BackstoryCategoryFilter> list = new List<BackstoryCategoryFilter>();
			if (pawn.kindDef.backstoryFilters != null)
			{
				list.AddRange(pawn.kindDef.backstoryFilters);
			}
			if (faction != null && !faction.backstoryFilters.NullOrEmpty<BackstoryCategoryFilter>())
			{
				for (int i = 0; i < faction.backstoryFilters.Count; i++)
				{
					BackstoryCategoryFilter item = faction.backstoryFilters[i];
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			if (!list.NullOrEmpty<BackstoryCategoryFilter>())
			{
				return list;
			}
			Log.ErrorOnce(string.Concat(new object[]
			{
				"PawnKind ",
				pawn.kindDef,
				" generating with factionDef ",
				faction,
				": no backstoryCategories in either."
			}), 1871521, false);
			return new List<BackstoryCategoryFilter>
			{
				PawnBioAndNameGenerator.FallbackCategoryGroup
			};
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x0016719C File Offset: 0x0016539C
		public static Name GeneratePawnName(Pawn pawn, NameStyle style = NameStyle.Full, string forcedLastName = null)
		{
			if (style != NameStyle.Full)
			{
				if (style == NameStyle.Numeric)
				{
					try
					{
						foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
						{
							NameSingle nameSingle = pawn2.Name as NameSingle;
							if (nameSingle != null)
							{
								PawnBioAndNameGenerator.usedNamesTmp.Add(nameSingle.Name);
							}
						}
						int num = 1;
						string text;
						for (;;)
						{
							text = string.Format("{0} {1}", pawn.KindLabel, num.ToString());
							if (!PawnBioAndNameGenerator.usedNamesTmp.Contains(text))
							{
								break;
							}
							num++;
						}
						return new NameSingle(text, true);
					}
					finally
					{
						PawnBioAndNameGenerator.usedNamesTmp.Clear();
					}
				}
				throw new InvalidOperationException();
			}
			if (pawn.story != null)
			{
				if (pawn.story.childhood != null && pawn.story.childhood.NameMaker != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(pawn.story.childhood.NameMaker, forcedLastName);
				}
				if (pawn.story.adulthood != null && pawn.story.adulthood.NameMaker != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(pawn.story.adulthood.NameMaker, forcedLastName);
				}
			}
			RulePackDef nameGenerator = pawn.RaceProps.GetNameGenerator(pawn.gender);
			if (nameGenerator != null)
			{
				return new NameSingle(NameGenerator.GenerateName(nameGenerator, (string x) => !new NameSingle(x, false).UsedThisGame, false, null, null), false);
			}
			if (pawn.Faction != null)
			{
				RulePackDef nameMaker = pawn.Faction.def.GetNameMaker(pawn.gender);
				if (nameMaker != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(nameMaker, forcedLastName);
				}
			}
			if (pawn.RaceProps.nameCategory != PawnNameCategory.NoName)
			{
				if (Rand.Value < 0.5f)
				{
					NameTriple nameTriple = PawnBioAndNameGenerator.TryGetRandomUnusedSolidName(pawn.gender, forcedLastName);
					if (nameTriple != null)
					{
						return nameTriple;
					}
				}
				return PawnBioAndNameGenerator.GeneratePawnName_Shuffled(pawn, forcedLastName);
			}
			Log.Error("No name making method for " + pawn, false);
			NameTriple nameTriple2 = NameTriple.FromString(pawn.def.label);
			nameTriple2.ResolveMissingPieces(null);
			return nameTriple2;
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x001673B0 File Offset: 0x001655B0
		private static Name NameResolvedFrom(RulePackDef nameMaker, string forcedLastName)
		{
			NameTriple nameTriple = NameTriple.FromString(NameGenerator.GenerateName(nameMaker, delegate(string x)
			{
				NameTriple nameTriple2 = NameTriple.FromString(x);
				nameTriple2.ResolveMissingPieces(forcedLastName);
				return !nameTriple2.UsedThisGame;
			}, false, null, null));
			nameTriple.CapitalizeNick();
			nameTriple.ResolveMissingPieces(forcedLastName);
			return nameTriple;
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x001673F8 File Offset: 0x001655F8
		private static NameTriple GeneratePawnName_Shuffled(Pawn pawn, string forcedLastName = null)
		{
			PawnNameCategory pawnNameCategory = pawn.RaceProps.nameCategory;
			if (pawnNameCategory == PawnNameCategory.NoName)
			{
				Log.Message("Can't create a name of type NoName. Defaulting to HumanStandard.", false);
				pawnNameCategory = PawnNameCategory.HumanStandard;
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(pawnNameCategory);
			string name = nameBank.GetName(PawnNameSlot.First, pawn.gender, true);
			string text;
			if (forcedLastName != null)
			{
				text = forcedLastName;
			}
			else
			{
				text = nameBank.GetName(PawnNameSlot.Last, Gender.None, true);
			}
			int num = 0;
			string nick;
			do
			{
				num++;
				if (Rand.Value < 0.15f)
				{
					Gender gender = pawn.gender;
					if (Rand.Value < 0.8f)
					{
						gender = Gender.None;
					}
					nick = nameBank.GetName(PawnNameSlot.Nick, gender, true);
				}
				else if (Rand.Value < 0.5f)
				{
					nick = name;
				}
				else
				{
					nick = text;
				}
			}
			while (num < 50 && NameUseChecker.AllPawnsNamesEverUsed.Any(delegate(Name x)
			{
				NameTriple nameTriple = x as NameTriple;
				return nameTriple != null && nameTriple.Nick == nick;
			}));
			return new NameTriple(name, nick, text);
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x001674D7 File Offset: 0x001656D7
		private static float BackstorySelectionWeight(Backstory bs)
		{
			return PawnBioAndNameGenerator.SelectionWeightFactorFromWorkTagsDisabled(bs.workDisables);
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x001674E4 File Offset: 0x001656E4
		private static float BioSelectionWeight(PawnBio bio)
		{
			return PawnBioAndNameGenerator.SelectionWeightFactorFromWorkTagsDisabled(bio.adulthood.workDisables | bio.childhood.workDisables);
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x00167504 File Offset: 0x00165704
		private static float SelectionWeightFactorFromWorkTagsDisabled(WorkTags wt)
		{
			float num = 1f;
			if ((wt & WorkTags.ManualDumb) != WorkTags.None)
			{
				num *= 0.5f;
			}
			if ((wt & WorkTags.ManualSkilled) != WorkTags.None)
			{
				num *= 1f;
			}
			if ((wt & WorkTags.Violent) != WorkTags.None)
			{
				num *= 0.6f;
			}
			if ((wt & WorkTags.Social) != WorkTags.None)
			{
				num *= 0.7f;
			}
			if ((wt & WorkTags.Intellectual) != WorkTags.None)
			{
				num *= 0.4f;
			}
			if ((wt & WorkTags.Firefighting) != WorkTags.None)
			{
				num *= 0.8f;
			}
			return num;
		}

		// Token: 0x0400266D RID: 9837
		private const float MinAgeForAdulthood = 20f;

		// Token: 0x0400266E RID: 9838
		private const float SolidBioChance = 0.25f;

		// Token: 0x0400266F RID: 9839
		private const float SolidNameChance = 0.5f;

		// Token: 0x04002670 RID: 9840
		private const float TryPreferredNameChance_Bio = 0.5f;

		// Token: 0x04002671 RID: 9841
		private const float TryPreferredNameChance_Name = 0.5f;

		// Token: 0x04002672 RID: 9842
		private const float ShuffledNicknameChance = 0.15f;

		// Token: 0x04002673 RID: 9843
		private const float ShuffledNicknameChanceImperial = 0.05f;

		// Token: 0x04002674 RID: 9844
		private const float ShuffledNicknameChanceUnisex = 0.8f;

		// Token: 0x04002675 RID: 9845
		private static readonly BackstoryCategoryFilter FallbackCategoryGroup = new BackstoryCategoryFilter
		{
			categories = new List<string>
			{
				"Civil"
			},
			commonality = 1f
		};

		// Token: 0x04002676 RID: 9846
		private static List<string> tmpNames = new List<string>();

		// Token: 0x04002677 RID: 9847
		private static HashSet<string> usedNamesTmp = new HashSet<string>();
	}
}
