using System;

namespace RimWorld
{
	
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		
		// (get) Token: 0x06005BBA RID: 23482 RVA: 0x001FABE8 File Offset: 0x001F8DE8
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}

		
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}
	}
}
