using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB1 RID: 3761
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x001FB4A4 File Offset: 0x001F96A4
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh;
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06005BD1 RID: 23505 RVA: 0x001FB4B8 File Offset: 0x001F96B8
		private Pawn SelPawnForSocialInfo
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				throw new InvalidOperationException("Social tab on non-pawn non-corpse " + base.SelThing);
			}
		}

		// Token: 0x06005BD2 RID: 23506 RVA: 0x001FB4FF File Offset: 0x001F96FF
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x06005BD3 RID: 23507 RVA: 0x001FB532 File Offset: 0x001F9732
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}

		// Token: 0x0400322E RID: 12846
		public const float Width = 540f;
	}
}
