using System;

namespace RimWorld
{
	// Token: 0x02000EAB RID: 3755
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06005BBA RID: 23482 RVA: 0x001FABE8 File Offset: 0x001F8DE8
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x001FAC0C File Offset: 0x001F8E0C
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}
	}
}
