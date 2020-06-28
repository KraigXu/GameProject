using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x020005EF RID: 1519
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060029FA RID: 10746 RVA: 0x000F5E21 File Offset: 0x000F4021
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060029FB RID: 10747 RVA: 0x000F5E2E File Offset: 0x000F402E
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x000F5E48 File Offset: 0x000F4048
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x000F5E6C File Offset: 0x000F406C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_TicksPassed))
				{
					BackCompatibility.TriggerDataTicksPassedNull(this);
				}
				TriggerData_TicksPassed data = this.Data;
				data.ticksPassed++;
				return data.ticksPassed > this.duration;
			}
			return false;
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000F5EC0 File Offset: 0x000F40C0
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}

		// Token: 0x04001918 RID: 6424
		private int duration = 100;
	}
}
