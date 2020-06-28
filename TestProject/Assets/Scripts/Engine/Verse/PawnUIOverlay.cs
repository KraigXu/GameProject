using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200021A RID: 538
	public class PawnUIOverlay
	{
		// Token: 0x06000F21 RID: 3873 RVA: 0x000564FC File Offset: 0x000546FC
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0005650C File Offset: 0x0005470C
		public void DrawPawnGUIOverlay()
		{
			if (!this.pawn.Spawned || this.pawn.Map.fogGrid.IsFogged(this.pawn.Position))
			{
				return;
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				switch (Prefs.AnimalNameMode)
				{
				case AnimalNameDisplayMode.None:
					return;
				case AnimalNameDisplayMode.TameNamed:
					if (this.pawn.Name == null || this.pawn.Name.Numerical)
					{
						return;
					}
					break;
				case AnimalNameDisplayMode.TameAll:
					if (this.pawn.Name == null)
					{
						return;
					}
					break;
				}
			}
			Vector2 pos = GenMapUI.LabelDrawPosFor(this.pawn, -0.6f);
			GenMapUI.DrawPawnLabel(this.pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
			if (this.pawn.CanTradeNow)
			{
				this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x04000B37 RID: 2871
		private Pawn pawn;

		// Token: 0x04000B38 RID: 2872
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x04000B39 RID: 2873
		private const int PawnStatBarWidth = 32;

		// Token: 0x04000B3A RID: 2874
		private const float ActivityIconSize = 13f;

		// Token: 0x04000B3B RID: 2875
		private const float ActivityIconOffsetY = 12f;
	}
}
