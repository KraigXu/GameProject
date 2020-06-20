using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8D RID: 2957
	public class Need_Comfort : Need_Seeker
	{
		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x0600455E RID: 17758 RVA: 0x00176EF3 File Offset: 0x001750F3
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastComfortUseTick >= Find.TickManager.TicksGame - 10)
				{
					return Mathf.Clamp01(this.lastComfortUsed);
				}
				return 0f;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x0600455F RID: 17759 RVA: 0x00176F1C File Offset: 0x0017511C
		public ComfortCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.1f)
				{
					return ComfortCategory.Uncomfortable;
				}
				if (this.CurLevel < 0.6f)
				{
					return ComfortCategory.Normal;
				}
				if (this.CurLevel < 0.7f)
				{
					return ComfortCategory.Comfortable;
				}
				if (this.CurLevel < 0.8f)
				{
					return ComfortCategory.VeryComfortable;
				}
				if (this.CurLevel < 0.9f)
				{
					return ComfortCategory.ExtremelyComfortable;
				}
				return ComfortCategory.LuxuriantlyComfortable;
			}
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00176F78 File Offset: 0x00175178
		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.9f);
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x00176FE7 File Offset: 0x001751E7
		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040027B9 RID: 10169
		public float lastComfortUsed;

		// Token: 0x040027BA RID: 10170
		public int lastComfortUseTick;

		// Token: 0x040027BB RID: 10171
		private const float MinNormal = 0.1f;

		// Token: 0x040027BC RID: 10172
		private const float MinComfortable = 0.6f;

		// Token: 0x040027BD RID: 10173
		private const float MinVeryComfortable = 0.7f;

		// Token: 0x040027BE RID: 10174
		private const float MinExtremelyComfortablee = 0.8f;

		// Token: 0x040027BF RID: 10175
		private const float MinLuxuriantlyComfortable = 0.9f;

		// Token: 0x040027C0 RID: 10176
		public const int ComfortUseInterval = 10;
	}
}
