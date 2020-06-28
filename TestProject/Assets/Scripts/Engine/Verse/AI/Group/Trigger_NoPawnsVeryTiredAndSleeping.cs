using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200060C RID: 1548
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x06002A3A RID: 10810 RVA: 0x000F677E File Offset: 0x000F497E
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000F6790 File Offset: 0x000F4990
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Need_Rest rest = lord.ownedPawns[i].needs.rest;
					if (rest != null && rest.CurLevelPercentage < 0.14f + this.extraRestThreshOffset && !lord.ownedPawns[i].Awake())
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04001932 RID: 6450
		private float extraRestThreshOffset;
	}
}
