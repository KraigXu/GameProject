using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class GameCondition_VolcanicWinter : GameCondition
	{
		
		
		public override int TransitionTicks
		{
			get
			{
				return 50000;
			}
		}

		
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.3f);
		}

		
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, this.MaxTempOffset);
		}

		
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		
		private float MaxTempOffset = -7f;

		
		private const float AnimalDensityImpact = 0.5f;

		
		private const float SkyGlow = 0.55f;

		
		private const float MaxSkyLerpFactor = 0.3f;

		
		private SkyColorSet VolcanicWinterColors = new SkyColorSet(new ColorInt(0, 0, 0).ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
	}
}
