using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200124D RID: 4685
	public class Caravan_ForageTracker : IExposable
	{
		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06006D30 RID: 27952 RVA: 0x00263973 File Offset: 0x00261B73
		public Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				return ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, null);
			}
		}

		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06006D31 RID: 27953 RVA: 0x00263984 File Offset: 0x00261B84
		public string ForagedFoodPerDayExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06006D32 RID: 27954 RVA: 0x002639AA File Offset: 0x00261BAA
		public Caravan_ForageTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06006D33 RID: 27955 RVA: 0x002639B9 File Offset: 0x00261BB9
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
		}

		// Token: 0x06006D34 RID: 27956 RVA: 0x002639D1 File Offset: 0x00261BD1
		public void ForageTrackerTick()
		{
			if (this.caravan.IsHashIntervalTick(10))
			{
				this.UpdateProgressInterval();
			}
		}

		// Token: 0x06006D35 RID: 27957 RVA: 0x002639E8 File Offset: 0x00261BE8
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Forage",
					action = new Action(this.Forage)
				};
			}
			yield break;
		}

		// Token: 0x06006D36 RID: 27958 RVA: 0x002639F8 File Offset: 0x00261BF8
		private void UpdateProgressInterval()
		{
			float num = 10f * ForagedFoodPerDayCalculator.GetProgressPerTick(this.caravan, null);
			this.progress += num;
			if (this.progress >= 1f)
			{
				this.Forage();
				this.progress = 0f;
			}
		}

		// Token: 0x06006D37 RID: 27959 RVA: 0x00263A44 File Offset: 0x00261C44
		private void Forage()
		{
			ThingDef foragedFood = this.caravan.Biome.foragedFood;
			if (foragedFood == null)
			{
				return;
			}
			int i = GenMath.RoundRandom(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(this.caravan, null));
			int b = Mathf.FloorToInt((this.caravan.MassCapacity - this.caravan.MassUsage) / foragedFood.GetStatValueAbstract(StatDefOf.Mass, null));
			i = Mathf.Min(i, b);
			while (i > 0)
			{
				Thing thing = ThingMaker.MakeThing(foragedFood, null);
				thing.stackCount = Mathf.Min(i, foragedFood.stackLimit);
				i -= thing.stackCount;
				CaravanInventoryUtility.GiveThing(this.caravan, thing);
			}
		}

		// Token: 0x040043D5 RID: 17365
		private Caravan caravan;

		// Token: 0x040043D6 RID: 17366
		private float progress;

		// Token: 0x040043D7 RID: 17367
		private const int UpdateProgressIntervalTicks = 10;
	}
}
