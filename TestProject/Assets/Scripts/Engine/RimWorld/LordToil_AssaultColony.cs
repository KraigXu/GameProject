using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_AssaultColony : LordToil
	{
		
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		
		public LordToil_AssaultColony(bool attackDownedIfStarving = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
		}

		
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
			}
		}

		
		private bool attackDownedIfStarving;
	}
}
