using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005CF RID: 1487
	public abstract class LordToil
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x000F4A36 File Offset: 0x000F2C36
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x000F4A48 File Offset: 0x000F2C48
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002981 RID: 10625 RVA: 0x000F4A50 File Offset: 0x000F2C50
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002982 RID: 10626 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002983 RID: 10627 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002984 RID: 10628 RVA: 0x000F4A68 File Offset: 0x000F2C68
		public virtual bool ShouldFail
		{
			get
			{
				for (int i = 0; i < this.failConditions.Count; i++)
				{
					if (this.failConditions[i]())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002985 RID: 10629 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Init()
		{
		}

		// Token: 0x06002987 RID: 10631
		public abstract void UpdateAllDuties();

		// Token: 0x06002988 RID: 10632 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void LordToilTick()
		{
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000F4AA1 File Offset: 0x000F2CA1
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000F4AB0 File Offset: 0x000F2CB0
		public override string ToString()
		{
			string text = base.GetType().ToString();
			if (text.Contains('.'))
			{
				text = text.Substring(text.LastIndexOf('.') + 1);
			}
			if (text.Contains('_'))
			{
				text = text.Substring(text.LastIndexOf('_') + 1);
			}
			return text;
		}

		// Token: 0x040018F4 RID: 6388
		public Lord lord;

		// Token: 0x040018F5 RID: 6389
		public LordToilData data;

		// Token: 0x040018F6 RID: 6390
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x040018F7 RID: 6391
		public bool useAvoidGrid;
	}
}
