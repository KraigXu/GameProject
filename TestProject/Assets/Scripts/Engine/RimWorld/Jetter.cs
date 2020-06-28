using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CA3 RID: 3235
	public class Jetter : Thing
	{
		// Token: 0x06004E3E RID: 20030 RVA: 0x001A4CFC File Offset: 0x001A2EFC
		public override void Tick()
		{
			if (this.JState == Jetter.JetterState.WickBurning)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
				this.WickTicksLeft--;
				if (this.WickTicksLeft == 0)
				{
					this.StartJetting();
					return;
				}
			}
			else if (this.JState == Jetter.JetterState.Jetting)
			{
				this.TicksUntilMove--;
				if (this.TicksUntilMove <= 0)
				{
					this.MoveJetter();
					this.TicksUntilMove = 3;
				}
			}
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x001A4D6E File Offset: 0x001A2F6E
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && dinfo.Def.harmsHealth && this.JState == Jetter.JetterState.Resting)
			{
				this.StartWick();
			}
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x001A4D9C File Offset: 0x001A2F9C
		protected void StartWick()
		{
			this.JState = Jetter.JetterState.WickBurning;
			this.WickTicksLeft = 25;
			SoundDefOf.MetalHitImportant.PlayOneShot(this);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(this);
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x001A4DD3 File Offset: 0x001A2FD3
		protected void StartJetting()
		{
			this.JState = Jetter.JetterState.Jetting;
			this.TicksUntilMove = 3;
			this.wickSoundSustainer.End();
			this.wickSoundSustainer = null;
			this.wickSoundSustainer = SoundDefOf.HissJet.TrySpawnSustainer(this);
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x001A4E0C File Offset: 0x001A300C
		protected void MoveJetter()
		{
			IntVec3 intVec = base.Position + base.Rotation.FacingCell;
			if (!intVec.Walkable(base.Map) || base.Map.thingGrid.CellContains(intVec, ThingCategory.Pawn) || intVec.GetEdifice(base.Map) != null)
			{
				this.Destroy(DestroyMode.Vanish);
				GenExplosion.DoExplosion(base.Position, base.Map, 2.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
				return;
			}
			base.Position = intVec;
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x001A4EB7 File Offset: 0x001A30B7
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
			if (this.jetSoundSustainer != null)
			{
				this.jetSoundSustainer.End();
				this.jetSoundSustainer = null;
			}
		}

		// Token: 0x04002BED RID: 11245
		private Jetter.JetterState JState;

		// Token: 0x04002BEE RID: 11246
		private int WickTicksLeft;

		// Token: 0x04002BEF RID: 11247
		private int TicksUntilMove;

		// Token: 0x04002BF0 RID: 11248
		protected Sustainer wickSoundSustainer;

		// Token: 0x04002BF1 RID: 11249
		protected Sustainer jetSoundSustainer;

		// Token: 0x04002BF2 RID: 11250
		private const int TicksBeforeBeginAccelerate = 25;

		// Token: 0x04002BF3 RID: 11251
		private const int TicksBetweenMoves = 3;

		// Token: 0x02001C0D RID: 7181
		private enum JetterState
		{
			// Token: 0x04006A59 RID: 27225
			Resting,
			// Token: 0x04006A5A RID: 27226
			WickBurning,
			// Token: 0x04006A5B RID: 27227
			Jetting
		}
	}
}
