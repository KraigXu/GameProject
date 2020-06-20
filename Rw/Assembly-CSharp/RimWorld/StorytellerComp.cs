using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A04 RID: 2564
	public abstract class StorytellerComp
	{
		// Token: 0x06003CFE RID: 15614 RVA: 0x00143143 File Offset: 0x00141343
		public virtual IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			yield break;
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x0014314C File Offset: 0x0014134C
		public virtual IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(incCat, target);
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00143158 File Offset: 0x00141358
		[Obsolete("Use IncidentParms argument instead")]
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IIncidentTarget target)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => this.GenerateParms(cat, target));
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x00143198 File Offset: 0x00141398
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IncidentParms parms)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => parms);
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x001431C8 File Offset: 0x001413C8
		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, Func<IncidentDef, IncidentParms> parmsGetter)
		{
			return from x in DefDatabase<IncidentDef>.AllDefsListForReading
			where x.category == cat && x.Worker.CanFireNow(parmsGetter(x), false)
			select x;
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x00143200 File Offset: 0x00141400
		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			if (def.chanceFactorByPopulationCurve == null)
			{
				return 1f;
			}
			int num = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			return def.chanceFactorByPopulationCurve.Evaluate((float)num);
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x00143234 File Offset: 0x00141434
		protected float IncidentChanceFactor_PopulationIntent(IncidentDef def)
		{
			if (def.populationEffect == IncidentPopulationEffect.None)
			{
				return 1f;
			}
			float num;
			switch (def.populationEffect)
			{
			case IncidentPopulationEffect.IncreaseHard:
				num = 0.4f;
				break;
			case IncidentPopulationEffect.IncreaseMedium:
				num = 0f;
				break;
			case IncidentPopulationEffect.IncreaseEasy:
				num = -0.4f;
				break;
			default:
				throw new Exception();
			}
			return Mathf.Max(StorytellerUtilityPopulation.PopulationIntent + num, this.props.minIncChancePopulationIntentFactor);
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x001432A4 File Offset: 0x001414A4
		protected float IncidentChanceFinal(IncidentDef def)
		{
			float num = def.Worker.BaseChanceThisGame;
			num *= this.IncidentChanceFactor_CurrentPopulation(def);
			num *= this.IncidentChanceFactor_PopulationIntent(def);
			return Mathf.Max(0f, num);
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Initialize()
		{
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x001432DC File Offset: 0x001414DC
		public override string ToString()
		{
			string text = base.GetType().Name;
			string text2 = typeof(StorytellerComp).Name + "_";
			if (text.StartsWith(text2))
			{
				text = text.Substring(text2.Length);
			}
			if (!this.props.allowedTargetTags.NullOrEmpty<IncidentTargetTagDef>())
			{
				text = text + " (" + (from x in this.props.allowedTargetTags
				select x.ToString()).ToCommaList(false) + ")";
			}
			return text;
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x00143380 File Offset: 0x00141580
		public virtual void DebugTablesIncidentChances()
		{
			IEnumerable<IncidentDef> dataSources = from d in DefDatabase<IncidentDef>.AllDefs
			orderby d.category.defName descending, this.IncidentChanceFinal(d) descending
			select d;
			TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[14];
			array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			array[1] = new TableDataGetter<IncidentDef>("category", (IncidentDef d) => d.category);
			array[2] = new TableDataGetter<IncidentDef>("can fire", (IncidentDef d) => StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|12_1(d).ToStringCheckBlank());
			array[3] = new TableDataGetter<IncidentDef>("base\nchance", (IncidentDef d) => d.baseChance.ToString("F2"));
			array[4] = new TableDataGetter<IncidentDef>("base\nchance\nwith\nRoyalty", delegate(IncidentDef d)
			{
				if (d.baseChanceWithRoyalty < 0f)
				{
					return "-";
				}
				return d.baseChanceWithRoyalty.ToString("F2");
			});
			array[5] = new TableDataGetter<IncidentDef>("base\nchance\nthis\ngame", (IncidentDef d) => d.Worker.BaseChanceThisGame.ToString("F2"));
			array[6] = new TableDataGetter<IncidentDef>("final\nchance", (IncidentDef d) => this.IncidentChanceFinal(d).ToString("F2"));
			array[7] = new TableDataGetter<IncidentDef>("final\nchance\npossible", delegate(IncidentDef d)
			{
				if (!StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|12_1(d))
				{
					return "-";
				}
				return this.IncidentChanceFinal(d).ToString("F2");
			});
			array[8] = new TableDataGetter<IncidentDef>("Factor from:\ncurrent pop", (IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString());
			array[9] = new TableDataGetter<IncidentDef>("Factor from:\npop intent", (IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString());
			array[10] = new TableDataGetter<IncidentDef>("default target", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d) == null)
				{
					return "-";
				}
				return StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d).ToString();
			});
			array[11] = new TableDataGetter<IncidentDef>("current\npop", (IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>().ToString());
			array[12] = new TableDataGetter<IncidentDef>("pop\nintent", (IncidentDef d) => StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			array[13] = new TableDataGetter<IncidentDef>("cur\npoints", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d) == null)
				{
					return "-";
				}
				return StorytellerUtility.DefaultThreatPointsNow(StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d)).ToString("F0");
			});
			DebugTables.MakeTablesDialog<IncidentDef>(dataSources, array);
		}

		// Token: 0x040023A3 RID: 9123
		public StorytellerCompProperties props;
	}
}
