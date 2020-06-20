using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002E3 RID: 739
	public class TriggerUnfogged : Thing
	{
		// Token: 0x060014E4 RID: 5348 RVA: 0x0007B50F File Offset: 0x0007970F
		public override void Tick()
		{
			if (base.Spawned)
			{
				if (base.Position.Fogged(base.Map))
				{
					this.everFogged = true;
					return;
				}
				if (this.everFogged)
				{
					this.Activated();
					return;
				}
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0007B54A File Offset: 0x0007974A
		public void Activated()
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0007B570 File Offset: 0x00079770
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.everFogged, "everFogged", false, false);
		}

		// Token: 0x04000DE3 RID: 3555
		public string signalTag;

		// Token: 0x04000DE4 RID: 3556
		private bool everFogged;
	}
}
