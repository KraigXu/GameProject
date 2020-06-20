using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD4 RID: 3540
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x060055FA RID: 22010 RVA: 0x001C835A File Offset: 0x001C655A
		protected override Color BGColor
		{
			get
			{
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num) * Color.red;
			}
		}

		// Token: 0x060055FB RID: 22011 RVA: 0x001C8388 File Offset: 0x001C6588
		public override void AlertActiveUpdate()
		{
			if (this.lastActiveFrame < Time.frameCount - 1)
			{
				Messages.Message("MessageCriticalAlert".Translate(base.Label.CapitalizeFirst()), new LookTargets(this.GetReport().AllCulprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame = Time.frameCount;
		}

		// Token: 0x060055FC RID: 22012 RVA: 0x001C83EC File Offset: 0x001C65EC
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x04002F0B RID: 12043
		private int lastActiveFrame = -1;

		// Token: 0x04002F0C RID: 12044
		private const float PulseFreq = 0.5f;

		// Token: 0x04002F0D RID: 12045
		private const float PulseAmpCritical = 0.6f;

		// Token: 0x04002F0E RID: 12046
		private const float PulseAmpTutorial = 0.2f;
	}
}
