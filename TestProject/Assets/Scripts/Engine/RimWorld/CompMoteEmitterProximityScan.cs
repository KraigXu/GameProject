﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompMoteEmitterProximityScan : CompMoteEmitter
	{
		
		// (get) Token: 0x06005205 RID: 20997 RVA: 0x001B67C3 File Offset: 0x001B49C3
		private CompProperties_MoteEmitterProximityScan Props
		{
			get
			{
				return (CompProperties_MoteEmitterProximityScan)this.props;
			}
		}

		
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

		
		private CompSendSignalOnPawnProximity proximityCompCached;
	}
}
