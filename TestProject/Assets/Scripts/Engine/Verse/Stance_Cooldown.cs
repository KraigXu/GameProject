using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A6 RID: 678
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x06001372 RID: 4978 RVA: 0x0006F8BF File Offset: 0x0006DABF
		public Stance_Cooldown()
		{
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0006FB3D File Offset: 0x0006DD3D
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0006FB48 File Offset: 0x0006DD48
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x04000D1F RID: 3359
		private const float RadiusPerTick = 0.002f;

		// Token: 0x04000D20 RID: 3360
		private const float MaxRadius = 0.5f;
	}
}
