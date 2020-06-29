using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class GameCondition_ForceWeather : GameCondition
	{
		
		public override void Init()
		{
			base.Init();
			if (this.weather == null)
			{
				this.weather = this.def.weatherDef;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		
		public override WeatherDef ForcedWeather()
		{
			return this.weather;
		}

		
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			this.weather = (from def in DefDatabase<WeatherDef>.AllDefsListForReading
			where def.isBad
			select def).RandomElement<WeatherDef>();
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("forcedWeather", this.weather));
		}

		
		public WeatherDef weather;
	}
}
