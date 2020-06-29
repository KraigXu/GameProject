using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ITab_Pawn_Character : ITab
	{
		
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

		
		// (get) Token: 0x06005B90 RID: 23440 RVA: 0x001F873B File Offset: 0x001F693B
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		
		public ITab_Pawn_Character()
		{
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout) + new Vector2(17f, 17f) * 2f;
		}

		
		protected override void FillTab()
		{
			this.UpdateSize();
			Vector2 vector = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout);
			CharacterCardUtility.DrawCharacterCard(new Rect(17f, 17f, vector.x, vector.y), this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
