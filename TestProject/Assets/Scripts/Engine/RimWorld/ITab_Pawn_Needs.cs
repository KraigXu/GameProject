using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000EB0 RID: 3760
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06005BCB RID: 23499 RVA: 0x001FB3D8 File Offset: 0x001F95D8
		public override bool IsVisible
		{
			get
			{
				return (!base.SelPawn.RaceProps.Animal || base.SelPawn.Faction != null) && base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x06005BCC RID: 23500 RVA: 0x001FB42D File Offset: 0x001F962D
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x06005BCD RID: 23501 RVA: 0x001FB44B File Offset: 0x001F964B
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x06005BCE RID: 23502 RVA: 0x001FB459 File Offset: 0x001F9659
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x06005BCF RID: 23503 RVA: 0x001FB491 File Offset: 0x001F9691
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}

		// Token: 0x0400322D RID: 12845
		private Vector2 thoughtScrollPosition;
	}
}
