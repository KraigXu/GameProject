using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD6 RID: 3030
	public class Trait : IExposable
	{
		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x060047E9 RID: 18409 RVA: 0x00185C28 File Offset: 0x00183E28
		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060047EA RID: 18410 RVA: 0x00185C30 File Offset: 0x00183E30
		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x060047EB RID: 18411 RVA: 0x00185C43 File Offset: 0x00183E43
		public string Label
		{
			get
			{
				return this.CurrentData.label;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060047EC RID: 18412 RVA: 0x00185C50 File Offset: 0x00183E50
		public string LabelCap
		{
			get
			{
				return this.CurrentData.LabelCap;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x060047ED RID: 18413 RVA: 0x00185C5D File Offset: 0x00183E5D
		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public Trait()
		{
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x00185C65 File Offset: 0x00183E65
		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x00185C84 File Offset: 0x00183E84
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

		// Token: 0x060047F1 RID: 18417 RVA: 0x00185CF4 File Offset: 0x00183EF4
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

		// Token: 0x060047F2 RID: 18418 RVA: 0x00185D58 File Offset: 0x00183F58
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

		// Token: 0x060047F3 RID: 18419 RVA: 0x00185DBC File Offset: 0x00183FBC
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

		// Token: 0x060047F4 RID: 18420 RVA: 0x0018617C File Offset: 0x0018437C
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

		// Token: 0x060047F5 RID: 18421 RVA: 0x001861C8 File Offset: 0x001843C8
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

		// Token: 0x060047F6 RID: 18422 RVA: 0x001861D8 File Offset: 0x001843D8
		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x001861EF File Offset: 0x001843EF
		public void Notify_MentalStateEndedOn(Pawn pawn, bool causedByMood)
		{
			if (causedByMood)
			{
				this.Notify_MentalStateEndedOn(pawn);
			}
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x001861FC File Offset: 0x001843FC
		public void Notify_MentalStateEndedOn(Pawn pawn)
		{
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.mentalBreakInspirationGainSet.NullOrEmpty<InspirationDef>() || Rand.Value > currentData.mentalBreakInspirationGainChance)
			{
				return;
			}
			pawn.mindState.inspirationHandler.TryStartInspiration_NewTemp(currentData.mentalBreakInspirationGainSet.RandomElement<InspirationDef>(), currentData.mentalBreakInspirationGainReasonText);
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x0018624D File Offset: 0x0018444D
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

		// Token: 0x0400293E RID: 10558
		public TraitDef def;

		// Token: 0x0400293F RID: 10559
		private int degree;

		// Token: 0x04002940 RID: 10560
		private bool scenForced;
	}
}
