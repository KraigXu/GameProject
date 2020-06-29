using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	
	public abstract class LordToil
	{
		
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x000F4A36 File Offset: 0x000F2C36
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x000F4A48 File Offset: 0x000F2C48
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06002981 RID: 10625 RVA: 0x000F4A50 File Offset: 0x000F2C50
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06002982 RID: 10626 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06002983 RID: 10627 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		
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

		
		// (get) Token: 0x06002985 RID: 10629 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		
		public virtual void Init()
		{
		}

		
		public abstract void UpdateAllDuties();

		
		public virtual void LordToilTick()
		{
		}

		
		public virtual void Cleanup()
		{
		}

		
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		
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

		
		public Lord lord;

		
		public LordToilData data;

		
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		
		public bool useAvoidGrid;
	}
}
