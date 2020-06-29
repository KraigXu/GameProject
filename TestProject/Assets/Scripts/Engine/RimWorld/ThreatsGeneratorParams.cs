using System;
using Verse;

namespace RimWorld
{
	
	public class ThreatsGeneratorParams : IExposable
	{
		
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

		
		public AllowedThreatsGeneratorThreats allowedThreats;

		
		public int randSeed;

		
		public float onDays;

		
		public float offDays;

		
		public float minSpacingDays;

		
		public FloatRange numIncidentsRange;

		
		public float? threatPoints;

		
		public float? minThreatPoints;

		
		public float currentThreatPointsFactor = 1f;

		
		public Faction faction;
	}
}
