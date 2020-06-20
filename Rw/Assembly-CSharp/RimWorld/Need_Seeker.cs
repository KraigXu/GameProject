using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9C RID: 2972
	public abstract class Need_Seeker : Need
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x060045A2 RID: 17826 RVA: 0x001784A8 File Offset: 0x001766A8
		public override int GUIChangeArrow
		{
			get
			{
				if (!this.pawn.Awake())
				{
					return 0;
				}
				float curInstantLevelPercentage = base.CurInstantLevelPercentage;
				if (curInstantLevelPercentage > base.CurLevelPercentage + 0.05f)
				{
					return 1;
				}
				if (curInstantLevelPercentage < base.CurLevelPercentage - 0.05f)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x00176C59 File Offset: 0x00174E59
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x001784F0 File Offset: 0x001766F0
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				float curInstantLevel = this.CurInstantLevel;
				if (curInstantLevel > this.CurLevel)
				{
					this.CurLevel += this.def.seekerRisePerHour * 0.06f;
					this.CurLevel = Mathf.Min(this.CurLevel, curInstantLevel);
				}
				if (curInstantLevel < this.CurLevel)
				{
					this.CurLevel -= this.def.seekerFallPerHour * 0.06f;
					this.CurLevel = Mathf.Max(this.CurLevel, curInstantLevel);
				}
			}
		}

		// Token: 0x0400280D RID: 10253
		private const float GUIArrowTolerance = 0.05f;
	}
}
