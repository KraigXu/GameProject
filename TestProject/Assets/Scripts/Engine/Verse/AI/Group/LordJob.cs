using System;

namespace Verse.AI.Group
{
	
	public abstract class LordJob : IExposable
	{
		
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x0600294B RID: 10571 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x0600294D RID: 10573 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x0600294E RID: 10574 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanBlockHostileVisitors
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600294F RID: 10575 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AddFleeToil
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06002950 RID: 10576 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool OrganizerIsStartingPawn
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x000F4054 File Offset: 0x000F2254
		protected Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		
		public abstract StateGraph CreateGraph();

		
		public virtual void LordJobTick()
		{
		}

		
		public virtual void ExposeData()
		{
		}

		
		public virtual void Cleanup()
		{
		}

		
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		
		public virtual void Notify_BuildingAdded(Building b)
		{
		}

		
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		
		public virtual void Notify_LordDestroyed()
		{
		}

		
		public virtual string GetReport(Pawn pawn)
		{
			return null;
		}

		
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}

		
		public Lord lord;
	}
}
