using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ITab_Pawn_Social : ITab
	{
		
		// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x001FB4A4 File Offset: 0x001F96A4
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh;
			}
		}

		
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

		
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}

		
		public const float Width = 540f;
	}
}
