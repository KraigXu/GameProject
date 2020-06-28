using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6E RID: 2926
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004470 RID: 17520 RVA: 0x0017205C File Offset: 0x0017025C
		public RulePackDef NameMaker
		{
			get
			{
				return this.nameMakerResolved;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06004471 RID: 17521 RVA: 0x00172064 File Offset: 0x00170264
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if (!this.AllowsWorkType(list[i]))
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06004472 RID: 17522 RVA: 0x00172074 File Offset: 0x00170274
		public IEnumerable<WorkGiverDef> DisabledWorkGivers
		{
			get
			{
				List<WorkGiverDef> list = DefDatabase<WorkGiverDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if (!this.AllowsWorkGiver(list[i]))
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x00172084 File Offset: 0x00170284
		public bool DisallowsTrait(TraitDef def, int degree)
		{
			if (this.disallowedTraits == null)
			{
				return false;
			}
			for (int i = 0; i < this.disallowedTraits.Count; i++)
			{
				if (this.disallowedTraits[i].def == def && this.disallowedTraits[i].degree == degree)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x001720DC File Offset: 0x001702DC
		public string TitleFor(Gender g)
		{
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				return this.title;
			}
			return this.titleFemale;
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x001720FC File Offset: 0x001702FC
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x0017210A File Offset: 0x0017030A
		public string TitleShortFor(Gender g)
		{
			if (g == Gender.Female && !this.titleShortFemale.NullOrEmpty())
			{
				return this.titleShortFemale;
			}
			if (!this.titleShort.NullOrEmpty())
			{
				return this.titleShort;
			}
			return this.TitleFor(g);
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0017213F File Offset: 0x0017033F
		public string TitleShortCapFor(Gender g)
		{
			return this.TitleShortFor(g).CapitalizeFirst();
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0017214D File Offset: 0x0017034D
		public BodyTypeDef BodyTypeFor(Gender g)
		{
			if (this.bodyTypeGlobalResolved != null || g == Gender.None)
			{
				return this.bodyTypeGlobalResolved;
			}
			if (g == Gender.Female)
			{
				return this.bodyTypeFemaleResolved;
			}
			return this.bodyTypeMaleResolved;
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x00172174 File Offset: 0x00170374
		public TaggedString FullDescriptionFor(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.baseDesc.Formatted(p.Named("PAWN")).AdjustedFor(p, "PAWN", true).Resolve());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				if (this.skillGainsResolved.ContainsKey(skillDef))
				{
					stringBuilder.AppendLine(skillDef.skillLabel.CapitalizeFirst() + ":   " + this.skillGainsResolved[skillDef].ToString("+##;-##"));
				}
			}
			stringBuilder.AppendLine();
			foreach (WorkTypeDef workTypeDef in this.DisabledWorkTypes)
			{
				stringBuilder.AppendLine(workTypeDef.gerundLabel.CapitalizeFirst() + " " + "DisabledLower".Translate());
			}
			foreach (WorkGiverDef workGiverDef in this.DisabledWorkGivers)
			{
				stringBuilder.AppendLine(workGiverDef.workType.gerundLabel.CapitalizeFirst() + ": " + workGiverDef.LabelCap + " " + "DisabledLower".Translate());
			}
			if (ModsConfig.RoyaltyActive)
			{
				this.unlockedMeditationTypesTemp.Clear();
				foreach (MeditationFocusDef meditationFocusDef in DefDatabase<MeditationFocusDef>.AllDefs)
				{
					for (int j = 0; j < meditationFocusDef.requiredBackstoriesAny.Count; j++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot = meditationFocusDef.requiredBackstoriesAny[j];
						if (this.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName) && backstoryCategoryAndSlot.slot == this.slot)
						{
							this.unlockedMeditationTypesTemp.Add(meditationFocusDef.LabelCap);
							break;
						}
					}
				}
				if (this.unlockedMeditationTypesTemp.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("MeditationFocusesUnlocked".Translate() + ": ");
					stringBuilder.AppendLine(this.unlockedMeditationTypesTemp.ToLineList("  - "));
				}
			}
			string str = stringBuilder.ToString().TrimEndNewlines();
			return Find.ActiveLanguageWorker.PostProcessed(str);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x00172454 File Offset: 0x00170654
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x00172466 File Offset: 0x00170666
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x00172478 File Offset: 0x00170678
		internal void AddForcedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.forcedTraits == null)
			{
				this.forcedTraits = new List<TraitEntry>();
			}
			this.forcedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0017249F File Offset: 0x0017069F
		internal void AddDisallowedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.disallowedTraits == null)
			{
				this.disallowedTraits = new List<TraitEntry>();
			}
			this.disallowedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x001724C8 File Offset: 0x001706C8
		public void PostLoad()
		{
			this.untranslatedTitle = this.title;
			this.untranslatedTitleFemale = this.titleFemale;
			this.untranslatedTitleShort = this.titleShort;
			this.untranslatedTitleShortFemale = this.titleShortFemale;
			this.untranslatedDesc = this.baseDesc;
			this.baseDesc = this.baseDesc.TrimEnd(Array.Empty<char>());
			this.baseDesc = this.baseDesc.Replace("\r", "");
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x00172544 File Offset: 0x00170744
		public void ResolveReferences()
		{
			int num = Mathf.Abs(GenText.StableStringHash(this.baseDesc) % 100);
			string s = this.title.Replace('-', ' ');
			s = GenText.CapitalizedNoSpaces(s);
			this.identifier = GenText.RemoveNonAlphanumeric(s) + num.ToString();
			foreach (KeyValuePair<string, int> keyValuePair in this.skillGains)
			{
				this.skillGainsResolved.Add(DefDatabase<SkillDef>.GetNamed(keyValuePair.Key, true), keyValuePair.Value);
			}
			this.skillGains = null;
			if (!this.bodyTypeGlobal.NullOrEmpty())
			{
				this.bodyTypeGlobalResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeGlobal, true);
			}
			if (!this.bodyTypeFemale.NullOrEmpty())
			{
				this.bodyTypeFemaleResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeFemale, true);
			}
			if (!this.bodyTypeMale.NullOrEmpty())
			{
				this.bodyTypeMaleResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeMale, true);
			}
			if (!this.nameMaker.NullOrEmpty())
			{
				this.nameMakerResolved = DefDatabase<RulePackDef>.GetNamed(this.nameMaker, true);
			}
			if (this.slot == BackstorySlot.Adulthood && this.bodyTypeGlobalResolved == null)
			{
				if (this.bodyTypeMaleResolved == null)
				{
					Log.Error("Adulthood backstory " + this.title + " is missing male body type. Defaulting...", false);
					this.bodyTypeMaleResolved = BodyTypeDefOf.Male;
				}
				if (this.bodyTypeFemaleResolved == null)
				{
					Log.Error("Adulthood backstory " + this.title + " is missing female body type. Defaulting...", false);
					this.bodyTypeFemaleResolved = BodyTypeDefOf.Female;
				}
			}
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x001726E4 File Offset: 0x001708E4
		public IEnumerable<string> ConfigErrors(bool ignoreNoSpawnCategories)
		{
			if (this.title.NullOrEmpty())
			{
				yield return "null title, baseDesc is " + this.baseDesc;
			}
			if (this.titleShort.NullOrEmpty())
			{
				yield return "null titleShort, baseDesc is " + this.baseDesc;
			}
			if ((this.workDisables & WorkTags.Violent) != WorkTags.None && this.spawnCategories.Contains("Pirate"))
			{
				yield return "cannot do Violent work but can spawn as a pirate";
			}
			if (this.spawnCategories.Count == 0 && !ignoreNoSpawnCategories)
			{
				yield return "no spawn categories";
			}
			if (!this.baseDesc.NullOrEmpty())
			{
				if (char.IsWhiteSpace(this.baseDesc[0]))
				{
					yield return "baseDesc starts with whitepspace";
				}
				if (char.IsWhiteSpace(this.baseDesc[this.baseDesc.Length - 1]))
				{
					yield return "baseDesc ends with whitespace";
				}
			}
			if (this.forcedTraits != null)
			{
				using (List<TraitEntry>.Enumerator enumerator = this.forcedTraits.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TraitEntry forcedTrait = enumerator.Current;
						if (!forcedTrait.def.degreeDatas.Any((TraitDegreeData d) => d.degree == forcedTrait.degree))
						{
							yield return string.Concat(new object[]
							{
								"Backstory ",
								this.title,
								" has invalid trait ",
								forcedTrait.def.defName,
								" degree=",
								forcedTrait.degree
							});
						}
					}
				}
				List<TraitEntry>.Enumerator enumerator = default(List<TraitEntry>.Enumerator);
			}
			if (Prefs.DevMode)
			{
				foreach (KeyValuePair<SkillDef, int> keyValuePair in this.skillGainsResolved)
				{
					if (keyValuePair.Key.IsDisabled(this.workDisables, this.DisabledWorkTypes))
					{
						yield return "modifies skill " + keyValuePair.Key + " but also disables this skill";
					}
				}
				Dictionary<SkillDef, int>.Enumerator enumerator2 = default(Dictionary<SkillDef, int>.Enumerator);
				foreach (KeyValuePair<string, Backstory> keyValuePair2 in BackstoryDatabase.allBackstories)
				{
					if (keyValuePair2.Value != this && keyValuePair2.Value.identifier == this.identifier)
					{
						yield return "backstory identifier used more than once: " + this.identifier;
					}
				}
				Dictionary<string, Backstory>.Enumerator enumerator3 = default(Dictionary<string, Backstory>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x001726FB File Offset: 0x001708FB
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0017270B File Offset: 0x0017090B
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0017271B File Offset: 0x0017091B
		public override string ToString()
		{
			if (this.title.NullOrEmpty())
			{
				return "(NullTitleBackstory)";
			}
			return "(" + this.title + ")";
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x00172745 File Offset: 0x00170945
		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		// Token: 0x040026F7 RID: 9975
		public string identifier;

		// Token: 0x040026F8 RID: 9976
		public BackstorySlot slot;

		// Token: 0x040026F9 RID: 9977
		public string title;

		// Token: 0x040026FA RID: 9978
		public string titleFemale;

		// Token: 0x040026FB RID: 9979
		public string titleShort;

		// Token: 0x040026FC RID: 9980
		public string titleShortFemale;

		// Token: 0x040026FD RID: 9981
		public string baseDesc;

		// Token: 0x040026FE RID: 9982
		private Dictionary<string, int> skillGains = new Dictionary<string, int>();

		// Token: 0x040026FF RID: 9983
		[Unsaved(false)]
		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		// Token: 0x04002700 RID: 9984
		public WorkTags workDisables;

		// Token: 0x04002701 RID: 9985
		public WorkTags requiredWorkTags;

		// Token: 0x04002702 RID: 9986
		public List<string> spawnCategories = new List<string>();

		// Token: 0x04002703 RID: 9987
		[LoadAlias("bodyNameGlobal")]
		private string bodyTypeGlobal;

		// Token: 0x04002704 RID: 9988
		[LoadAlias("bodyNameFemale")]
		private string bodyTypeFemale;

		// Token: 0x04002705 RID: 9989
		[LoadAlias("bodyNameMale")]
		private string bodyTypeMale;

		// Token: 0x04002706 RID: 9990
		[Unsaved(false)]
		private BodyTypeDef bodyTypeGlobalResolved;

		// Token: 0x04002707 RID: 9991
		[Unsaved(false)]
		private BodyTypeDef bodyTypeFemaleResolved;

		// Token: 0x04002708 RID: 9992
		[Unsaved(false)]
		private BodyTypeDef bodyTypeMaleResolved;

		// Token: 0x04002709 RID: 9993
		public List<TraitEntry> forcedTraits;

		// Token: 0x0400270A RID: 9994
		public List<TraitEntry> disallowedTraits;

		// Token: 0x0400270B RID: 9995
		public List<string> hairTags;

		// Token: 0x0400270C RID: 9996
		private string nameMaker;

		// Token: 0x0400270D RID: 9997
		private RulePackDef nameMakerResolved;

		// Token: 0x0400270E RID: 9998
		public bool shuffleable = true;

		// Token: 0x0400270F RID: 9999
		[Unsaved(false)]
		public string untranslatedTitle;

		// Token: 0x04002710 RID: 10000
		[Unsaved(false)]
		public string untranslatedTitleFemale;

		// Token: 0x04002711 RID: 10001
		[Unsaved(false)]
		public string untranslatedTitleShort;

		// Token: 0x04002712 RID: 10002
		[Unsaved(false)]
		public string untranslatedTitleShortFemale;

		// Token: 0x04002713 RID: 10003
		[Unsaved(false)]
		public string untranslatedDesc;

		// Token: 0x04002714 RID: 10004
		[Unsaved(false)]
		public bool titleTranslated;

		// Token: 0x04002715 RID: 10005
		[Unsaved(false)]
		public bool titleFemaleTranslated;

		// Token: 0x04002716 RID: 10006
		[Unsaved(false)]
		public bool titleShortTranslated;

		// Token: 0x04002717 RID: 10007
		[Unsaved(false)]
		public bool titleShortFemaleTranslated;

		// Token: 0x04002718 RID: 10008
		[Unsaved(false)]
		public bool descTranslated;

		// Token: 0x04002719 RID: 10009
		private List<string> unlockedMeditationTypesTemp = new List<string>();
	}
}
