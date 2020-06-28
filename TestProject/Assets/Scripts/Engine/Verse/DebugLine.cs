using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000156 RID: 342
	internal struct DebugLine
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x000347AC File Offset: 0x000329AC
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x000347C3 File Offset: 0x000329C3
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000347ED File Offset: 0x000329ED
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}

		// Token: 0x040007E9 RID: 2025
		public Vector3 a;

		// Token: 0x040007EA RID: 2026
		public Vector3 b;

		// Token: 0x040007EB RID: 2027
		private int deathTick;

		// Token: 0x040007EC RID: 2028
		private SimpleColor color;
	}
}
