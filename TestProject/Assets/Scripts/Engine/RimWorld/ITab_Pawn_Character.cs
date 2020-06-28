using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA7 RID: 3751
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06005B8F RID: 23439 RVA: 0x001F86F4 File Offset: 0x001F68F4
		private Pawn PawnToShowInfoAbout
		{
			get
			{
				Pawn pawn = null;
				if (base.SelPawn != null)
				{
					pawn = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						pawn = corpse.InnerPawn;
					}
				}
				if (pawn == null)
				{
					Log.Error("Character tab found no selected pawn to display.", false);
					return null;
				}
				return pawn;
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06005B90 RID: 23440 RVA: 0x001F873B File Offset: 0x001F693B
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x001F874B File Offset: 0x001F694B
		public ITab_Pawn_Character()
		{
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x001F8769 File Offset: 0x001F6969
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout) + new Vector2(17f, 17f) * 2f;
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x001F87A0 File Offset: 0x001F69A0
		protected override void FillTab()
		{
			this.UpdateSize();
			Vector2 vector = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout);
			CharacterCardUtility.DrawCharacterCard(new Rect(17f, 17f, vector.x, vector.y), this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
