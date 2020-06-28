using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200121A RID: 4634
	public class WorldInfo : IExposable
	{
		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x06006B6D RID: 27501 RVA: 0x002583E6 File Offset: 0x002565E6
		public string FileNameNoExtension
		{
			get
			{
				return GenText.CapitalizedNoSpaces(this.name);
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x06006B6E RID: 27502 RVA: 0x002583F3 File Offset: 0x002565F3
		public int Seed
		{
			get
			{
				return GenText.StableStringHash(this.seedString);
			}
		}

		// Token: 0x06006B6F RID: 27503 RVA: 0x00258400 File Offset: 0x00256600
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<float>(ref this.planetCoverage, "planetCoverage", 0f, false);
			Scribe_Values.Look<string>(ref this.seedString, "seedString", null, false);
			Scribe_Values.Look<int>(ref this.persistentRandomValue, "persistentRandomValue", 0, false);
			Scribe_Values.Look<OverallRainfall>(ref this.overallRainfall, "overallRainfall", OverallRainfall.AlmostNone, false);
			Scribe_Values.Look<OverallTemperature>(ref this.overallTemperature, "overallTemperature", OverallTemperature.VeryCold, false);
			Scribe_Values.Look<IntVec3>(ref this.initialMapSize, "initialMapSize", default(IntVec3), false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0400432A RID: 17194
		public string name = "DefaultWorldName";

		// Token: 0x0400432B RID: 17195
		public float planetCoverage;

		// Token: 0x0400432C RID: 17196
		public string seedString = "SeedError";

		// Token: 0x0400432D RID: 17197
		public int persistentRandomValue = Rand.Int;

		// Token: 0x0400432E RID: 17198
		public OverallRainfall overallRainfall = OverallRainfall.Normal;

		// Token: 0x0400432F RID: 17199
		public OverallTemperature overallTemperature = OverallTemperature.Normal;

		// Token: 0x04004330 RID: 17200
		public OverallPopulation overallPopulation = OverallPopulation.Normal;

		// Token: 0x04004331 RID: 17201
		public IntVec3 initialMapSize = new IntVec3(250, 1, 250);
	}
}
