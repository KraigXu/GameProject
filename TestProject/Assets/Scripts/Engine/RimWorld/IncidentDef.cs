using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D4 RID: 2260
	public class IncidentDef : Def
	{
		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003638 RID: 13880 RVA: 0x00125E9B File Offset: 0x0012409B
		public bool NeedsParmsPoints
		{
			get
			{
				return this.category.needsParmsPoints;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003639 RID: 13881 RVA: 0x00125EA8 File Offset: 0x001240A8
		public IncidentWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (IncidentWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x0600363A RID: 13882 RVA: 0x00125EDC File Offset: 0x001240DC
		public List<IncidentDef> RefireCheckIncidents
		{
			get
			{
				if (this.refireCheckTags == null)
				{
					return null;
				}
				if (this.cachedRefireCheckIncidents == null)
				{
					this.cachedRefireCheckIncidents = new List<IncidentDef>();
					List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (this.ShouldDoRefireCheckWith(allDefsListForReading[i]))
						{
							this.cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedRefireCheckIncidents;
			}
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x00125F44 File Offset: 0x00124144
		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00125F50 File Offset: 0x00124150
		private bool ShouldDoRefireCheckWith(IncidentDef other)
		{
			if (other.tags == null)
			{
				return false;
			}
			if (other == this)
			{
				return false;
			}
			for (int i = 0; i < other.tags.Count; i++)
			{
				for (int j = 0; j < this.refireCheckTags.Count; j++)
				{
					if (other.tags[i] == this.refireCheckTags[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x00125FBB File Offset: 0x001241BB
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category == null)
			{
				yield return "category is undefined.";
			}
			if (this.targetTags == null || this.targetTags.Count == 0)
			{
				yield return "no target type";
			}
			if (this.TargetTagAllowed(IncidentTargetTagDefOf.World))
			{
				if (this.targetTags.Any((IncidentTargetTagDef tt) => tt != IncidentTargetTagDefOf.World))
				{
					yield return "allows world target type along with other targets. World targeting incidents should only target the world.";
				}
			}
			if (this.TargetTagAllowed(IncidentTargetTagDefOf.World) && this.allowedBiomes != null)
			{
				yield return "world-targeting incident has a biome restriction list";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x00125FCB File Offset: 0x001241CB
		public bool TargetTagAllowed(IncidentTargetTagDef target)
		{
			return this.targetTags.Contains(target);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x00125FD9 File Offset: 0x001241D9
		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTags.Intersect(target.IncidentTargetTags()).Any<IncidentTargetTagDef>();
		}

		// Token: 0x04001E6C RID: 7788
		public Type workerClass;

		// Token: 0x04001E6D RID: 7789
		public IncidentCategoryDef category;

		// Token: 0x04001E6E RID: 7790
		public List<IncidentTargetTagDef> targetTags;

		// Token: 0x04001E6F RID: 7791
		public float baseChance;

		// Token: 0x04001E70 RID: 7792
		public float baseChanceWithRoyalty = -1f;

		// Token: 0x04001E71 RID: 7793
		public IncidentPopulationEffect populationEffect;

		// Token: 0x04001E72 RID: 7794
		public int earliestDay;

		// Token: 0x04001E73 RID: 7795
		public int minPopulation;

		// Token: 0x04001E74 RID: 7796
		public bool requireColonistsPresent;

		// Token: 0x04001E75 RID: 7797
		public float minRefireDays;

		// Token: 0x04001E76 RID: 7798
		public int minDifficulty;

		// Token: 0x04001E77 RID: 7799
		public bool pointsScaleable;

		// Token: 0x04001E78 RID: 7800
		public float minThreatPoints = float.MinValue;

		// Token: 0x04001E79 RID: 7801
		public List<BiomeDef> allowedBiomes;

		// Token: 0x04001E7A RID: 7802
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04001E7B RID: 7803
		[NoTranslate]
		public List<string> refireCheckTags;

		// Token: 0x04001E7C RID: 7804
		public SimpleCurve chanceFactorByPopulationCurve;

		// Token: 0x04001E7D RID: 7805
		public TaleDef tale;

		// Token: 0x04001E7E RID: 7806
		public int minGreatestPopulation = -1;

		// Token: 0x04001E7F RID: 7807
		[MustTranslate]
		public string letterText;

		// Token: 0x04001E80 RID: 7808
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04001E81 RID: 7809
		public LetterDef letterDef;

		// Token: 0x04001E82 RID: 7810
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x04001E83 RID: 7811
		public PawnKindDef pawnKind;

		// Token: 0x04001E84 RID: 7812
		public bool pawnMustBeCapableOfViolence;

		// Token: 0x04001E85 RID: 7813
		public Gender pawnFixedGender;

		// Token: 0x04001E86 RID: 7814
		public GameConditionDef gameCondition;

		// Token: 0x04001E87 RID: 7815
		public FloatRange durationDays;

		// Token: 0x04001E88 RID: 7816
		public HediffDef diseaseIncident;

		// Token: 0x04001E89 RID: 7817
		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		// Token: 0x04001E8A RID: 7818
		public int diseaseMaxVictims = 99999;

		// Token: 0x04001E8B RID: 7819
		public List<BiomeDiseaseRecord> diseaseBiomeRecords;

		// Token: 0x04001E8C RID: 7820
		public List<BodyPartDef> diseasePartsToAffect;

		// Token: 0x04001E8D RID: 7821
		public ThingDef mechClusterBuilding;

		// Token: 0x04001E8E RID: 7822
		public List<MTBByBiome> mtbDaysByBiome;

		// Token: 0x04001E8F RID: 7823
		public QuestScriptDef questScriptDef;

		// Token: 0x04001E90 RID: 7824
		[Unsaved(false)]
		private IncidentWorker workerInt;

		// Token: 0x04001E91 RID: 7825
		[Unsaved(false)]
		private List<IncidentDef> cachedRefireCheckIncidents;
	}
}
