using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		
		// (get) Token: 0x06004470 RID: 17520 RVA: 0x0017205C File Offset: 0x0017025C
		public RulePackDef NameMaker
		{
			get
			{
				return this.nameMakerResolved;
			}
		}

		
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

		
		public string TitleFor(Gender g)
		{
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				return this.title;
			}
			return this.titleFemale;
		}

		
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		
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

		
		public string TitleShortCapFor(Gender g)
		{
			return this.TitleShortFor(g).CapitalizeFirst();
		}

		
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

		
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		
		internal void AddForcedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.forcedTraits == null)
			{
				this.forcedTraits = new List<TraitEntry>();
			}
			this.forcedTraits.Add(new TraitEntry(traitDef, degree));
		}

		
		internal void AddDisallowedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.disallowedTraits == null)
			{
				this.disallowedTraits = new List<TraitEntry>();
			}
			this.disallowedTraits.Add(new TraitEntry(traitDef, degree));
		}

		
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

		
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		
		public override string ToString()
		{
			if (this.title.NullOrEmpty())
			{
				return "(NullTitleBackstory)";
			}
			return "(" + this.title + ")";
		}

		
		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		
		public string identifier;

		
		public BackstorySlot slot;

		
		public string title;

		
		public string titleFemale;

		
		public string titleShort;

		
		public string titleShortFemale;

		
		public string baseDesc;

		
		private Dictionary<string, int> skillGains = new Dictionary<string, int>();

		
		[Unsaved(false)]
		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		
		public WorkTags workDisables;

		
		public WorkTags requiredWorkTags;

		
		public List<string> spawnCategories = new List<string>();

		
		[LoadAlias("bodyNameGlobal")]
		private string bodyTypeGlobal;

		
		[LoadAlias("bodyNameFemale")]
		private string bodyTypeFemale;

		
		[LoadAlias("bodyNameMale")]
		private string bodyTypeMale;

		
		[Unsaved(false)]
		private BodyTypeDef bodyTypeGlobalResolved;

		
		[Unsaved(false)]
		private BodyTypeDef bodyTypeFemaleResolved;

		
		[Unsaved(false)]
		private BodyTypeDef bodyTypeMaleResolved;

		
		public List<TraitEntry> forcedTraits;

		
		public List<TraitEntry> disallowedTraits;

		
		public List<string> hairTags;

		
		private string nameMaker;

		
		private RulePackDef nameMakerResolved;

		
		public bool shuffleable = true;

		
		[Unsaved(false)]
		public string untranslatedTitle;

		
		[Unsaved(false)]
		public string untranslatedTitleFemale;

		
		[Unsaved(false)]
		public string untranslatedTitleShort;

		
		[Unsaved(false)]
		public string untranslatedTitleShortFemale;

		
		[Unsaved(false)]
		public string untranslatedDesc;

		
		[Unsaved(false)]
		public bool titleTranslated;

		
		[Unsaved(false)]
		public bool titleFemaleTranslated;

		
		[Unsaved(false)]
		public bool titleShortTranslated;

		
		[Unsaved(false)]
		public bool titleShortFemaleTranslated;

		
		[Unsaved(false)]
		public bool descTranslated;

		
		private List<string> unlockedMeditationTypesTemp = new List<string>();
	}
}
