﻿using System;
using Verse;

namespace RimWorld
{
	
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (Find.TickManager.TicksGame % 30 == 4)
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		
		private const int HeatPushInterval = 30;
	}
}
