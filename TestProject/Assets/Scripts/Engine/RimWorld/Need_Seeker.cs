using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class Need_Seeker : Need
	{
		
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

		
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		
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

		
		private const float GUIArrowTolerance = 0.05f;
	}
}
