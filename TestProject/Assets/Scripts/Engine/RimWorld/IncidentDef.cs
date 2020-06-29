using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class IncidentDef : Def
	{
		
		
		public bool NeedsParmsPoints
		{
			get
			{
				return this.category.needsParmsPoints;
			}
		}

		
		
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

		
		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		
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

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
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

		
		public bool TargetTagAllowed(IncidentTargetTagDef target)
		{
			return this.targetTags.Contains(target);
		}

		
		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTags.Intersect(target.IncidentTargetTags()).Any<IncidentTargetTagDef>();
		}

		
		public Type workerClass;

		
		public IncidentCategoryDef category;

		
		public List<IncidentTargetTagDef> targetTags;

		
		public float baseChance;

		
		public float baseChanceWithRoyalty = -1f;

		
		public IncidentPopulationEffect populationEffect;

		
		public int earliestDay;

		
		public int minPopulation;

		
		public bool requireColonistsPresent;

		
		public float minRefireDays;

		
		public int minDifficulty;

		
		public bool pointsScaleable;

		
		public float minThreatPoints = float.MinValue;

		
		public List<BiomeDef> allowedBiomes;

		
		[NoTranslate]
		public List<string> tags;

		
		[NoTranslate]
		public List<string> refireCheckTags;

		
		public SimpleCurve chanceFactorByPopulationCurve;

		
		public TaleDef tale;

		
		public int minGreatestPopulation = -1;

		
		[MustTranslate]
		public string letterText;

		
		[MustTranslate]
		public string letterLabel;

		
		public LetterDef letterDef;

		
		public List<HediffDef> letterHyperlinkHediffDefs;

		
		public PawnKindDef pawnKind;

		
		public bool pawnMustBeCapableOfViolence;

		
		public Gender pawnFixedGender;

		
		public GameConditionDef gameCondition;

		
		public FloatRange durationDays;

		
		public HediffDef diseaseIncident;

		
		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		
		public int diseaseMaxVictims = 99999;

		
		public List<BiomeDiseaseRecord> diseaseBiomeRecords;

		
		public List<BodyPartDef> diseasePartsToAffect;

		
		public ThingDef mechClusterBuilding;

		
		public List<MTBByBiome> mtbDaysByBiome;

		
		public QuestScriptDef questScriptDef;

		
		[Unsaved(false)]
		private IncidentWorker workerInt;

		
		[Unsaved(false)]
		private List<IncidentDef> cachedRefireCheckIncidents;
	}
}
