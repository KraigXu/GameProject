using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDC RID: 3292
	public class CompCauseGameCondition_ForceWeather : CompCauseGameCondition
	{
		// Token: 0x06004FE2 RID: 20450 RVA: 0x001AF3CB File Offset: 0x001AD5CB
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.weather = base.Props.conditionDef.weatherDef;
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x001AF3EA File Offset: 0x001AD5EA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x001AF402 File Offset: 0x001AD602
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

		// Token: 0x06004FE5 RID: 20453 RVA: 0x001AF412 File Offset: 0x001AD612
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_ForceWeather)condition).weather = this.weather;
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x001AF430 File Offset: 0x001AD630
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "Weather".Translate() + ": " + this.weather.LabelCap;
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001AF487 File Offset: 0x001AD687
		public override void RandomizeSettings()
		{
			this.weather = (from x in DefDatabase<WeatherDef>.AllDefsListForReading
			where x.isBad
			select x).RandomElement<WeatherDef>();
		}

		// Token: 0x04002CA8 RID: 11432
		public WeatherDef weather;
	}
}
