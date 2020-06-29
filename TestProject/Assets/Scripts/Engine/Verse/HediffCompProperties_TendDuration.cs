using System;
using UnityEngine;

namespace Verse
{
	
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x0005F0FE File Offset: 0x0005D2FE
		public bool TendIsPermanent
		{
			get
			{
				return this.baseTendDurationHours < 0f;
			}
		}

		
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

		
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		
		private float baseTendDurationHours = -1f;

		
		private float tendOverlapHours = 3f;

		
		public bool tendAllAtOnce;

		
		public int disappearsAtTotalTendQuality = -1;

		
		public float severityPerDayTended;

		
		public bool showTendQuality = true;

		
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell;

		
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner;

		
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell;
	}
}
