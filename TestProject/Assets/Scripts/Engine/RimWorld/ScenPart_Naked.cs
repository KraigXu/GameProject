using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0F RID: 3087
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x06004988 RID: 18824 RVA: 0x0018F340 File Offset: 0x0018D540
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(this.context.ToStringHuman()).CapitalizeFirst();
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x0018F374 File Offset: 0x0018D574
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x0018F38C File Offset: 0x0018D58C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
