using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Need_Comfort : Need_Seeker
	{
		
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

		
		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.9f);
		}

		
		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}

		
		public float lastComfortUsed;

		
		public int lastComfortUseTick;

		
		private const float MinNormal = 0.1f;

		
		private const float MinComfortable = 0.6f;

		
		private const float MinVeryComfortable = 0.7f;

		
		private const float MinExtremelyComfortablee = 0.8f;

		
		private const float MinLuxuriantlyComfortable = 0.9f;

		
		public const int ComfortUseInterval = 10;
	}
}
