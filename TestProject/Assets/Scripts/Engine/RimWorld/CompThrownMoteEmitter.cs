using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D70 RID: 3440
	public class CompThrownMoteEmitter : ThingComp
	{
		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x060053BF RID: 21439 RVA: 0x001BFA82 File Offset: 0x001BDC82
		private CompProperties_ThrownMoteEmitter Props
		{
			get
			{
				return (CompProperties_ThrownMoteEmitter)this.props;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x060053C0 RID: 21440 RVA: 0x001BFA90 File Offset: 0x001BDC90
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x060053C1 RID: 21441 RVA: 0x001BFB11 File Offset: 0x001BDD11
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x001BFB34 File Offset: 0x001BDD34
		private bool IsOn
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
				CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
				if (comp2 != null && comp2.ticksLeft <= 0)
				{
					return false;
				}
				Building_MusicalInstrument building_MusicalInstrument = this.parent as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && !building_MusicalInstrument.IsBeingPlayed)
				{
					return false;
				}
				CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
				return comp3 == null || comp3.Initiated;
			}
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x001BFBB8 File Offset: 0x001BDDB8
		public override void CompTick()
		{
			if (!this.IsOn)
			{
				return;
			}
			if (this.Props.emissionInterval == -1)
			{
				if (!this.emittedBefore)
				{
					this.Emit();
					this.emittedBefore = true;
				}
				return;
			}
			if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
			{
				this.Emit();
				this.ticksSinceLastEmitted = 0;
				return;
			}
			this.ticksSinceLastEmitted++;
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x001BFC24 File Offset: 0x001BDE24
		private void Emit()
		{
			for (int i = 0; i < this.Props.burstCount; i++)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(this.Props.mote, null);
				moteThrown.Scale = this.Props.scale.RandomInRange;
				moteThrown.rotationRate = this.Props.rotationRate.RandomInRange;
				moteThrown.exactPosition = this.parent.DrawPos + this.EmissionOffset;
				moteThrown.instanceColor = this.EmissionColor;
				moteThrown.SetVelocity(this.Props.velocityX.RandomInRange, this.Props.velocityY.RandomInRange);
				GenSpawn.Spawn(moteThrown, moteThrown.exactPosition.ToIntVec3(), this.parent.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x001BFCFC File Offset: 0x001BDEFC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x04002E42 RID: 11842
		public bool emittedBefore;

		// Token: 0x04002E43 RID: 11843
		public int ticksSinceLastEmitted;
	}
}
