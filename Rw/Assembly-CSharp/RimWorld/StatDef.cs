using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000906 RID: 2310
	public class StatDef : Def
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060036FD RID: 14077 RVA: 0x001289DC File Offset: 0x00126BDC
		public StatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.parts != null)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							this.parts[i].parentStat = this;
						}
					}
					this.workerInt = (StatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.InitSetStat(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060036FE RID: 14078 RVA: 0x00128A49 File Offset: 0x00126C49
		public ToStringStyle ToStringStyleUnfinalized
		{
			get
			{
				if (this.toStringStyleUnfinalized == null)
				{
					return this.toStringStyle;
				}
				return this.toStringStyleUnfinalized.Value;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060036FF RID: 14079 RVA: 0x00128A6A File Offset: 0x00126C6A
		public string LabelForFullStatList
		{
			get
			{
				if (!this.labelForFullStatList.NullOrEmpty())
				{
					return this.labelForFullStatList;
				}
				return this.label;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x00128A86 File Offset: 0x00126C86
		public string LabelForFullStatListCap
		{
			get
			{
				return this.LabelForFullStatList.CapitalizeFirst(this);
			}
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x00128A94 File Offset: 0x00126C94
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.capacityFactors != null)
			{
				using (List<PawnCapacityFactor>.Enumerator enumerator2 = this.capacityFactors.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.weight > 1f)
						{
							yield return this.defName + " has activity factor with weight > 1";
						}
					}
				}
				List<PawnCapacityFactor>.Enumerator enumerator2 = default(List<PawnCapacityFactor>.Enumerator);
			}
			if (this.parts != null)
			{
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					foreach (string text2 in this.parts[i].ConfigErrors())
					{
						yield return string.Concat(new string[]
						{
							this.defName,
							" has error in StatPart ",
							this.parts[i].ToString(),
							": ",
							text2
						});
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x00128AA4 File Offset: 0x00126CA4
		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute, bool finalized = true)
		{
			return this.Worker.ValueToString(val, finalized, numberSense);
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x00128AB4 File Offset: 0x00126CB4
		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x00128AC0 File Offset: 0x00126CC0
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x00128B28 File Offset: 0x00126D28
		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x00128B3C File Offset: 0x00126D3C
		public bool CanShowWithLoadedMods()
		{
			if (!this.showIfModsLoaded.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.showIfModsLoaded.Count; i++)
				{
					if (!ModsConfig.IsActive(this.showIfModsLoaded[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001FCB RID: 8139
		public StatCategoryDef category;

		// Token: 0x04001FCC RID: 8140
		public Type workerClass = typeof(StatWorker);

		// Token: 0x04001FCD RID: 8141
		public string labelForFullStatList;

		// Token: 0x04001FCE RID: 8142
		public bool forInformationOnly;

		// Token: 0x04001FCF RID: 8143
		public float hideAtValue = -2.14748365E+09f;

		// Token: 0x04001FD0 RID: 8144
		public bool alwaysHide;

		// Token: 0x04001FD1 RID: 8145
		public bool showNonAbstract = true;

		// Token: 0x04001FD2 RID: 8146
		public bool showIfUndefined = true;

		// Token: 0x04001FD3 RID: 8147
		public bool showOnPawns = true;

		// Token: 0x04001FD4 RID: 8148
		public bool showOnHumanlikes = true;

		// Token: 0x04001FD5 RID: 8149
		public bool showOnNonWildManHumanlikes = true;

		// Token: 0x04001FD6 RID: 8150
		public bool showOnAnimals = true;

		// Token: 0x04001FD7 RID: 8151
		public bool showOnMechanoids = true;

		// Token: 0x04001FD8 RID: 8152
		public bool showOnNonWorkTables = true;

		// Token: 0x04001FD9 RID: 8153
		public bool showOnDefaultValue = true;

		// Token: 0x04001FDA RID: 8154
		public bool showOnUnhaulables = true;

		// Token: 0x04001FDB RID: 8155
		public bool showOnUntradeables = true;

		// Token: 0x04001FDC RID: 8156
		public List<string> showIfModsLoaded;

		// Token: 0x04001FDD RID: 8157
		public List<HediffDef> showIfHediffsPresent;

		// Token: 0x04001FDE RID: 8158
		public bool neverDisabled;

		// Token: 0x04001FDF RID: 8159
		public int displayPriorityInCategory;

		// Token: 0x04001FE0 RID: 8160
		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		// Token: 0x04001FE1 RID: 8161
		public ToStringStyle toStringStyle;

		// Token: 0x04001FE2 RID: 8162
		private ToStringStyle? toStringStyleUnfinalized;

		// Token: 0x04001FE3 RID: 8163
		[MustTranslate]
		public string formatString;

		// Token: 0x04001FE4 RID: 8164
		[MustTranslate]
		public string formatStringUnfinalized;

		// Token: 0x04001FE5 RID: 8165
		public float defaultBaseValue = 1f;

		// Token: 0x04001FE6 RID: 8166
		public List<SkillNeed> skillNeedOffsets;

		// Token: 0x04001FE7 RID: 8167
		public float noSkillOffset;

		// Token: 0x04001FE8 RID: 8168
		public List<PawnCapacityOffset> capacityOffsets;

		// Token: 0x04001FE9 RID: 8169
		public List<StatDef> statFactors;

		// Token: 0x04001FEA RID: 8170
		public bool applyFactorsIfNegative = true;

		// Token: 0x04001FEB RID: 8171
		public List<SkillNeed> skillNeedFactors;

		// Token: 0x04001FEC RID: 8172
		public float noSkillFactor = 1f;

		// Token: 0x04001FED RID: 8173
		public List<PawnCapacityFactor> capacityFactors;

		// Token: 0x04001FEE RID: 8174
		public SimpleCurve postProcessCurve;

		// Token: 0x04001FEF RID: 8175
		public List<StatDef> postProcessStatFactors;

		// Token: 0x04001FF0 RID: 8176
		public float minValue = -9999999f;

		// Token: 0x04001FF1 RID: 8177
		public float maxValue = 9999999f;

		// Token: 0x04001FF2 RID: 8178
		public float valueIfMissing;

		// Token: 0x04001FF3 RID: 8179
		public bool roundValue;

		// Token: 0x04001FF4 RID: 8180
		public float roundToFiveOver = float.MaxValue;

		// Token: 0x04001FF5 RID: 8181
		public bool minifiedThingInherits;

		// Token: 0x04001FF6 RID: 8182
		public bool supressDisabledError;

		// Token: 0x04001FF7 RID: 8183
		public bool scenarioRandomizable;

		// Token: 0x04001FF8 RID: 8184
		public List<StatPart> parts;

		// Token: 0x04001FF9 RID: 8185
		[Unsaved(false)]
		private StatWorker workerInt;
	}
}
