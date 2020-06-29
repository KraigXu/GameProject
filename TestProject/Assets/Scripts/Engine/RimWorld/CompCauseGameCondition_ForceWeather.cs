using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class CompCauseGameCondition_ForceWeather : CompCauseGameCondition
	{
		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.weather = base.Props.conditionDef.weatherDef;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.weather.LabelCap,
				action = delegate
				{
					List<WeatherDef> allDefsListForReading = DefDatabase<WeatherDef>.AllDefsListForReading;
					int num = allDefsListForReading.FindIndex((WeatherDef w) => w == this.weather);
					num++;
					if (num >= allDefsListForReading.Count)
					{
						num = 0;
					}
					this.weather = allDefsListForReading[num];
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield break;
		}

		
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_ForceWeather)condition).weather = this.weather;
		}

		
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "Weather".Translate() + ": " + this.weather.LabelCap;
		}

		
		public override void RandomizeSettings()
		{
			this.weather = (from x in DefDatabase<WeatherDef>.AllDefsListForReading
			where x.isBad
			select x).RandomElement<WeatherDef>();
		}

		
		public WeatherDef weather;
	}
}
