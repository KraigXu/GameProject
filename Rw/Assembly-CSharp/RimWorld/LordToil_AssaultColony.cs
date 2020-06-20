using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078C RID: 1932
	public class LordToil_AssaultColony : LordToil
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x00118CE1 File Offset: 0x00116EE1
		public LordToil_AssaultColony(bool attackDownedIfStarving = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x00118CF0 File Offset: 0x00116EF0
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x00118D04 File Offset: 0x00116F04
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
			}
		}

		// Token: 0x04001B56 RID: 6998
		private bool attackDownedIfStarving;
	}
}
