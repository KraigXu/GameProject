using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C95 RID: 3221
	public abstract class SignalAction : Thing
	{
		// Token: 0x06004DB6 RID: 19894 RVA: 0x001A1C7F File Offset: 0x0019FE7F
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06004DB7 RID: 19895
		protected abstract void DoAction(SignalArgs args);

		// Token: 0x06004DB8 RID: 19896 RVA: 0x001A1CB6 File Offset: 0x0019FEB6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04002B7F RID: 11135
		public string signalTag;
	}
}
