using System;
using UnityEngine;

namespace Verse.Sound
{
	
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x000D91CE File Offset: 0x000D73CE
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x000D91E5 File Offset: 0x000D73E5
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		
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

		
		private EchoFilterProperty filterProperty;
	}
}
