using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C93 RID: 3219
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x001A194C File Offset: 0x0019FB4C
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06004DA8 RID: 19880 RVA: 0x001A195F File Offset: 0x0019FB5F
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x001A1970 File Offset: 0x0019FB70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x001A19DE File Offset: 0x0019FBDE
		public override void Draw()
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x001A19E8 File Offset: 0x0019FBE8
		public virtual void StartStrike()
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartStrike() on unspawned thing.", false);
				return;
			}
			this.angle = OrbitalStrike.AngleRange.RandomInRange;
			this.startTick = Find.TickManager.TicksGame;
			base.GetComp<CompAffectsSky>().StartFadeInHoldFadeOut(30, this.duration - 30 - 15, 15, 1f);
			base.GetComp<CompOrbitalBeam>().StartAnimation(this.duration, 10, this.angle);
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x001A1A65 File Offset: 0x0019FC65
		public override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04002B71 RID: 11121
		public int duration;

		// Token: 0x04002B72 RID: 11122
		public Thing instigator;

		// Token: 0x04002B73 RID: 11123
		public ThingDef weaponDef;

		// Token: 0x04002B74 RID: 11124
		private float angle;

		// Token: 0x04002B75 RID: 11125
		private int startTick;

		// Token: 0x04002B76 RID: 11126
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x04002B77 RID: 11127
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x04002B78 RID: 11128
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x04002B79 RID: 11129
		private const int OrbitalBeamFadeOutTicks = 10;
	}
}
