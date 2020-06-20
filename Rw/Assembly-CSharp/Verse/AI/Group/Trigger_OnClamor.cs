using System;

namespace Verse.AI.Group
{
	// Token: 0x020005FB RID: 1531
	public class Trigger_OnClamor : Trigger
	{
		// Token: 0x06002A16 RID: 10774 RVA: 0x000F614A File Offset: 0x000F434A
		public Trigger_OnClamor(ClamorDef clamorType)
		{
			this.clamorType = clamorType;
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000F6159 File Offset: 0x000F4359
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Clamor && signal.clamorType == this.clamorType;
		}

		// Token: 0x04001926 RID: 6438
		private ClamorDef clamorType;
	}
}
