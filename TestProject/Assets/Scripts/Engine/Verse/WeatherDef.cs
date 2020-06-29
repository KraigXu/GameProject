using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public class WeatherDef : Def
	{
		
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001FCFE File Offset: 0x0001DEFE
		public WeatherWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = new WeatherWorker(this);
				}
				return this.workerInt;
			}
		}

		
		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		
		public IntRange durationRange = new IntRange(16000, 160000);

		
		public bool repeatable;

		
		public bool isBad;

		
		public Favorability favorability = Favorability.Neutral;

		
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		
		public SimpleCurve commonalityRainfallFactor;

		
		public float rainRate;

		
		public float snowRate;

		
		public float windSpeedFactor = 1f;

		
		public float windSpeedOffset;

		
		public float moveSpeedMultiplier = 1f;

		
		public float accuracyMultiplier = 1f;

		
		public float perceivePriority;

		
		public ThoughtDef exposedThought;

		
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		
		public List<Type> overlayClasses = new List<Type>();

		
		public SkyColorSet skyColorsNightMid;

		
		public SkyColorSet skyColorsNightEdge;

		
		public SkyColorSet skyColorsDay;

		
		public SkyColorSet skyColorsDusk;

		
		[Unsaved(false)]
		private WeatherWorker workerInt;
	}
}
