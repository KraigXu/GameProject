using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D30 RID: 3376
	public class CompMoteEmitterProximityScan : CompMoteEmitter
	{
		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06005205 RID: 20997 RVA: 0x001B67C3 File Offset: 0x001B49C3
		private CompProperties_MoteEmitterProximityScan Props
		{
			get
			{
				return (CompProperties_MoteEmitterProximityScan)this.props;
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005206 RID: 20998 RVA: 0x001B67D0 File Offset: 0x001B49D0
		private CompSendSignalOnPawnProximity ProximityComp
		{
			get
			{
				CompSendSignalOnPawnProximity result;
				if ((result = this.proximityCompCached) == null)
				{
					result = (this.proximityCompCached = this.parent.GetComp<CompSendSignalOnPawnProximity>());
				}
				return result;
			}
		}

		// Token: 0x06005207 RID: 20999 RVA: 0x001B67FC File Offset: 0x001B49FC
		public override void CompTick()
		{
			if (this.ProximityComp == null || this.ProximityComp.Sent)
			{
				return;
			}
			if (this.mote == null)
			{
				base.Emit();
			}
			if (this.mote == null)
			{
				return;
			}
			Mote mote = this.mote;
			if (mote != null)
			{
				mote.Maintain();
			}
			float a;
			if (!this.ProximityComp.Enabled)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
				float num = (float)this.ticksSinceLastEmitted / 60f;
				if (num <= this.Props.warmupPulseFadeInTime)
				{
					if (this.Props.warmupPulseFadeInTime > 0f)
					{
						a = num / this.Props.warmupPulseFadeInTime;
					}
					else
					{
						a = 1f;
					}
				}
				else if (num <= this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime)
				{
					a = 1f;
				}
				else if (this.Props.warmupPulseFadeOutTime > 0f)
				{
					a = 1f - Mathf.InverseLerp(this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime, this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime + this.Props.warmupPulseFadeOutTime, num);
				}
				else
				{
					a = 1f;
				}
			}
			else
			{
				a = 1f;
			}
			this.mote.instanceColor.a = a;
		}

		// Token: 0x04002D36 RID: 11574
		private CompSendSignalOnPawnProximity proximityCompCached;
	}
}
