using System;

namespace Verse
{
	// Token: 0x0200031F RID: 799
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x0008583C File Offset: 0x00083A3C
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x170004D7 RID: 1239
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

		// Token: 0x0600175B RID: 5979 RVA: 0x00085890 File Offset: 0x00083A90
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000858E1 File Offset: 0x00083AE1
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.16666651f);
			}
		}

		// Token: 0x04000EA4 RID: 3748
		private const int HeatPushInterval = 60;
	}
}
