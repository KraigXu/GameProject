using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class StorytellerComp
	{
		
		public virtual IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			yield break;
		}

		
		public virtual void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
		}

		
		public virtual IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(incCat, target);
		}

		
		[Obsolete("Use IncidentParms argument instead")]
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IIncidentTarget target)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => this.GenerateParms(cat, target));
		}

		
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IncidentParms parms)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => parms);
		}

		
		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, Func<IncidentDef, IncidentParms> parmsGetter)
		{
			return from x in DefDatabase<IncidentDef>.AllDefsListForReading
			where x.category == cat && x.Worker.CanFireNow(parmsGetter(x), false)
			select x;
		}

		
		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			if (def.chanceFactorByPopulationCurve == null)
			{
				return 1f;
			}
			int num = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			return def.chanceFactorByPopulationCurve.Evaluate((float)num);
		}

		
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

		
		protected float IncidentChanceFinal(IncidentDef def)
		{
			float num = def.Worker.BaseChanceThisGame;
			num *= this.IncidentChanceFactor_CurrentPopulation(def);
			num *= this.IncidentChanceFactor_PopulationIntent(def);
			return Mathf.Max(0f, num);
		}

		
		public virtual void Initialize()
		{
		}

		
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

		
		public virtual void DebugTablesIncidentChances()
		{
			//IEnumerable<IncidentDef> dataSources = from d in DefDatabase<IncidentDef>.AllDefs
			//orderby d.category.defName descending, this.IncidentChanceFinal(d) descending
			//select d;
			//TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[14];
			//array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			//array[1] = new TableDataGetter<IncidentDef>("category", (IncidentDef d) => d.category);
			//array[2] = new TableDataGetter<IncidentDef>("can fire", (IncidentDef d) => StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|12_1(d).ToStringCheckBlank());
			//array[3] = new TableDataGetter<IncidentDef>("base\nchance", (IncidentDef d) => d.baseChance.ToString("F2"));
			//array[4] = new TableDataGetter<IncidentDef>("base\nchance\nwith\nRoyalty", delegate(IncidentDef d)
			//{
			//	if (d.baseChanceWithRoyalty < 0f)
			//	{
			//		return "-";
			//	}
			//	return d.baseChanceWithRoyalty.ToString("F2");
			//});
			//array[5] = new TableDataGetter<IncidentDef>("base\nchance\nthis\ngame", (IncidentDef d) => d.Worker.BaseChanceThisGame.ToString("F2"));
			//array[6] = new TableDataGetter<IncidentDef>("final\nchance", (IncidentDef d) => this.IncidentChanceFinal(d).ToString("F2"));
			//array[7] = new TableDataGetter<IncidentDef>("final\nchance\npossible", delegate(IncidentDef d)
			//{
			//	if (!StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|12_1(d))
			//	{
			//		return "-";
			//	}
			//	return this.IncidentChanceFinal(d).ToString("F2");
			//});
			//array[8] = new TableDataGetter<IncidentDef>("Factor from:\ncurrent pop", (IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString());
			//array[9] = new TableDataGetter<IncidentDef>("Factor from:\npop intent", (IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString());
			//array[10] = new TableDataGetter<IncidentDef>("default target", delegate(IncidentDef d)
			//{
			//	if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d) == null)
			//	{
			//		return "-";
			//	}
			//	return StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d).ToString();
			//});
			//array[11] = new TableDataGetter<IncidentDef>("current\npop", (IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>().ToString());
			//array[12] = new TableDataGetter<IncidentDef>("pop\nintent", (IncidentDef d) => StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			//array[13] = new TableDataGetter<IncidentDef>("cur\npoints", delegate(IncidentDef d)
			//{
			//	if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d) == null)
			//	{
			//		return "-";
			//	}
			//	return StorytellerUtility.DefaultThreatPointsNow(StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d)).ToString("F0");
			//});
			//DebugTables.MakeTablesDialog<IncidentDef>(dataSources, array);
		}

		
		public StorytellerCompProperties props;
	}
}
