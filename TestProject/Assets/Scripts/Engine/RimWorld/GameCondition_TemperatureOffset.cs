using System;
using Verse;

namespace RimWorld
{
	
	public class GameCondition_TemperatureOffset : GameCondition
	{
		
		public override void Init()
		{
			base.Init();
			this.tempOffset = this.def.temperatureOffset;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		
		public float tempOffset;
	}
}
