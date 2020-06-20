using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020009BF RID: 2495
	public class GameCondition_ForceWeather : GameCondition
	{
		// Token: 0x06003B9D RID: 15261 RVA: 0x0013AE18 File Offset: 0x00139018
		public override void Init()
		{
			base.Init();
			if (this.weather == null)
			{
				this.weather = this.def.weatherDef;
			}
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x0013AE39 File Offset: 0x00139039
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x0013AE51 File Offset: 0x00139051
		public override WeatherDef ForcedWeather()
		{
			return this.weather;
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0013AE5C File Offset: 0x0013905C
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			this.weather = (from def in DefDatabase<WeatherDef>.AllDefsListForReading
			where def.isBad
			select def).RandomElement<WeatherDef>();
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("forcedWeather", this.weather));
		}

		// Token: 0x04002331 RID: 9009
		public WeatherDef weather;
	}
}
