using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200021B RID: 539
	public class DamageFlasher
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x000565F9 File Offset: 0x000547F9
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0005660F File Offset: 0x0005480F
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0005661D File Offset: 0x0005481D
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00056630 File Offset: 0x00054830
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00056645 File Offset: 0x00054845
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04000B3C RID: 2876
		private int lastDamageTick = -9999;

		// Token: 0x04000B3D RID: 2877
		private const int DamagedMatTicksTotal = 16;
	}
}
