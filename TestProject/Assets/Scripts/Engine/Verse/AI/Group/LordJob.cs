using System;

namespace Verse.AI.Group
{
	// Token: 0x020005C7 RID: 1479
	public abstract class LordJob : IExposable
	{
		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600294B RID: 10571 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600294D RID: 10573 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x0600294E RID: 10574 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanBlockHostileVisitors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600294F RID: 10575 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AddFleeToil
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002950 RID: 10576 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool OrganizerIsStartingPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x000F4054 File Offset: 0x000F2254
		protected Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x06002952 RID: 10578
		public abstract StateGraph CreateGraph();

		// Token: 0x06002953 RID: 10579 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void LordJobTick()
		{
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Cleanup()
		{
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_BuildingAdded(Building b)
		{
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string GetReport(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}

		// Token: 0x040018E5 RID: 6373
		public Lord lord;
	}
}
