using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9F RID: 3231
	public class Gas : Thing
	{
		// Token: 0x06004E13 RID: 19987 RVA: 0x001A4610 File Offset: 0x001A2810
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			for (;;)
			{
				Thing gas = base.Position.GetGas(map);
				if (gas == null)
				{
					break;
				}
				gas.Destroy(DestroyMode.Vanish);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.destroyTick = Find.TickManager.TicksGame + this.def.gas.expireSeconds.RandomInRange.SecondsToTicks();
			this.graphicRotationSpeed = Rand.Range(-this.def.gas.rotationSpeed, this.def.gas.rotationSpeed) / 60f;
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x001A469B File Offset: 0x001A289B
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x001A46C9 File Offset: 0x001A28C9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x04002BE0 RID: 11232
		public int destroyTick;

		// Token: 0x04002BE1 RID: 11233
		public float graphicRotation;

		// Token: 0x04002BE2 RID: 11234
		public float graphicRotationSpeed;
	}
}
