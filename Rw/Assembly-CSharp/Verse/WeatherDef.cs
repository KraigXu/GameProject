using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000FD RID: 253
	public class WeatherDef : Def
	{
		// Token: 0x17000182 RID: 386
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

		// Token: 0x060006EB RID: 1771 RVA: 0x0001FD1A File Offset: 0x0001DF1A
		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001FD2E File Offset: 0x0001DF2E
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001FD3E File Offset: 0x0001DF3E
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		// Token: 0x04000654 RID: 1620
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x04000655 RID: 1621
		public bool repeatable;

		// Token: 0x04000656 RID: 1622
		public bool isBad;

		// Token: 0x04000657 RID: 1623
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x04000658 RID: 1624
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x04000659 RID: 1625
		public SimpleCurve commonalityRainfallFactor;

		// Token: 0x0400065A RID: 1626
		public float rainRate;

		// Token: 0x0400065B RID: 1627
		public float snowRate;

		// Token: 0x0400065C RID: 1628
		public float windSpeedFactor = 1f;

		// Token: 0x0400065D RID: 1629
		public float windSpeedOffset;

		// Token: 0x0400065E RID: 1630
		public float moveSpeedMultiplier = 1f;

		// Token: 0x0400065F RID: 1631
		public float accuracyMultiplier = 1f;

		// Token: 0x04000660 RID: 1632
		public float perceivePriority;

		// Token: 0x04000661 RID: 1633
		public ThoughtDef exposedThought;

		// Token: 0x04000662 RID: 1634
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x04000663 RID: 1635
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x04000664 RID: 1636
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x04000665 RID: 1637
		public SkyColorSet skyColorsNightMid;

		// Token: 0x04000666 RID: 1638
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x04000667 RID: 1639
		public SkyColorSet skyColorsDay;

		// Token: 0x04000668 RID: 1640
		public SkyColorSet skyColorsDusk;

		// Token: 0x04000669 RID: 1641
		[Unsaved(false)]
		private WeatherWorker workerInt;
	}
}
