using System;

namespace Verse
{
	
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		
		public CameraMapConfig_ContinuousPan()
		{
			this.dollyRateKeys = 10f;
			this.dollyRateScreenEdge = 5f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
			this.minSize = 8.2f;
		}
	}
}
