using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102B RID: 4139
	public class ThreatsGeneratorParams : IExposable
	{
		// Token: 0x06006313 RID: 25363 RVA: 0x00226D20 File Offset: 0x00224F20
		public void ExposeData()
		{
			Scribe_Values.Look<AllowedThreatsGeneratorThreats>(ref this.allowedThreats, "allowedThreats", AllowedThreatsGeneratorThreats.None, false);
			Scribe_Values.Look<int>(ref this.randSeed, "randSeed", 0, false);
			Scribe_Values.Look<float>(ref this.onDays, "onDays", 0f, false);
			Scribe_Values.Look<float>(ref this.offDays, "offDays", 0f, false);
			Scribe_Values.Look<float>(ref this.minSpacingDays, "minSpacingDays", 0f, false);
			Scribe_Values.Look<FloatRange>(ref this.numIncidentsRange, "numIncidentsRange", default(FloatRange), false);
			Scribe_Values.Look<float?>(ref this.threatPoints, "threatPoints", null, false);
			Scribe_Values.Look<float?>(ref this.minThreatPoints, "minThreatPoints", null, false);
			Scribe_Values.Look<float>(ref this.currentThreatPointsFactor, "currentThreatPointsFactor", 1f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x00226E08 File Offset: 0x00225008
		public override string ToString()
		{
			string text = "(";
			text = text + "onDays=" + this.onDays.ToString("0.##");
			text = text + " offDays=" + this.offDays.ToString("0.##");
			text = text + " minSpacingDays=" + this.minSpacingDays.ToString("0.##");
			text = text + " numIncidentsRange=" + this.numIncidentsRange;
			if (this.threatPoints != null)
			{
				text = text + " threatPoints=" + this.threatPoints.Value;
			}
			if (this.minThreatPoints != null)
			{
				text = text + " minThreatPoints=" + this.minThreatPoints.Value;
			}
			if (this.faction != null)
			{
				text = text + " faction=" + this.faction;
			}
			return text + ")";
		}

		// Token: 0x04003C3F RID: 15423
		public AllowedThreatsGeneratorThreats allowedThreats;

		// Token: 0x04003C40 RID: 15424
		public int randSeed;

		// Token: 0x04003C41 RID: 15425
		public float onDays;

		// Token: 0x04003C42 RID: 15426
		public float offDays;

		// Token: 0x04003C43 RID: 15427
		public float minSpacingDays;

		// Token: 0x04003C44 RID: 15428
		public FloatRange numIncidentsRange;

		// Token: 0x04003C45 RID: 15429
		public float? threatPoints;

		// Token: 0x04003C46 RID: 15430
		public float? minThreatPoints;

		// Token: 0x04003C47 RID: 15431
		public float currentThreatPointsFactor = 1f;

		// Token: 0x04003C48 RID: 15432
		public Faction faction;
	}
}
