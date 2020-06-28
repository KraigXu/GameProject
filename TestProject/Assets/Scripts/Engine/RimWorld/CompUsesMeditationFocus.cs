using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D79 RID: 3449
	public class CompUsesMeditationFocus : ThingComp
	{
		// Token: 0x0600540F RID: 21519 RVA: 0x001C122B File Offset: 0x001BF42B
		public override void PostDrawExtraSelectionOverlays()
		{
			MeditationUtility.DrawMeditationSpotOverlay(this.parent.Position, this.parent.Map);
		}
	}
}
