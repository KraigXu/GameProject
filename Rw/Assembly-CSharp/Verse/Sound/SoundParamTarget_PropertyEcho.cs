using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004F1 RID: 1265
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x000D91CE File Offset: 0x000D73CE
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x000D91E5 File Offset: 0x000D73E5
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000D91F4 File Offset: 0x000D73F4
		public override void SetOn(Sample sample, float value)
		{
			AudioEchoFilter audioEchoFilter = sample.source.GetComponent<AudioEchoFilter>();
			if (audioEchoFilter == null)
			{
				audioEchoFilter = sample.source.gameObject.AddComponent<AudioEchoFilter>();
			}
			if (this.filterProperty == EchoFilterProperty.Delay)
			{
				audioEchoFilter.delay = value;
			}
			if (this.filterProperty == EchoFilterProperty.DecayRatio)
			{
				audioEchoFilter.decayRatio = value;
			}
			if (this.filterProperty == EchoFilterProperty.WetMix)
			{
				audioEchoFilter.wetMix = value;
			}
			if (this.filterProperty == EchoFilterProperty.DryMix)
			{
				audioEchoFilter.dryMix = value;
			}
		}

		// Token: 0x04001617 RID: 5655
		private EchoFilterProperty filterProperty;
	}
}
