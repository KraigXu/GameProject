using System;

namespace RimWorld
{
	// Token: 0x02000EAC RID: 3756
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06005BBC RID: 23484 RVA: 0x001FAC2A File Offset: 0x001F8E2A
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x001FAC37 File Offset: 0x001F8E37
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}
	}
}
