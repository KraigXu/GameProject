using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAF RID: 2735
	public class WeatherDecider : IExposable
	{
		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x060040BE RID: 16574 RVA: 0x0015AE68 File Offset: 0x00159068
		public WeatherDef ForcedWeather
		{
			get
			{
				WeatherDecider.allConditionsTmp.Clear();
				this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, WeatherDecider.allConditionsTmp);
				WeatherDef result = null;
				foreach (GameCondition gameCondition in WeatherDecider.allConditionsTmp)
				{
					WeatherDef weatherDef = gameCondition.ForcedWeather();
					if (weatherDef != null)
					{
						result = weatherDef;
					}
				}
				return result;
			}
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x0015AEE8 File Offset: 0x001590E8
		public WeatherDecider(Map map)
		{
			this.map = map;
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x0015AF02 File Offset: 0x00159102
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherDuration, "curWeatherDuration", 0, true);
			Scribe_Values.Look<int>(ref this.ticksWhenRainAllowedAgain, "ticksWhenRainAllowedAgain", 0, false);
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x0015AF28 File Offset: 0x00159128
		public void WeatherDeciderTick()
		{
			WeatherDef forcedWeather = this.ForcedWeather;
			int num = this.curWeatherDuration;
			if (this.map.fireWatcher.LargeFireDangerPresent || !this.map.weatherManager.curWeather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				num = (int)((float)num * 0.25f);
			}
			if (forcedWeather != null && this.map.weatherManager.curWeather != forcedWeather)
			{
				num = 4000;
			}
			if (this.map.weatherManager.curWeatherAge > num)
			{
				this.StartNextWeather();
			}
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x0015AFC8 File Offset: 0x001591C8
		public void StartNextWeather()
		{
			WeatherDef weatherDef = this.ChooseNextWeather();
			this.map.weatherManager.TransitionTo(weatherDef);
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x0015B000 File Offset: 0x00159200
		public void StartInitialWeather()
		{
			if (Find.GameInitData != null)
			{
				this.map.weatherManager.curWeather = WeatherDefOf.Clear;
				this.curWeatherDuration = 10000;
				this.map.weatherManager.curWeatherAge = 0;
				return;
			}
			this.map.weatherManager.curWeather = null;
			WeatherDef weatherDef = this.ChooseNextWeather();
			WeatherDef lastWeather = this.ChooseNextWeather();
			this.map.weatherManager.curWeather = weatherDef;
			this.map.weatherManager.lastWeather = lastWeather;
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
			this.map.weatherManager.curWeatherAge = Rand.Range(0, this.curWeatherDuration);
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x0015B0B4 File Offset: 0x001592B4
		private WeatherDef ChooseNextWeather()
		{
			if (TutorSystem.TutorialMode)
			{
				return WeatherDefOf.Clear;
			}
			WeatherDef forcedWeather = this.ForcedWeather;
			if (forcedWeather != null)
			{
				return forcedWeather;
			}
			WeatherDef result;
			if (!DefDatabase<WeatherDef>.AllDefs.TryRandomElementByWeight((WeatherDef w) => this.CurrentWeatherCommonality(w), out result))
			{
				Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".", false);
				return WeatherDefOf.Clear;
			}
			return result;
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x0015B11A File Offset: 0x0015931A
		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x0015B130 File Offset: 0x00159330
		private float CurrentWeatherCommonality(WeatherDef weather)
		{
			if (this.map.weatherManager.curWeather != null && !this.map.weatherManager.curWeather.repeatable && weather == this.map.weatherManager.curWeather)
			{
				return 0f;
			}
			if (!weather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				return 0f;
			}
			if (weather.favorability < Favorability.Neutral && GenDate.DaysPassed < 8)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f && Find.TickManager.TicksGame < this.ticksWhenRainAllowedAgain)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f)
			{
				if (this.map.gameConditionManager.ActiveConditions.Any((GameCondition x) => x.def.preventRain))
				{
					return 0f;
				}
			}
			BiomeDef biome = this.map.Biome;
			for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
			{
				WeatherCommonalityRecord weatherCommonalityRecord = biome.baseWeatherCommonalities[i];
				if (weatherCommonalityRecord.weather == weather)
				{
					float num = weatherCommonalityRecord.commonality;
					if (this.map.fireWatcher.LargeFireDangerPresent && weather.rainRate > 0.1f)
					{
						num *= 15f;
					}
					if (weatherCommonalityRecord.weather.commonalityRainfallFactor != null)
					{
						num *= weatherCommonalityRecord.weather.commonalityRainfallFactor.Evaluate(this.map.TileInfo.rainfall);
					}
					return num;
				}
			}
			return 0f;
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x0015B2C8 File Offset: 0x001594C8
		public void LogWeatherChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeatherDef weatherDef in from w in DefDatabase<WeatherDef>.AllDefs
			orderby this.CurrentWeatherCommonality(w) descending
			select w)
			{
				stringBuilder.AppendLine(weatherDef.label + " - " + this.CurrentWeatherCommonality(weatherDef).ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04002599 RID: 9625
		private Map map;

		// Token: 0x0400259A RID: 9626
		private int curWeatherDuration = 10000;

		// Token: 0x0400259B RID: 9627
		private int ticksWhenRainAllowedAgain;

		// Token: 0x0400259C RID: 9628
		private const int FirstWeatherDuration = 10000;

		// Token: 0x0400259D RID: 9629
		private const float ChanceFactorRainOnFire = 15f;

		// Token: 0x0400259E RID: 9630
		private static List<GameCondition> allConditionsTmp = new List<GameCondition>();
	}
}
