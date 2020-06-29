using System;

namespace RimWorld
{
	
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		
		// (get) Token: 0x06005BBC RID: 23484 RVA: 0x001FAC2A File Offset: 0x001F8E2A
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}

		
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}
	}
}
