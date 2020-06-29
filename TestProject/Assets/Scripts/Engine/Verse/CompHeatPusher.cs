using System;

namespace Verse
{
	
	public class CompHeatPusher : ThingComp
	{
		
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x0008583C File Offset: 0x00083A3C
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x0008584C File Offset: 0x00083A4C
		protected virtual bool ShouldPushHeatNow
		{
			get
			{
				if (!this.parent.SpawnedOrAnyParentSpawned)
				{
					return false;
				}
				CompProperties_HeatPusher props = this.Props;
				float ambientTemperature = this.parent.AmbientTemperature;
				return ambientTemperature < props.heatPushMaxTemperature && ambientTemperature > props.heatPushMinTemperature;
			}
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.16666651f);
			}
		}

		
		private const int HeatPushInterval = 60;
	}
}
