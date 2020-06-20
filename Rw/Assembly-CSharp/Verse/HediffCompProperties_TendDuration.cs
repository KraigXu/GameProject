using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000271 RID: 625
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x0005F0FE File Offset: 0x0005D2FE
		public bool TendIsPermanent
		{
			get
			{
				return this.baseTendDurationHours < 0f;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0005F10D File Offset: 0x0005D30D
		public int TendTicksFull
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksFull on permanent-tend Hediff.", 6163263, false);
				}
				return Mathf.RoundToInt((this.baseTendDurationHours + this.tendOverlapHours) * 2500f);
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x0005F13F File Offset: 0x0005D33F
		public int TendTicksBase
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksBase on permanent-tend Hediff.", 61621263, false);
				}
				return Mathf.RoundToInt(this.baseTendDurationHours * 2500f);
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0005F16A File Offset: 0x0005D36A
		public int TendTicksOverlap
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksOverlap on permanent-tend Hediff.", 1963263, false);
				}
				return Mathf.RoundToInt(this.tendOverlapHours * 2500f);
			}
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0005F195 File Offset: 0x0005D395
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x04000C2C RID: 3116
		private float baseTendDurationHours = -1f;

		// Token: 0x04000C2D RID: 3117
		private float tendOverlapHours = 3f;

		// Token: 0x04000C2E RID: 3118
		public bool tendAllAtOnce;

		// Token: 0x04000C2F RID: 3119
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x04000C30 RID: 3120
		public float severityPerDayTended;

		// Token: 0x04000C31 RID: 3121
		public bool showTendQuality = true;

		// Token: 0x04000C32 RID: 3122
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell;

		// Token: 0x04000C33 RID: 3123
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner;

		// Token: 0x04000C34 RID: 3124
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell;
	}
}
