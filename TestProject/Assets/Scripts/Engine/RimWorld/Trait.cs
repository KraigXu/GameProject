using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Trait : IExposable
	{
		
		
		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		
		
		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		
		
		public string Label
		{
			get
			{
				return this.CurrentData.label;
			}
		}

		
		
		public string LabelCap
		{
			get
			{
				return this.CurrentData.LabelCap;
			}
		}

		
		
		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		
		public Trait()
		{
		}

		
		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<TraitDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
			Scribe_Values.Look<bool>(ref this.scenForced, "scenForced", false, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == null)
			{
				this.def = DefDatabase<TraitDef>.GetRandom();
				this.degree = PawnGenerator.RandomTraitDegree(this.def);
			}
		}

		
		public float OffsetOfStat(StatDef stat)
		{
			float num = 0f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statOffsets != null)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					if (currentData.statOffsets[i].stat == stat)
					{
						num += currentData.statOffsets[i].value;
					}
				}
			}
			return num;
		}

		
		public float MultiplierOfStat(StatDef stat)
		{
			float num = 1f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statFactors != null)
			{
				for (int i = 0; i < currentData.statFactors.Count; i++)
				{
					if (currentData.statFactors[i].stat == stat)
					{
						num *= currentData.statFactors[i].value;
					}
				}
			}
			return num;
		}

		
		public string TipString(Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TraitDegreeData currentData = this.CurrentData;
			stringBuilder.Append(currentData.description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).Resolve());
			bool flag = this.CurrentData.skillGains.Count > 0;
			bool flag2 = this.GetPermaThoughts().Any<ThoughtDef>();
			bool flag3 = currentData.statOffsets != null;
			bool flag4 = currentData.statFactors != null;
			if (flag || flag2 || flag3 || flag4)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
			if (flag)
			{
				foreach (KeyValuePair<SkillDef, int> keyValuePair in this.CurrentData.skillGains)
				{
					if (keyValuePair.Value != 0)
					{
						string value = "    " + keyValuePair.Key.skillLabel.CapitalizeFirst() + ":   " + keyValuePair.Value.ToString("+##;-##");
						stringBuilder.AppendLine(value);
					}
				}
			}
			if (flag2)
			{
				foreach (ThoughtDef thoughtDef in this.GetPermaThoughts())
				{
					stringBuilder.AppendLine("    " + "PermanentMoodEffect".Translate() + " " + thoughtDef.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
				}
			}
			if (flag3)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					StatModifier statModifier = currentData.statOffsets[i];
					string valueToStringAsOffset = statModifier.ValueToStringAsOffset;
					string value2 = "    " + statModifier.stat.LabelCap + " " + valueToStringAsOffset;
					stringBuilder.AppendLine(value2);
				}
			}
			if (flag4)
			{
				for (int j = 0; j < currentData.statFactors.Count; j++)
				{
					StatModifier statModifier2 = currentData.statFactors[j];
					string toStringAsFactor = statModifier2.ToStringAsFactor;
					string value3 = "    " + statModifier2.stat.LabelCap + " " + toStringAsFactor;
					stringBuilder.AppendLine(value3);
				}
			}
			if (currentData.hungerRateFactor != 1f)
			{
				string t = currentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor);
				string value4 = "    " + "HungerRate".Translate() + " " + t;
				stringBuilder.AppendLine(value4);
			}
			if (ModsConfig.RoyaltyActive)
			{
				List<MeditationFocusDef> allowedMeditationFocusTypes = this.CurrentData.allowedMeditationFocusTypes;
				if (!allowedMeditationFocusTypes.NullOrEmpty<MeditationFocusDef>())
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("EnablesMeditationFocusType".Translate() + ":\n" + (from f in allowedMeditationFocusTypes
					select f.LabelCap.RawText).ToLineList("  - ", false));
				}
			}
			if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '\n')
			{
				if (stringBuilder.Length > 1 && stringBuilder[stringBuilder.Length - 2] == '\r')
				{
					stringBuilder.Remove(stringBuilder.Length - 2, 2);
				}
				else
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
			}
			return stringBuilder.ToString();
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Trait(",
				this.def.ToString(),
				"-",
				this.degree,
				")"
			});
		}

		
		private IEnumerable<ThoughtDef> GetPermaThoughts()
		{
			TraitDegreeData degree = this.CurrentData;
			List<ThoughtDef> allThoughts = DefDatabase<ThoughtDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < allThoughts.Count; i = num + 1)
			{
				if (allThoughts[i].IsSituational && allThoughts[i].Worker is ThoughtWorker_AlwaysActive && allThoughts[i].requiredTraits != null && allThoughts[i].requiredTraits.Contains(this.def) && (!allThoughts[i].RequiresSpecificTraitsDegree || allThoughts[i].requiredTraitsDegree == degree.degree))
				{
					yield return allThoughts[i];
				}
				num = i;
			}
			yield break;
		}

		
		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		
		public void Notify_MentalStateEndedOn(Pawn pawn, bool causedByMood)
		{
			if (causedByMood)
			{
				this.Notify_MentalStateEndedOn(pawn);
			}
		}

		
		public void Notify_MentalStateEndedOn(Pawn pawn)
		{
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.mentalBreakInspirationGainSet.NullOrEmpty<InspirationDef>() || Rand.Value > currentData.mentalBreakInspirationGainChance)
			{
				return;
			}
			pawn.mindState.inspirationHandler.TryStartInspiration_NewTemp(currentData.mentalBreakInspirationGainSet.RandomElement<InspirationDef>(), currentData.mentalBreakInspirationGainReasonText);
		}

		
		public IEnumerable<WorkTypeDef> GetDisabledWorkTypes()
		{
			int num;
			for (int i = 0; i < this.def.disabledWorkTypes.Count; i = num + 1)
			{
				yield return this.def.disabledWorkTypes[i];
				num = i;
			}
			List<WorkTypeDef> workTypeDefList = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < workTypeDefList.Count; i = num + 1)
			{
				WorkTypeDef workTypeDef = workTypeDefList[i];
				if (!this.AllowsWorkType(workTypeDef))
				{
					yield return workTypeDef;
				}
				num = i;
			}
			yield break;
		}

		
		public TraitDef def;

		
		private int degree;

		
		private bool scenForced;
	}
}
