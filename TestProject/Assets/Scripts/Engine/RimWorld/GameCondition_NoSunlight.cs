using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class GameCondition_NoSunlight : GameCondition
	{
		
		// (get) Token: 0x06003B5B RID: 15195 RVA: 0x000FAF75 File Offset: 0x000F9175
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, this.EclipseSkyColors, 1f, 0f));
		}

		
		private SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);
	}
}
