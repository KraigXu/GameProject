using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class StatDef : Def
	{
		
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

		
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x00128A86 File Offset: 0x00126C86
		public string LabelForFullStatListCap
		{
			get
			{
				return this.LabelForFullStatList.CapitalizeFirst(this);
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute, bool finalized = true)
		{
			return this.Worker.ValueToString(val, finalized, numberSense);
		}

		
		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		
		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}

		
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

		
		public StatCategoryDef category;

		
		public Type workerClass = typeof(StatWorker);

		
		public string labelForFullStatList;

		
		public bool forInformationOnly;

		
		public float hideAtValue = -2.14748365E+09f;

		
		public bool alwaysHide;

		
		public bool showNonAbstract = true;

		
		public bool showIfUndefined = true;

		
		public bool showOnPawns = true;

		
		public bool showOnHumanlikes = true;

		
		public bool showOnNonWildManHumanlikes = true;

		
		public bool showOnAnimals = true;

		
		public bool showOnMechanoids = true;

		
		public bool showOnNonWorkTables = true;

		
		public bool showOnDefaultValue = true;

		
		public bool showOnUnhaulables = true;

		
		public bool showOnUntradeables = true;

		
		public List<string> showIfModsLoaded;

		
		public List<HediffDef> showIfHediffsPresent;

		
		public bool neverDisabled;

		
		public int displayPriorityInCategory;

		
		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		
		public ToStringStyle toStringStyle;

		
		private ToStringStyle? toStringStyleUnfinalized;

		
		[MustTranslate]
		public string formatString;

		
		[MustTranslate]
		public string formatStringUnfinalized;

		
		public float defaultBaseValue = 1f;

		
		public List<SkillNeed> skillNeedOffsets;

		
		public float noSkillOffset;

		
		public List<PawnCapacityOffset> capacityOffsets;

		
		public List<StatDef> statFactors;

		
		public bool applyFactorsIfNegative = true;

		
		public List<SkillNeed> skillNeedFactors;

		
		public float noSkillFactor = 1f;

		
		public List<PawnCapacityFactor> capacityFactors;

		
		public SimpleCurve postProcessCurve;

		
		public List<StatDef> postProcessStatFactors;

		
		public float minValue = -9999999f;

		
		public float maxValue = 9999999f;

		
		public float valueIfMissing;

		
		public bool roundValue;

		
		public float roundToFiveOver = float.MaxValue;

		
		public bool minifiedThingInherits;

		
		public bool supressDisabledError;

		
		public bool scenarioRandomizable;

		
		public List<StatPart> parts;

		
		[Unsaved(false)]
		private StatWorker workerInt;
	}
}
